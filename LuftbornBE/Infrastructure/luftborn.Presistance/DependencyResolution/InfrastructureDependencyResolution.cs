using Dapper;
using Microsoft.Extensions.DependencyInjection;
using luftborn.Ground;
using luftborn.Presistance.Features.Common.Implementation;
using luftborn.Presistance.Infrastructure;
using luftborn.Service.DependencyResolution;
using luftborn.Service.Features.Common.Interfaces;
using System;

namespace luftborn.Presistance.DependencyResolution
{
    public static class InfrastructureDependencyResolution
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var interfaceAssembly = typeof(CoreDependencyResolution).Assembly;
            var implementationAssembly = typeof(InfrastructureDependencyResolution).Assembly;

            //inject by convention
            services.AddTransientByConvention(interfaceAssembly, implementationAssembly, x => x.Name.EndsWith("Factory"));
            services.AddTransientByConvention(interfaceAssembly, implementationAssembly, x => x.Name.EndsWith("Helper") && x.Name != "IMimeTypeHelper");
            services.AddTransientByConvention(interfaceAssembly, implementationAssembly, x => x.Name.EndsWith("Repository"));
            services.AddTransientByConvention(interfaceAssembly, implementationAssembly, x => x.Name.EndsWith("Validator"));
            services.AddTransientByConvention(interfaceAssembly, implementationAssembly, x => x.Name.EndsWith("Validators"));
            services.AddTransientByConvention(interfaceAssembly, implementationAssembly, x => x.Name.EndsWith("Strategy"));
            services.AddTransientByConvention(interfaceAssembly, implementationAssembly, x => x.Name.EndsWith("Director"));

            //inject UnitOfWork
            services.AddTransient<IUnitOfWork, UnitOfWork>();


            //inject lazy
            services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));

            SqlMapper.AddTypeHandler(new DateTimeHandler());

            //var provider = new FileExtensionContentTypeProvider();
            //provider.Mappings.Add(".dnct", "application/dotnetcoretutorials");

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
