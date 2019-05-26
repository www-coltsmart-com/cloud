using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace ColtSmart
{
    public static class ServiceProviderHostingExtension
    {
        public static IApplicationBuilder UseGlobalAppServiceProvider(this IApplicationBuilder app, IConfiguration configuration)
        {
            EnjoyGlobals.SetGlobalAppServiceProvider(app.ApplicationServices);

            return app;
        }
    }
}
