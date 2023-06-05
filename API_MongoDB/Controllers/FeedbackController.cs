using API_MongoDB.Services;
using API_MongoDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_MongoDB.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase {
        public NutritecService service = new NutritecService();

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