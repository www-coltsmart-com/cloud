using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
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
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseStartup<Startup>();
    }
}
