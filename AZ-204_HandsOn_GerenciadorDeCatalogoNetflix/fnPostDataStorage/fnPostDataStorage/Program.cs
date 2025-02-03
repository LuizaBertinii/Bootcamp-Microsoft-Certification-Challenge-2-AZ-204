using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.Configure<KestrelServerOptions>(Options =>
{
    Options.Limits.MaxRequestBodySize = 1024 * 1024 * 100;
});

builder.Build().Run();
