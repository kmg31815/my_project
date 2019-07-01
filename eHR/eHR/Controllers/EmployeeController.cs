using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace eHR.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
			Models.EmployeeService employeeService = new Models.EmployeeService();
			var employees = employeeService.GetEmployeeByCondtioin(new Models.EmployeeSearchArg() {
				EmployeeId = "1",
				EmployeeName = "aaa" ,
				HireDateEnd = "",
				HireDateStart = "" ,
				JobTitleId = ""
			});
			
			ViewBag.EmpAdd = employees[0].Address;
            return View();
        }

		[HttpGet()]
		public ActionResult InsertEmp()
		{
			Models.Employee result = new Models.Employee();
			return View(result);
		}

		[HttpPost()]
		public ActionResult InsertEmp(Models.Employee employee)
		{
			return View();
		}

	}
}