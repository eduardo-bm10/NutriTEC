using API_MongoDB.Services;
using API_MongoDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_MongoDB.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase {
        private static int _id = 0;
        public NutritecService service = new NutritecService();

        [HttpPost]
        [Route("createFeedback")]
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
        [Route("getBySsn")]
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
        [Route("getByDate")]
        public dynamic GetByDate(string date) {
            var result = service.GetByDate(date);
            if (!result.Any()) {
                return new { message = "There is no feedback on date: " + date};
            }
            else {
                return new JsonResult(result);
            }
        }

        [HttpDelete]
        [Route("removeById")]
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