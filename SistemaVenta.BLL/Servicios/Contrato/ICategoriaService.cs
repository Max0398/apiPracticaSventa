namespace SistemaVenta.BLL.Servicios.Contrato
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDTO>> listaCategorias();
    }
}
