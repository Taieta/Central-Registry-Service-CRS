using AmsGrpc.Extensions;
using AmsGrpc.Interceptors;
using AmsKafka.Extensions;
using Application.Extensions;
using GatewayGrpc.Extensions;
using MdsKafka.Extensions;
using Persistence.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddMdsKafka(builder.Configuration)
    .AddAmsKafka(builder.Configuration)
    .AddGrpc(options =>
    {
        options.Interceptors.Add<ErrorHandlingInterceptor>();
    });
WebApplication app = builder.Build();

app.Services.RunMigrations();
app.MapGatewayGrpc();
app.MapAmsGrpc();

await app.RunAsync();