using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CarrerasService
{
    public class DatabaseInitializer
    {
        private readonly IMongoDatabase _database;

        public DatabaseInitializer(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task InitializeCollectionAsync()
{
    var carrerasCollection = _database.GetCollection<MongoCarrera>("Carreras");


    await carrerasCollection.InsertManyAsync(new[]
    {
        new MongoCarrera { Name = "Ingeniería en Sistemas" }
    });

    Console.WriteLine("Datos de prueba insertados en la colección 'Carreras'.");
}

    }
}
