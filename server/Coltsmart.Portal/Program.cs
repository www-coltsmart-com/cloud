using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace coltsmart.server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseKestrel(o =>
                   {
                       o.ListenAnyIP(5000);
                   })
                .UseStartup<Startup>();
    }
}
