using API_MongoDB.Models;
using MongoDB.Driver;

namespace API_MongoDB.Services {
    public class NutritecService {
        private readonly IMongoCollection<Feedback> feedbacks;
        private NutritecSettings settings;
        private IMongoClient client;
        public NutritecService() {
            settings = new NutritecSettings("mongodb://api-mongov2-server:m6xH4Tb8mpzv41NyqgurjPsWirmeiR0zw8XZOpSIhUPThNZwoV8MqWzfdea9jNP5j4nZY5RbZ9XAACDbuAACeA==@api-mongov2-server.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@api-mongov2-server@", "nutritec-feedback", "Feedback");
            client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DataBaseName);
            feedbacks = database.GetCollection<Feedback>(settings.CollectionName);
        }
        public Feedback Create(Feedback f) {
            feedbacks.InsertOne(f);
            return f;
        }
        public List<Feedback> Get() {
            return feedbacks.Find(f => true).ToList();
        }
        public Feedback GetById(string id) {
            return feedbacks.Find(f => f.Id == id).First();
        }
        public List<Feedback> GetBySsn(string ssn) {
            return feedbacks.Find(f => f.SenderSsn == ssn).ToList();
        }
        public List<Feedback> GetByDate(string date) {
            return feedbacks.Find(f => f.Date.Contains(date)).ToList();
        }
        public void Remove(string id) {
            feedbacks.DeleteOne(f => f.Id == id);
        }
    }
}