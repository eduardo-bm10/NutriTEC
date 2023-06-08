using API_MongoDB.Models;
using MongoDB.Driver;

namespace API_MongoDB.Services {

    /// <summary>
    /// NutritecService Class: Provides the methods for manipulating the Mongo Database.
    /// </summary>
    public class NutritecService {
        private readonly IMongoCollection<Feedback> feedbacks;
        private NutritecSettings settings;
        private IMongoClient client;

        /// <summary>
        /// Constructor: Establishes initital connection with the Database.
        /// </summary>
        public NutritecService() {
            settings = new NutritecSettings("mongodb+srv://mongoadmin:123456789Ed@nutritecdb.mongocluster.cosmos.azure.com/?tls=true&authMechanism=SCRAM-SHA-256&retrywrites=false&maxIdleTimeMS=120000", "nutritec", "Feedback");
            client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DataBaseName);
            feedbacks = database.GetCollection<Feedback>(settings.CollectionName);
        }

        /// <summary>
        /// Inserts a new feedback on the database.
        /// </summary>
        /// <param name="f">The feedback object to be inserted</param>
        /// <returns>The recently-inserted feedback object</returns>  
        public Feedback Create(Feedback f) {
            feedbacks.InsertOne(f);
            return f;
        }

        /// <summary>
        /// Retrieves all feedbacks from database.
        /// </summary>
        /// <returns>A list of feedback objects</returns>
        public List<Feedback> Get() {
            return feedbacks.Find(f => true).ToList();
        }

        ///<summary>
        /// Retrieves a specified feedback from database.
        /// </summary>
        /// <returns>Feedback object</returns>
        public Feedback GetById(string id) {
            return feedbacks.Find(f => f.Id == id).First();
        }

        /// <summary>
        /// Retrieves all feedbacks by specified ssn
        /// </summary>
        /// <param name="ssn">The ssn of the user who posted feedbacks</param>
        /// <returns>A list of feedback objects</returns>
        public List<Feedback> GetBySsn(string ssn) {
            return feedbacks.Find(f => f.SenderSsn == ssn).ToList();
        }

        /// <summary>
        /// Retrieves all feedbacks by specified date.
        /// </summary>
        /// <param name="date">The date on which feedbacks were posted</param>
        /// <returns>A list of feedback objects</returns>
        public List<Feedback> GetByDate(string date) {
            return feedbacks.Find(f => f.Date.Contains(date)).ToList();
        }

        /// <summary>
        /// Removes a feedback by specified identifier.
        /// </summary>
        /// <param name="">The identifier of the feedback object</param>
        public void Remove(string id) {
            feedbacks.DeleteOne(f => f.Id == id);
        }
    }
}