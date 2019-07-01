using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace eHR.Models
{
	public class Employee
	{
		/// <summary>
		/// 員工編號
		/// </summary>
		[DisplayName("員工編號")]
		[Required()]
		public int EmployeeId { get; set; }
		
		/// <summary>
		/// 員工姓名(First)
		/// </summary>
		[DisplayName("員工姓名")]
		[Required()]
		public string EmployeeFirstName { get; set; }
		
		/// <summary>
		/// 員工姓名(Last)
		/// </summary>
		public string EmployeeLastName { get; set; }

		/// <summary>
		/// 職稱
		/// </summary>
		public string JobTitle { get; set; }

		public string JobTitleId { get; set; }

		/// <summary>
		/// 稱謂
		/// </summary>
		public string TitleOfCourtesy { get; set; }

		/// <summary>
		/// 任職日期
		/// </summary>
		public string HireDate { get; set; }

		/// <summary>
		/// 生日日期
		/// </summary>
		public string BirthDate { get; set; }

		/// <summary>
		/// 年齡
		/// </summary>
		public int Age { get; set; }

		/// <summary>
		/// 國家
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// 城市代號
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// 性別
		/// </summary>
		public string Gender { get; set; }

		public string GenderId { get; set; }

		/// <summary>
		/// 電話號碼
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		/// 地址
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// 直屬主管
		/// </summary>
		public string ManagerId { get; set; }

		/// <summary>
		/// 月薪
		/// </summary>
		public string MonthlyPayment { get; set; }

		/// <summary>
		/// 年薪
		/// </summary>
		public string YearlyPayment { get; set; }
	}
}