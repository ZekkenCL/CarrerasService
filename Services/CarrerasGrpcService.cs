using Grpc.Core;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;

namespace CarrerasService
{
    public class CarrerasGrpcService : Carreras.CarrerasBase
    {
        private readonly IMongoCollection<Carrera> _carrerasCollection;

        public CarrerasGrpcService(IMongoDatabase database)
        {
            _carrerasCollection = database.GetCollection<Carrera>("Carreras");
        }

        public override async Task<CarreraList> GetAll(Empty request, ServerCallContext context)
{
    var carreras = await _carrerasCollection
        .Find(_ => true)
        .Project(c => c.Name) // Solo selecciona el campo "Name"
        .ToListAsync();

    var response = new CarreraList();

    foreach (var name in carreras)
    {
        response.Carreras.Add(new Carrera
        {
            Name = name
        });
    }

    return response;
}



    }

}
