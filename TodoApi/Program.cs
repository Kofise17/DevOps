using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TodoApi.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddDbContext<TodoContext>(options =>
                        options.UseInMemoryDatabase("TodoDb"));
                })
                .Build();

            host.Run();
        }
    }
}
