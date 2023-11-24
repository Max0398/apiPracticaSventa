using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Respositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService:IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ProductoDTO>> listaProductos()
        {
            try
            {
                var queryProductos = await _productoRepositorio.Consultar();
                var listaProductos = queryProductos.Include(cat => cat.IdCategoriaNavigation).ToList();

                return _mapper.Map<List<ProductoDTO>>(listaProductos.ToList());

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var productoCreado = await _productoRepositorio.Crear(_mapper.Map<Producto>(modelo));
                
                if(productoCreado.IdProducto == 0)
                    throw new TaskCanceledException("No se Pudo Crear");

                return _mapper.Map<ProductoDTO>(productoCreado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(modelo);
                var productoEncontrado = await _productoRepositorio.Obtener(u => u.IdProducto == productoModelo.IdProducto);

                if (productoEncontrado == null)
                    throw new TaskCanceledException("El Producto No Existe");

                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = productoModelo.Precio;
                productoEncontrado.EsActivo = productoModelo.EsActivo;

                bool respuesta = await _productoRepositorio.Editar(productoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se Pudo Editar");

                return respuesta;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            var productoEncontrado = await _productoRepositorio.Obtener(p => p.IdProducto == id);

            if (productoEncontrado == null) throw new TaskCanceledException("No se Encontro el Producto con Id Solicitado");

            bool respuesta = await _productoRepositorio.Eliminar(productoEncontrado);

            if(!respuesta)
                throw new TaskCanceledException("No se Pudo Eliminar");

            return respuesta;
        }

      
    }
}
