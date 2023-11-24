using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DTO;
using SistemaVenta.BLL.Servicios.Contrato;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _cateoService;

        public CategoriaController(ICategoriaService cateoService)
        {
            _cateoService = cateoService;
        }

        [HttpGet]
        [Route("ListaCategorias")]
        public async Task<IActionResult> ListaCategorias()
        {
            var response = new Response<List<CategoriaDTO>>();

            try
            {
                response.status = true;
                response.value= await _cateoService.listaCategorias();

            }
            catch (Exception ex)
            {
                response.status = false;
                response.message= ex.Message;   
            }
            return Ok(response);
        }
    }
}
