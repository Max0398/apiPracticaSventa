using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Respositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private IMapper _mapper;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> ListaUsuarios()
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));

                if(usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("El Usuario No se Pudo crear");

                var query = await _usuarioRepositorio.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(usuarioCreado);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El Usuario No Existe");

                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

                var respuesta = await _usuarioRepositorio.Editar(usuarioEncontrado);

                if(!respuesta)
                    throw new TaskCanceledException("No se Pudo Realizar la Accion de Editar");

                return respuesta;



            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var usuarioEncontardo = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);

                if (usuarioEncontardo == null)
                    throw new TaskCanceledException("El Usuario No Existe");

                bool respuesta = await _usuarioRepositorio.Eliminar(usuarioEncontardo);

                if (!respuesta)
                    throw new TaskCanceledException("No se Pudo Eliminar el Usuario");

                return respuesta;   
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {
                var queryUsuario= await _usuarioRepositorio.Consultar(u=> u.Correo == correo && u.Clave == clave);

                //si el usuario no existe
                if (queryUsuario.FirstOrDefault() == null)
                    throw new TaskCanceledException("EL Usuario no Existe ");

               Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).FirstOrDefault();
                return _mapper.Map<SesionDTO>(devolverUsuario);


            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}
