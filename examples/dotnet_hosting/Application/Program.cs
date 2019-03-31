using System;
using System.Threading.Tasks;
using Grpc.Core;
using Store;
using Grpc.Host;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    class StoreImpl : Store.Store.StoreBase
    {
        public override Task<StoreRes> GetStore(StoreReq req, ServerCallContext context) {
            return Task.FromResult(new StoreRes {Message = "hello"});
        }
    }
    class Program
    {
        const int Port = 50051;
        static async Task Main(string[] args)
        {

            var hostBuilder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                Server server = new Server
                {
                    Services = {Store.Store.BindService(new StoreImpl())},
                    Ports = {new ServerPort("localhost", Port, ServerCredentials.Insecure)}
                };
                services.AddSingleton<Server>(server);
                services.AddSingleton<IHostedService, GrpcHostedService>();
            });
            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            await hostBuilder.RunConsoleAsync();
        }
    }
}


