using API_MongoDB.Services;
using API_MongoDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_MongoDB.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase {

        private NutritecService service = new NutritecService();
        /// <summary>
        /// Creates feedback from nutritionist to patient or from patient to nutritionist.
        /// </summary>
        /// <param name="senderSsn">The ID of the user posting the message</param>
        /// <param name="receptorSsn">The ID of the destination user</param>
        /// <param name="message">The message that is going to be posted</param>
        /// <returns>A JSON-format message indicating that the publication has been successful</returns>  
        [HttpPost]
        [Route("createFeedback/{senderSsn}/{receptorSsn}/{message}")]
        public dynamic CreateFeedback(string senderSsn, string receptorSsn, string message) {
            Feedback f = new Feedback();
            f.SenderSsn = senderSsn;
            f.ReceptorSsn = receptorSsn;
            f.Date = DateTime.Now.ToString();
            f.Message = message;

            service.Create(f);
            return new { message = "Successfully created on " + f.Date + " by " + senderSsn};
        }

        /// <summary>
        /// Retrieves all posted feedback messages.
        /// </summary>
        /// <returns>A JSON-format object containing all feedback messages</returns> 
        [HttpGet]
        [Route("getAllFeedbacks")]
        public dynamic GetAllFeedbacks() {
            var result = service.Get();
            if (!result.Any()) {
                return new { message = "There is not any feedback on the Database"};
            }
            else {
                return new JsonResult(result);
            }
        }

        /// <summary>
        /// Retrieves every feedback message posted by the indicated user.
        /// </summary>
        /// <param name="ssn">The ID of the user who posted the messages</param>
        /// <returns>A JSON-format object containing the feedback messages posted by the user</returns> 
        [HttpGet]
        [Route("getBySsn/{ssn}")]
        public dynamic GetBySsn(string ssn) {
            var result = service.GetBySsn(ssn);
            if (!result.Any()) {
                return new { message = "User with SSN " + ssn + " has not published any feedback"};
            }
            else {
                return new JsonResult(result);
            }

        }

        /// <summary>
        /// Retrieves every feedback message posted on the indicated date.
        /// </summary>
        /// <param name="date">The date on which the feedback had been posted</param>
        /// <returns>A JSON-format object containing the feedback messages posted on that date</returns> 
        [HttpGet]
        [Route("getByDate/{date}")]
        public dynamic GetByDate(string date) {
            string day = date.Split("-")[0];
            string month = date.Split("-")[1];
            string year = date.Split("-")[2];
            string newDate = day + "/" + month + "/" + year;
            var result = service.GetByDate(newDate);
            if (!result.Any()) {
                return new { message = "There is no feedback on date: " + newDate};
            }
            else {
                return new JsonResult(result);
            }
        }

        /// <summary>
        /// Removes a feedback message indicating its ID
        /// </summary>
        /// <param name="id">The identifier of the feedback message</param>
        /// <returns>A JSON-format message indicating that the feedback has been successfully removed</returns> 
        [HttpDelete]
        [Route("removeById/{id}")]
        public dynamic RemoveById(string id) {
            if (service.GetById(id) != null) {
                service.Remove(id);
                return new { message = id + " successfully removed."};
            }
            else {
                return new { message = "Feedback with ID " + id + " does not exist on the Database"};
            }
        }
    }
}