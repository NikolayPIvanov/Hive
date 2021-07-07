using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Hive.Identity.Areas.Identity.IdentityHostingStartup))]
namespace Hive.Identity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}