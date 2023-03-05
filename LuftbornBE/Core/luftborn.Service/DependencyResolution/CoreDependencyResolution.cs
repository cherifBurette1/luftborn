using Microsoft.Extensions.DependencyInjection;
using luftborn.Ground;
using System;

namespace luftborn.Service.DependencyResolution
{
    public static class CoreDependencyResolution
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services)
        {
            var assembly = typeof(CoreDependencyResolution).Assembly;

            //inject by convention
            services.AddTransientByConvention(assembly, x => x.Name.EndsWith("Service") && x.Name != "IIdentityUserService");
            //services.AddTransientByConvention(assembly, x => x.Name.EndsWith("Notifier"));

            //inject lazy
            services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));

            return services;
        }
        internal class Lazier<T> : Lazy<T> where T : class
        {
            public Lazier(IServiceProvider provider)
                : base(() => provider.GetRequiredService<T>())
            {
            }
        }
    }

}
