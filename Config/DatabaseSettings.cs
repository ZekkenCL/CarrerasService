namespace CarrerasService.Config
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string SubjectsCollectionName { get; set; } = string.Empty;
        public string SubjectRelationshipsCollectionName { get; set; } = string.Empty;
    }
}
