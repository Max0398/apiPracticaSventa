using SistemaVenta.Model;

namespace SistemaVenta.DAL.Respositorios.Contrato
{
    public interface IVentaRepository:IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta modelo);
    }
}
