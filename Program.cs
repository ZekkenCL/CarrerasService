using DotNetEnv;
using System;
using MongoDB.Driver;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using CarrerasService.Services;
using CarrerasService.Config;
using CarreraService.Config;
using CarreraService.Services;
using CarrerasService;


var builder = WebApplication.CreateBuilder(args);


Env.Load();


builder.Services.Configure<DatabaseSettings>(options =>
{
    options.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_URI")!;
    options.DatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE")!;
    options.SubjectsCollectionName = "Subjects";
    options.SubjectRelationshipsCollectionName = "SubjectRelationships";
});


builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);


builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<DatabaseSettings>();
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<DatabaseSettings>();
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});
builder.Services.Configure<RabbitMQSettings>(options =>
{
    options.Host = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
    options.Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672");
    options.User = Environment.GetEnvironmentVariable("RABBITMQ_USER");
    options.Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
    options.Queue = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE");
    options.Exchange = Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE");
    options.RoutingKey = Environment.GetEnvironmentVariable("RABBITMQ_ROUTING_KEY");
});

builder.Services.AddSingleton<RabbitMQConnection>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
builder.Services.AddHostedService<RabbitMQConsumer>();

builder.Services.AddSingleton<SubjectsRepository>();
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddGrpc();


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5190, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeCollectionAsync();




app.MapGrpcService<CarrerasGrpcService>();
app.MapGrpcService<SubjectsGrpcService>();
app.MapGet("/", () => "El servicio de Carreras está en ejecución con gRPC.");

app.Run();
