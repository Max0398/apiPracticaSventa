using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Respositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SistemaVenta.BLL.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepositorio;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepositorio, IGenericRepository<DetalleVenta> detalleVentaRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventaGenerada = await _ventaRepositorio.Registrar(_mapper.Map<Venta>(modelo));
                if (ventaGenerada.IdVenta == 0)
                    throw new TaskCanceledException("No se Pudo Registrar la Venta");
                return _mapper.Map<VentaDTO>(ventaGenerada);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepositorio.Consultar();
            var listaResultado = new List<Venta>();

            try
            {
                if (buscarPor == "fecha")
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yy", new CultureInfo("es-Ni"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yy", new CultureInfo("es-Ni"));

                    listaResultado = await query.Where(v => v.FechaRegistro.Value.Date >= fecha_Inicio.Date &&
                    v.FechaRegistro.Value.Date <= fecha_Fin.Date)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
                else
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yy", new CultureInfo("es-Ni"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yy", new CultureInfo("es-Ni"));

                    listaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta)
                        .Include(dv => dv.DetalleVenta)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return _mapper.Map<List<VentaDTO>>(listaResultado);
        }

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepositorio.Consultar();
            var listaResultados = new List<DetalleVenta>();
            try
            {
                DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yy", new CultureInfo("es-Ni"));
                DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yy", new CultureInfo("es-Ni"));

                listaResultados = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv =>
                    dv.IdVentaNavigation.FechaRegistro.Value.Date >= fecha_Inicio.Date &&
                    dv.IdVentaNavigation.FechaRegistro.Value.Date <= fecha_Fin.Date).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(listaResultados);
        }
    }
}
