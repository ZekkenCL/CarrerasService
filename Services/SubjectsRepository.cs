using CarrerasService.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using CarrerasService.Config;

namespace CarrerasService.Services
{
    public class SubjectsRepository
    {
        private readonly IMongoCollection<Subject> _subjects;
        private readonly IMongoCollection<SubjectRelationship> _subjectRelationships;

        public SubjectsRepository(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _subjects = database.GetCollection<Subject>(settings.SubjectsCollectionName);
            _subjectRelationships = database.GetCollection<SubjectRelationship>(settings.SubjectRelationshipsCollectionName);
        }

        public IEnumerable<Subject> GetAllSubjects() => _subjects.Find(subject => true).ToList();

        public IEnumerable<SubjectRelationship> GetPrerequisites() =>
            _subjectRelationships.Find(rel => rel.Type == "prerequisite").ToList();

        public IEnumerable<SubjectRelationship> GetPostrequisites() =>
            _subjectRelationships.Find(rel => rel.Type == "postrequisite").ToList();
    }
}
