using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DTO;
using SistemaVenta.BLL.Servicios.Contrato;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        [Route("ListaRoles")]
        public async Task<IActionResult> ListaRoles()
        {
            var response = new Response<List<RolDTO>>();

            try
            {
                response.status = true;
                response.value = await _rolService.ListaRoles();

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
