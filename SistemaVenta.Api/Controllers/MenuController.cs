using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Route("ListaMenu")]
        public async Task<IActionResult> ListaMenu(int idUsuario)
        {
            var response = new Response<List<MenuDTO>>();
            try
            {
                response.status = true;
                response.value = await _menuService.ListaMenu(idUsuario);

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
