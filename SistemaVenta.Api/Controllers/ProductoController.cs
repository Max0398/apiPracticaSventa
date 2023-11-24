using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.DTO;
using SIstemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.API.Utilidad;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        [Route("ListaProductos")]
        public async Task<IActionResult> ListaUsuarios()
        {
            var response = new Response<List<ProductoDTO>>();
            try
            {
                response.status = true;
                response.value = await _productoService.listaProductos();

            }
            catch (Exception ex)
            {

                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("GuardarProducto")]
        public async Task<IActionResult> GuardarProducto([FromBody] ProductoDTO producto)
        {
            var response = new Response<ProductoDTO>();

            try
            {
                response.status = true;
                response.value = await _productoService.Crear(producto);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }


        [HttpPut]
        [Route("EditarProducto")]
        public async Task<IActionResult> EditarProducto([FromBody] ProductoDTO producto)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _productoService.Editar(producto);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }


        [HttpDelete]
        [Route("EliminarProducto/{id:int}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _productoService.Eliminar(id);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);

        }

    }
}
