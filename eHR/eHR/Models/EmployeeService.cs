using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eHR.Models
{
	public class EmployeeService
	{
		public int InsertEmployee(Models.Employee employee)
		{
			return 0;
		}

		public List<Models.Employee> GetEmployeeByCondtioin(Models.EmployeeSearchArg arg)
		{
			List<Models.Employee> result = new List<Employee>();
			result.Add(new Employee() { EmployeeId = 1, EmployeeLastName = "123", Address = "新北市新莊區" });
			result.Add(new Employee() { EmployeeId = 2, EmployeeLastName = "456", Address = "新北市泰山區" });

			return result;
		}
	}
}