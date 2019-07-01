using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eSale.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

		[HttpPost()]
		public ActionResult Index(FormCollection form)
		{
			return View("Index");
		}
    }
}