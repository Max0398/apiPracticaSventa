namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface IRolService
    {
        Task<List<RolDTO>> ListaRoles();
    }
}
