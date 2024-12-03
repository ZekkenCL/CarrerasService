using DotNetEnv;
using System;
using MongoDB.Driver;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CarrerasService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables desde .env
Env.Load();

string mongoDbUri = Environment.GetEnvironmentVariable("MONGODB_URI")!;
string mongoDbName = Environment.GetEnvironmentVariable("MONGODB_DATABASE")!;
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoDbUri));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbName));

builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5190, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

var database = app.Services.GetRequiredService<IMongoDatabase>();
var initializer = new DatabaseInitializer(database);
await initializer.InitializeCollectionAsync(); 

app.MapGrpcService<CarrerasGrpcService>();
app.MapGet("/", () => "El servicio de Carreras está en ejecución con gRPC.");

app.Run();
