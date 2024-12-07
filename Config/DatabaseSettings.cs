namespace CarrerasService.Config
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string SubjectsCollectionName { get; set; }
        public string SubjectRelationshipsCollectionName { get; set; }
    }
}
