using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders.Implementation;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.UnitTest")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core
{
    public static class Configuration
    {
        public static IServiceCollection RegisterCore(this IServiceCollection services, IConfiguration configuration)
        {
            // register Data Providers
            services.AddScoped<ISatelliteDataProvider, SatelliteDataProvider>();

            // register Domains
            services.AddScoped<ITrilaterationDomain, TrilaterationDomain>();
            services.AddScoped<IMessageDomain, MessageDomain>();

            // return the same IServiceCollection (for fluent writing)
            return services;
        }
    }
}
