//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Mvc;


//namespace AST_Intranet.Controllers
//{
//    public class CalendarController : Controller
//    {
//        // GET: Calendar

//            // Fetch events (including holidays from the API)
//            public async Task<JsonResult> GetEvents()
//            {
//                var events = new List<object>();

//                // Fetch events from external API (Holidays in this case)
//                using (var client = new HttpClient())
//                {
//                    var response = await client.GetStringAsync("https://api.holidayapi.com/v1/holidays?country=US&year=2025&apiKey=YOUR_API_KEY");

//                    // Assuming you get a list of holidays in response
//                    var holidays = JsonConvert.DeserializeObject<List<Holiday>>(response);
//                    foreach (var holiday in holidays)
//                    {
//                        events.Add(new
//                        {
//                            title = holiday.title,
//                            start = holiday.start,
//                            end = holiday.end,
//                            description = holiday.description
//                        });
//                    }
//                }

//                // Add user events here (you can fetch from your database or another source)

//                return Json(events, JsonRequestBehavior.AllowGet);
//            }

//            // Method to handle saving user-created events
//            [HttpPost]
//            public JsonResult CreateEvent(string title, string start, string end)
//            {
//                // Logic for saving the event into the database
//                // Assuming the event is saved and an ID is generated.
//                return Json(new { success = true });
//            }

//            // Additional methods to handle event updates and deletions
//        }

//        public class Holiday
//        {
//            public string title { get; set; }
//            public string start { get; set; }
//            public string end { get; set; }
//            public string description { get; set; }
//        }
//    }

