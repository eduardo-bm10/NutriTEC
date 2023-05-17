namespace API_MongoDB.Models {
    public class DataBaseSettings {
        public string ConnectionString { get; set; } = null!;
        public string DataBaseName { get; set; } = null!;
        public string CollectionName {get; set; } = null!;
    }
}