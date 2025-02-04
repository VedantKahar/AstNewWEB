using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AST_Intranet.Controllers
{
    public class EventsController : Controller
    {
        // GET: events
        public ActionResult EventsView()
        {
            return View();
        }
    }
}