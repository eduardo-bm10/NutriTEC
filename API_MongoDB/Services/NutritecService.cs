using API_MongoDB.Models;
using MongoDB.Driver;

namespace API_MongoDB.Services {
    public class NutritecService {
        private readonly IMongoCollection<Feedback> feedbacks;
        private NutritecSettings settings;
        private IMongoClient client;
        public NutritecService() {
            settings = new NutritecSettings("mongodb://localhost:27017", "nutritec", "Feedback");
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
            return feedbacks.Find(f => f.Date == date).ToList();
        }
        public void Remove(string id) {
            feedbacks.DeleteOne(f => f.Id == id);
        }
    }
}