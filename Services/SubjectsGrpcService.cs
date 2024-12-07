using Grpc.Core;
using System.Linq;
using System.Threading.Tasks;
using CarrerasService.Protos;

namespace CarrerasService.Services
{
    public class SubjectsGrpcService : SubjectsService.SubjectsServiceBase
    {
        private readonly SubjectsRepository _repository;

        public SubjectsGrpcService(SubjectsRepository repository)
        {
            _repository = repository;
        }

        public override Task<SubjectsList> GetAllSubjects(CarrerasService.Protos.Empty request, ServerCallContext context)
        {
            var subjects = _repository.GetAllSubjects().Select(s => new CarrerasService.Protos.Subject
            {
                Id = s.Id.ToString(),
                Name = s.Name,
                Department = s.Department,
                Credits = s.Credits,
                Semester = s.Semester,
                InternalId = s.InternalId,
                Code = s.Code
            }).ToList();

            var response = new SubjectsList();
            response.Subjects.AddRange(subjects);
            return Task.FromResult(response);
        }

        public override Task<PrerequisiteMap> GetAllPrerequisites(CarrerasService.Protos.Empty request, ServerCallContext context)
        {
            var map = _repository.GetPrerequisites()
                .ToDictionary(r => r.SubjectCode, r => new Prerequisite
                {
                    Code = r.SubjectCode,
                    PrerequisiteCode = r.RelatedSubjectCode
                });

            var response = new PrerequisiteMap();
            response.Map.Add(map);
            return Task.FromResult(response);
        }

        public override Task<PostrequisiteMap> GetAllPostrequisites(CarrerasService.Protos.Empty request, ServerCallContext context)
        {
            var map = _repository.GetPostrequisites()
                .ToDictionary(r => r.SubjectCode, r => new Postrequisite
                {
                    Code = r.SubjectCode,
                    PostrequisiteCode = r.RelatedSubjectCode
                });

            var response = new PostrequisiteMap();
            response.Map.Add(map);
            return Task.FromResult(response);
        }
    }
}
