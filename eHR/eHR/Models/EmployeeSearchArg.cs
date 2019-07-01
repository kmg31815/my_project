using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHR.Models
{
	public class EmployeeSearchArg
	{
		public string EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public string JobTitleId { get; set; }
		public string HireDateStart { get; set; }
		public string HireDateEnd { get; set; }
	}
}