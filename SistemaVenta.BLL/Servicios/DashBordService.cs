using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Primitives;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Respositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SistemaVenta.BLL.Servicios
{
    public class DashBordService:IDashBoardService
    {

        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public DashBordService(IVentaRepository ventaRepositorio, IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashBoard = new DashBoardDTO();

            try
            {
                vmDashBoard.TotalVentas = await TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana();
                vmDashBoard.TotalProductos = await TotalProductos();

                List<VentasSemanaDTO> listaVentasSemana = new List<VentasSemanaDTO>();

                foreach (KeyValuePair <string,int> item  in await VentasUltimSemana())
                {
                    listaVentasSemana.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value,
                    });
                }
                vmDashBoard.VentasUltimaSemana = listaVentasSemana;
            }
            catch (Exception)
            {

                throw;
            }
            return vmDashBoard;

        }

        private IQueryable<Venta>retornarVentas(IQueryable<Venta>tablaVenta, int restarCantidadDias)
        {
            DateTime? ultimaFecha = tablaVenta.OrderBy(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);

            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }

        private async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if(_ventaQuery.Count() > 0)
            {
                var tablaVenta = retornarVentas(_ventaQuery, -7);
            }
            return total;

        }

        private async Task<string> TotalIngresosUltimaSemana()
        {
            decimal resultado = 0;

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                var tablaventa = retornarVentas(_ventaQuery, -7);
                resultado = tablaventa.Select(v => v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-NI"));
        }

        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> _productoQuery = await _productoRepositorio.Consultar();

            int total = _productoQuery.Count();
            return total;
        }

        private async Task<Dictionary<string, int>> VentasUltimSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
        
            if(_ventaQuery.Count() > 0)
            {
                var tablaVentas = retornarVentas(_ventaQuery, -7);

                resultado = tablaVentas
                    .GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key)
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() }) 
                    .ToDictionary(keySelector:r =>r.fecha,elementSelector: r => r.total);
            }
            return resultado;
        }

    }
}
