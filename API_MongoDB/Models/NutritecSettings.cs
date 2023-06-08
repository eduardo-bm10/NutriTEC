namespace API_MongoDB.Models {
    
    /// <summary>
    /// NutritecSettings Class: Establishes the connection to the Mongo Server.
    /// </summary> 
    public class NutritecSettings {
        
        public string ConnectionString; //The connection string for the Mongo Server.
        public string DataBaseName; // The database name
        public string CollectionName; // The collection name

        /// <summary>
        /// Constructor: initializes the connection for the Mongo Server.
        /// </summary>
        /// <param name="connection">The connection string</param>
        /// <param name="database">The database name</param>
        /// <param name="collection">The collection name</param> 
        public NutritecSettings(string connection, string database, string collection) {
            this.ConnectionString = connection;
            this.DataBaseName = database;
            this.CollectionName = collection;
        }
    }
}