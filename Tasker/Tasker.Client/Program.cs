using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tasker.API;
using Tasker.Client.Services;
using Tasker.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

await builder.Build().RunAsync();