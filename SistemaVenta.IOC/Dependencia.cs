using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Respositorios;
using SistemaVenta.DAL.Respositorios.Contrato;
using SistemaVenta.Model;
using SistemaVenta.Utility;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        //Para Agrega las Dependencias Para Luego ser Usadas en Program PAra todo el proyecto
        public static void InyectarDependencias(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DbventaContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConexionSql"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository,VentaRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService,UsuarioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();  
            services.AddScoped<IDashBoardService,DashBordService>();
            services.AddScoped<IMenuService, MenuService>();

        }
    }
}
