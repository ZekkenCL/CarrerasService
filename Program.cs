using DotNetEnv;
using System;
using MongoDB.Driver;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CarrerasService.Services;
using CarrerasService.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
