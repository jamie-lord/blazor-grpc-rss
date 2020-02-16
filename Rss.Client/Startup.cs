using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcGreeter;
using GrpcItems;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;

namespace Rss.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(services =>
            {
                // Create a gRPC-Web channel pointing to the backend server
                var wasmHttpMessageHandlerType = Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");
                var wasmHttpMessageHandler = (HttpMessageHandler)Activator.CreateInstance(wasmHttpMessageHandlerType);
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, wasmHttpMessageHandler));
                var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });

                // Now we can instantiate gRPC clients for this channel
                return new Greeter.GreeterClient(channel);
            });

            services.AddSingleton(services =>
            {
                // Create a gRPC-Web channel pointing to the backend server
                var wasmHttpMessageHandlerType = Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");
                var wasmHttpMessageHandler = (HttpMessageHandler)Activator.CreateInstance(wasmHttpMessageHandlerType);
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, wasmHttpMessageHandler));
                var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpClient = httpClient });

                // Now we can instantiate gRPC clients for this channel
                return new Items.ItemsClient(channel);
            });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
