using GYM_ITM.Models.Observer;

namespace GYM_ITM.Controllers.Observer
{
    public static class ObserverConfiguration
    {
        public static void Configuration(IServiceCollection services) {
            services.AddScoped<INotifier, HorariosController>();
            services.AddScoped<ISuscriber, UsuariosController>();
        }

    }
}
