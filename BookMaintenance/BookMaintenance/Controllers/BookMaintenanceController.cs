using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BookMaintenance.Controllers
{
	public class BookMaintenanceController : Controller
	{
		Models.CodeService codeService = new Models.CodeService();
		Models.BookService bookService = new Models.BookService();

		/// <summary>
		/// 書籍資料查詢
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			//Models.CodeService codeService = new Models.CodeService();
			//List<BookMaintenance.Models.BooksSearchArg> bookData = codeService.GetBookDataByCondtioin(new Models.BooksSearchArg());
			//ViewBag.SearchResult = bookData;

			ViewBag.BookClassNameData = this.codeService.GetBookClassName();
			ViewBag.UserNameData = this.codeService.GetUserName();
			ViewBag.CodeNameData = this.codeService.GetCodeName();
			return View();
		}

		/// <summary>
		/// 書籍資料查詢(查詢)
		/// </summary>
		/// <returns></returns>
		[HttpPost()]
		public ActionResult Index(Models.BooksSearchArg arg)
		{
			//Models.CodeService codeService = new Models.CodeService();

			ViewBag.BookClassNameData = this.codeService.GetBookClassName();
			ViewBag.UserNameData = this.codeService.GetUserName();
			ViewBag.CodeNameData = this.codeService.GetCodeName();
			ViewBag.SearchResult = bookService.GetBookDataByCondtioin(arg);
			return View("Index");
		}

		/// <summary>
		/// 刪除圖書
		/// </summary>
		/// <param name="BookID"></param>
		/// <returns></returns>
		[HttpPost()]
		public JsonResult DeleteBook(int BookID)
		{
			try
			{
				bookService.DeleteBook(BookID);
				return this.Json(true);
			}
			catch (Exception ex)
			{
				return this.Json(false);
			}
		}

		/// <summary>
		/// 新增書籍畫面
		/// </summary>
		/// <returns></returns>
		[HttpGet()]
		public ActionResult InsertBook()
		{
			ViewBag.BookClassNameData = this.codeService.GetBookClassName();
			return View(new Models.Books());
		}

		/// <summary>
		/// 新增圖書
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		[HttpPost()]
		public ActionResult InsertBook(Models.Books book)
		{
			ViewBag.BookClassNameData = this.codeService.GetBookClassName();
			if (ModelState.IsValid)
			{
				try
				{
					DateTime dateTime = DateTime.Parse(book.BookBoughtDate);
					int BookID = bookService.InsertBook(book);
					return RedirectToAction("BookData", new { BookID = BookID });
				}
				catch
				{
					Response.Write("<script language=javascript>alert('日期格式錯誤')</script>");
				}
			}
			return View(book);
		}

		/// <summary>
		/// 明細圖書畫面
		/// </summary>
		/// <param name="BookID"></param>
		/// <returns></returns>
		public ActionResult BookData(int BookID)
		{
			Models.Books books = bookService.GetBookDetail(BookID).FirstOrDefault();
			return View(books);
		}

		/// <summary>
		/// 修改圖書畫面
		/// </summary>
		/// <param name="BookID"></param>
		/// <returns></returns>
		[HttpGet()]
		public ActionResult UpdateBook(int BookID)
		{
			ViewBag.BookClassNameData = this.codeService.GetBookClassName();
			ViewBag.UserNameData = this.codeService.GetUserName();
			ViewBag.CodeNameData = this.codeService.GetCodeName();
			Models.Books books = bookService.GetBookData(BookID).FirstOrDefault();
			return View(books);
		}

		/// <summary>
		/// 修改圖書存檔
		/// </summary>
		[HttpPost()]
		public ActionResult UpdateBook(Models.Books books)
		{
			ViewBag.BookClassNameData = this.codeService.GetBookClassName();
			ViewBag.UserNameData = this.codeService.GetUserName();
			ViewBag.CodeNameData = this.codeService.GetCodeName();
			if (ModelState.IsValid)
			{
				try
				{
					DateTime dateTime = DateTime.Parse(books.BookBoughtDate);
					bookService.UpdateBookData(books);
					return RedirectToAction("BookData", new { BookID = books.BookID });
				}
				catch (Exception ex)
				{
					Response.Write("<script language=javascript>alert('日期格式錯誤')</script>");
				}
			}
			return View(books);
		}

		/// <summary>
		/// 借閱紀錄畫面
		/// </summary>
		[HttpGet()]
		public ActionResult BookLendRecord(int BookID)
		{

			ViewBag.LendRecord = bookService.GetBookLendRecord(BookID);
			return View("BookLendRecord");
		}
	}
}