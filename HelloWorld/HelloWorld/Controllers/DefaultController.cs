using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloWorld.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
			Models.HelloworldBusiness business = new Models.HelloworldBusiness();
			//ViewBag.Label = "Hello MVC~";
			ViewBag.Label = business.GetLabelText();
            return View();
        }
    }
}