using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using SIstemaVenta.BLL.Servicios.Contrato;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("ListaUsuarios")]
        public async Task<IActionResult> ListaUsuarios()
        {
            var response = new Response<List<UsuarioDTO>>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.ListaUsuarios();

            }
            catch (Exception ex)
            {

                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody]LoginDTO login)
        {
            var response = new Response<SesionDTO>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);

            }
            catch ( Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> GuardarUsuario([FromBody] UsuarioDTO usuario)
        {
            var response = new Response<UsuarioDTO>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Crear(usuario);
            }
            catch (Exception ex)
            {

                response.status = false;
                response.message=ex.Message;
            }
            return Ok(response);
        }

        [HttpPut]
        [Route ("Editar")]
        public async Task<IActionResult> EditarUsuario([FromBody] UsuarioDTO usuario)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Editar(usuario);
            }
            catch (Exception ex)
            {

                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.value = await _usuarioService.Eliminar(id);
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
