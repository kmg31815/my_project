using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BookMaintenance.Models
{
	public class BookService
	{
		/// <summary>
		/// 取得DB連線字串
		/// </summary>
		/// <returns></returns>
		private string GetDBConnectionString()
		{
			return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
		}

		/// <summary>
		/// 依照條件取得圖書資料
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public List<Models.Books> GetBookDataByCondtioin(Models.BooksSearchArg arg)
		{
			DataTable dt = new DataTable();
			string sql = @"SELECT bd.BOOK_ID AS BookID,
                                  bd.BOOK_NAME AS BookName,
	                              bcl.BOOK_CLASS_NAME AS BookClassName,
                                  bcl.BOOK_CLASS_ID AS BookClassID,
                                  CONVERT(VARCHAR,bd.BOOK_BOUGHT_DATE,111) AS BookBoughtDate,
		                          mm.USER_ENAME+'-'+mm.USER_CNAME AS UserName,
                                  mm.USER_ID AS UserID,
		                          bc.CODE_NAME AS CodeName,
                                  bc.CODE_ID AS CodeID
                           FROM BOOK_DATA AS bd 
	                            LEFT JOIN MEMBER_M  AS mm
		                            ON bd.BOOK_KEEPER = mm.USER_ID
	                            INNER JOIN BOOK_CLASS AS bcl
		                            ON bd.BOOK_CLASS_ID = bcl.BOOK_CLASS_ID
	                            INNER JOIN BOOK_CODE AS bc
		                            ON (bd.BOOK_STATUS=bc.CODE_ID) AND bc.CODE_TYPE='BOOK_STATUS' 
                           WHERE (UPPER(bd.BOOK_NAME)LIKE UPPER(@BookName) OR @BookName='') AND
	                             (bcl.BOOK_CLASS_ID= @BookClassName OR @BookClassName='') AND
	                             (mm.USER_ID=@UserName OR @UserName='') AND
	                             (bc.CODE_ID= @CodeName OR @CodeName='')
                            ORDER BY BookBoughtDate DESC";

							//SELECT bcs.BOOK_CLASS_NAME AS BookClass, bd.BOOK_NAME AS BookName , FORMAT(bd.book_bought_date, 'yyyy/MM/dd') AS BookBoughtDate , bc.CODE_NAME AS CodeName,  mm.USER_ENAME AS KeeperName
							//FROM BOOK_DATA bd join BOOK_CLASS bcs on bd.BOOK_CLASS_ID = bcs.BOOK_CLASS_ID join BOOK_CODE bc on bd.BOOK_STATUS = bc.CODE_ID LEFT join MEMBER_M mm on bd.BOOK_KEEPER = mm.USER_ID
							//WHERE  CODE_TYPE = 'BOOK_STATUS' and (bd.BOOK_NAME like '%'+@BookName+'%' OR @BookName='')  and (bcs.BOOK_CLASS_NAME = @BookClass or @BookClass = '') and (mm.USER_ENAME = @KeeperName or @KeeperName = '') and ( bc.CODE_NAME = @CodeName or @CodeName = '')
							//ORDER BY bd.book_bought_date DESC";

			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookName", arg.BookName == null ? string.Empty : "%" + arg.BookName + "%"));
				cmd.Parameters.Add(new SqlParameter("@BookClassName", arg.BookClassName == null ? string.Empty : arg.BookClassName));
				cmd.Parameters.Add(new SqlParameter("@UserName", arg.UserName == null ? string.Empty : arg.UserName));
				cmd.Parameters.Add(new SqlParameter("@CodeName", arg.CodeName == null ? string.Empty : arg.CodeName));
				SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
				sqlAdapter.Fill(dt);
				conn.Close();
			}
			return this.MapBookDataToListForSearch(dt);
		}

		/// <summary>
		/// 刪除圖書
		/// </summary>
		/// <param name="BookID"></param>
		public void DeleteBook(int BookID)
		{
			try
			{
				string sql = @"DELETE FROM BOOK_DATA WHERE BOOK_ID = @BookID";
				using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.Add(new SqlParameter("@BookID", BookID));
					SqlTransaction Tran = conn.BeginTransaction();
					cmd.Transaction = Tran;
					try
					{
						cmd.ExecuteNonQuery();
						Tran.Commit();
					}
					catch (Exception)
					{
						Tran.Rollback();
						throw;
					}
					finally
					{
						conn.Close();
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 新增圖書
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		public int InsertBook(Models.Books book)
		{
			string sql = @"INSERT INTO BOOK_DATA(BOOK_NAME,BOOK_CLASS_ID,BOOK_AUTHOR,BOOK_BOUGHT_DATE,BOOK_PUBLISHER,BOOK_NOTE,BOOK_STATUS)
						   VALUES(@BookName,@BookClassID,@BookAuthor,@BookBoughtDate,@BookPublisher,@BookNote,@CanBeLend)
                           SELECT SCOPE_IDENTITY()";

			int BookID;
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookName", book.BookName));
				cmd.Parameters.Add(new SqlParameter("@BookClassID", book.BookClassID));
				cmd.Parameters.Add(new SqlParameter("@BookAuthor", book.BookAuthor));
				cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", book.BookBoughtDate));
				cmd.Parameters.Add(new SqlParameter("@BookPublisher", book.BookPublisher));
				cmd.Parameters.Add(new SqlParameter("@BookNote", book.BookNote));
				cmd.Parameters.Add(new SqlParameter("@CanBeLend", "A"));
				SqlTransaction Tran = conn.BeginTransaction();
				cmd.Transaction = Tran;
				try
				{
					BookID = Convert.ToInt32(cmd.ExecuteScalar());
					Tran.Commit();
				}
				catch (Exception)
				{
					Tran.Rollback();
					throw;
				}
				finally
				{
					conn.Close();
				}

			}
			return BookID;
		}

		/// <summary>
		/// 抓取圖書明細
		/// </summary>
		public List<Models.Books> GetBookDetail(int BookID)
		{
			DataTable dtmodify = new DataTable();
			string sql = @"SELECT bd.BOOK_ID AS BookID,
                                  bd.BOOK_NAME AS BookName,
                                  CONVERT(VARCHAR,bd.BOOK_BOUGHT_DATE,111)  AS BookBoughtDate,
                                  bd.BOOK_CLASS_ID AS BookClassID,
                                  bcl.BOOK_CLASS_NAME AS BookClassName,
                                  bd.BOOK_STATUS AS CodeID,
                                  bc.CODE_NAME AS CodeName,
                                  bd.BOOK_KEEPER AS UserID,
                                  mm.USER_ENAME+'-'+mm.USER_CNAME AS UserName,
                                  bd.BOOK_AUTHOR AS BookAuthor,
                                  bd.BOOK_PUBLISHER AS BookPublisher,
                                  bd.BOOK_NOTE AS BookNote
                           FROM BOOK_DATA AS bd
                                 LEFT JOIN MEMBER_M  AS mm
                                    ON bd.BOOK_KEEPER = mm.USER_ID
                                 INNER JOIN BOOK_CODE AS bc
		                            ON (bd.BOOK_STATUS=bc.CODE_ID) AND bc.CODE_TYPE='BOOK_STATUS'
                                 INNER JOIN BOOK_CLASS AS bcl
		                            ON bd.BOOK_CLASS_ID = bcl.BOOK_CLASS_ID
                           WHERE bd.BOOK_ID = @BookID;";
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookID", BookID));
				SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
				sqlAdapter.Fill(dtmodify);
				conn.Close();
			}
			return MapBookDataToListForModify(dtmodify);
		}

		/// <summary>
		/// 修改圖書(抓取預設值)
		/// </summary>
		/// <param name="BookID"></param>
		/// <returns></returns>
		public List<Models.Books> GetBookData(int BookID)
		{
			DataTable dtmodify = new DataTable();
			string sql = @"SELECT bd.BOOK_ID AS BookID,
                                  bd.BOOK_NAME AS BookName,
                                  CONVERT(char(10),bd.BOOK_BOUGHT_DATE,126) AS BookBoughtDate,
                                  bd.BOOK_CLASS_ID AS BookClassName,
                                  bd.BOOK_STATUS AS CodeName,
                                  bd.BOOK_KEEPER AS UserName,
                                  bd.BOOK_AUTHOR AS BookAuthor,
                                  bd.BOOK_PUBLISHER AS BookPublisher,
                                  bd.BOOK_NOTE AS BookNote
                           FROM   BOOK_DATA AS bd
                           WHERE  bd.BOOK_ID = @BookID;";
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookID", BookID));
				SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
				sqlAdapter.Fill(dtmodify);
				conn.Close();
			}
			return MapBookDataToListForModify(dtmodify);
		}

		/// <summary>
		/// 修改圖書(儲存)
		/// </summary>
		/// <param name="bookData"></param>
		/// <returns></returns>
		public void UpdateBookData(Models.Books books)
		{
			string sql = @"UPDATE BOOK_DATA
                           SET BOOK_NAME=@BookName,
                               BOOK_BOUGHT_DATE=@BookBoughtDate,
                               BOOK_CLASS_ID=@BookClassID,
                               BOOK_STATUS=@CodeName,
                               BOOK_KEEPER=@UserName,
                               BOOK_AUTHOR=@BookAuthor,
                               BOOK_PUBLISHER=@BookPublisher,
                               BOOK_NOTE=@BookNote
                           WHERE BOOK_ID=@BookID   ";

			///借閱紀錄新增
			if (!string.IsNullOrEmpty(books.UserName))
			{
				sql += @"INSERT INTO BOOK_LEND_RECORD(BOOK_ID,KEEPER_ID,LEND_DATE)
                         VALUES(@BookID,@UserName,@LendDate)";
			}
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookID", books.BookID));
				cmd.Parameters.Add(new SqlParameter("@BookName", books.BookName));
				cmd.Parameters.Add(new SqlParameter("@BookClassID", books.BookClassID));
				cmd.Parameters.Add(new SqlParameter("@BookAuthor", books.BookAuthor));
				cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", books.BookBoughtDate));
				cmd.Parameters.Add(new SqlParameter("@BookPublisher", books.BookPublisher));
				cmd.Parameters.Add(new SqlParameter("@BookNote", books.BookNote));
				cmd.Parameters.Add(new SqlParameter("@CodeName", books.CodeName));
				cmd.Parameters.Add(new SqlParameter("@UserName", books.UserName == null ? string.Empty : books.UserName));
				cmd.Parameters.Add(new SqlParameter("@LendDate", DateTime.Now));
				SqlTransaction Tran = conn.BeginTransaction();
				cmd.Transaction = Tran;
				try
				{
					cmd.ExecuteNonQuery();
					Tran.Commit();
				}
				catch (Exception)
				{
					Tran.Rollback();
					throw;
				}
				finally
				{
					conn.Close();
				}
			}
		}

		/// <summary>
		/// Map查詢資料進List
		/// </summary>
		/// <param name="bookData"></param>
		/// <returns></returns>
		private List<Models.Books> MapBookDataToListForSearch(DataTable bookData)
		{
			List<Models.Books> result = new List<Books>();
			foreach (DataRow row in bookData.Rows)
			{
				result.Add(new Books()
				{
					BookID = (int)row["BookID"],
					BookName = row["BookName"].ToString(),
					BookClassName = row["BookClassName"].ToString(),
					BookBoughtDate = row["BookBoughtDate"].ToString(),
					CodeName = row["CodeName"].ToString(),
					UserName = row["UserName"].ToString()

				});
			}
			return result;
		}

		/// <summary>
		/// Map明細資料進List
		/// </summary>
		/// <param name="dtmodify"></param>
		/// <returns></returns>
		private List<Books> MapBookDataToListForModify(DataTable BookDataModify)
		{
			List<Models.Books> resultModify = new List<Books>();
			foreach(DataRow row in BookDataModify.Rows)
			{
				resultModify.Add(new Books()
				{
					BookID = (int)row["BookID"],
					BookName = row["BookName"].ToString(),
					BookAuthor = row["BookAuthor"].ToString(),
					BookPublisher = row["BookPublisher"].ToString(),
					BookNote = row["BookNote"].ToString(),
					BookBoughtDate = row["BookBoughtDate"].ToString(),
					BookClassName = row["BookClassName"].ToString(),
					UserName = row["UserName"].ToString(),
					CodeName = row["CodeName"].ToString(),
				});
			}
			return resultModify;
		}

		/// <summary>
		/// 換行符號取代
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string Replace(string input)
		{
			return Regex.Replace(input, "<BR>", "\r\n");
		}

		/// <summary>
		/// 借閱紀錄
		/// </summary>
		public List<Models.Books> GetBookLendRecord(int BookID)
		{
			DataTable dtmodify = new DataTable();
			string sql = @"SELECT bd.BOOK_NAME AS BookName,
                                  blr.BOOK_ID AS BookID,
	                              (mm.USER_ENAME +'-'+ mm.USER_CNAME) AS UserName,
                                  blr.KEEPER_ID AS UserID,
	                              CONVERT(char(10),blr.LEND_DATE,111) AS LendDate
                           FROM BOOK_LEND_RECORD blr
                           INNER JOIN BOOK_DATA bd
	                           ON blr.BOOK_ID = bd.BOOK_ID
                           INNER JOIN MEMBER_M mm
	                           ON blr.KEEPER_ID=mm.USER_ID
                           WHERE blr.BOOK_ID=@BookID
                           ORDER BY blr.LEND_DATE DESC";
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@BookID", BookID));
				SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
				sqlAdapter.Fill(dtmodify);
				conn.Close();
			}
			return MapBookDataToListForLendRecord(dtmodify);
		}

		/// <summary>
		/// Map借閱紀錄資料進List
		/// </summary>
		/// <param name="BookData"></param>
		/// <returns></returns>
		private List<Models.Books> MapBookDataToListForLendRecord(DataTable BookData)
		{
			List<Models.Books> result = new List<Books>();
			foreach (DataRow row in BookData.Rows)
			{
				result.Add(new Books()
				{
					BookID = (int)row["BookID"],
					BookName = row["BookName"].ToString(),
					UserName = row["UserName"].ToString(),
					UserID = row["UserID"].ToString(),
					LendDate = row["LendDate"].ToString()
				});
			}
			return result;
		}
	}
}