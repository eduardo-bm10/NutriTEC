namespace API_MongoDB.Models {
    public class NutritecSettings {
        public string ConnectionString;
        public string DataBaseName;
        public string CollectionName;
        public NutritecSettings(string connection, string database, string collection) {
            this.ConnectionString = connection;
            this.DataBaseName = database;
            this.CollectionName = collection;
        }
    }
}