using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarrerasService.Models;

namespace CarrerasService.Services
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
            // Inicializar Subjects
            var subjectsCollection = _database.GetCollection<Subject>("Subjects");
            if (await subjectsCollection.CountDocumentsAsync(FilterDefinition<Subject>.Empty) == 0)
            {
                var subjects = new List<Subject>
                {
                    new Subject { Id = 1, Name = "Matemáticas I", Department = "Ciencias Básicas", Credits = 4, Semester = "1", InternalId = "MAT101", Code = "MAT1" },
                    new Subject { Id = 2, Name = "Matemáticas II", Department = "Ciencias Básicas", Credits = 4, Semester = "2", InternalId = "MAT102", Code = "MAT2" },
                    new Subject { Id = 3, Name = "Programación Básica", Department = "Informática", Credits = 3, Semester = "1", InternalId = "INF101", Code = "INF1" },
                    new Subject { Id = 4, Name = "Estructuras de Datos", Department = "Informática", Credits = 3, Semester = "2", InternalId = "INF102", Code = "INF2" }
                };

                await subjectsCollection.InsertManyAsync(subjects);
            }

            // Inicializar SubjectRelationships
            var relationshipsCollection = _database.GetCollection<SubjectRelationship>("SubjectRelationships");
            if (await relationshipsCollection.CountDocumentsAsync(FilterDefinition<SubjectRelationship>.Empty) == 0)
            {
                var relationships = new List<SubjectRelationship>
                {
                    new SubjectRelationship { SubjectCode = "MAT2", RelatedSubjectCode = "MAT1", Type = "prerequisite" },
                    new SubjectRelationship { SubjectCode = "INF2", RelatedSubjectCode = "INF1", Type = "prerequisite" },
                    new SubjectRelationship { SubjectCode = "MAT1", RelatedSubjectCode = "MAT2", Type = "postrequisite" },
                    new SubjectRelationship { SubjectCode = "INF1", RelatedSubjectCode = "INF2", Type = "postrequisite" }
                };

                await relationshipsCollection.InsertManyAsync(relationships);
            }


            await Task.CompletedTask;
        }
    }
}
