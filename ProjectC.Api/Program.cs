using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ProjectC.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
            //.UseHttpSys(x => x.MaxRequestBodySize = 52428800)
            //.UseKestrel(options =>
            //    {
            //        options.Limits.MaxRequestBodySize = 52428800; //50MB
            //    })
            ;
    }
}