using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab5.Controllers
{
    public class HomeController : Controller
    {
        //khai báo chuỗi kết nối
        string strcon =
       "Data Source=DESKTOP-EQGN1TN\\SQLEXPRESS;Initial Catalog=Lab5;Integrated Security=True";
        SqlConnection conn;
        public HomeController()
        {
            //tạo đối tượng kết nối
            conn = new SqlConnection(strcon);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Connection()
        {
            //mở kết nối
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Open)
                ViewBag.msg = "Kết nối đã thiết lập: " + strcon;
            else
                ViewBag.msg = "Kết nối chưa thiết lập";
            //đóng kết nối
            conn.Close();
            return View();
        }
        public ActionResult InsertStudent()
        {
            //chuyển danh sách lớp ra view hiển thị lên combo
            @ViewBag.listclass = GetClass();
            return View();
        }
        private List<SelectListItem> GetClass()
        {
            //tạo danh sách chứa các lớp
            List<SelectListItem> items = new List<SelectListItem>();
            //tạo đối tượng Command
            SqlCommand cmd = conn.CreateCommand();
            //gán câu lệnh sql
            cmd.CommandText = "select * from Class";
            //mở kết nối
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            //thực thi và trả về kết quả ra datareader
            SqlDataReader dr = cmd.ExecuteReader();
            //đọc reader và đưa vào danh sách lớp
            while (dr.Read())
            {
                items.Add(new SelectListItem()
                {
                    Text = dr.GetString(1),
                    Value = dr.GetInt32(0).ToString()
                });
            }
            return items;
        }
        [HttpPost]
        public ActionResult InsertStudent(HttpPostedFileBase file, string
        StudentId, string FirstName, string LastName, int ClassId, string Address,
        DateTime BirthDay, bool Gender, string Phone)
        {
            var filename = file == null ? "" : file.FileName;
            if (filename != "")
            {
                if (filename.ToLower().EndsWith("jpg") ||
               filename.ToLower().EndsWith("png"))
                {
                    file.SaveAs(Server.MapPath("~/Content/Images/" +
                   filename));
                }
                else //nếu không đúng loại ảnh thì quay lại view báo lỗi
                {
                    ViewBag.error = "Ảnh phải đúng loại: jpg, png";
                    @ViewBag.listclass = GetClass();
                    return View();
                }
            }
            //tạo đối tượng Command
            SqlCommand cmd = conn.CreateCommand();
            //chỉ ra thủ tục cần gọi
            cmd.CommandText = "InsertStudent";
            //chỉ ra loại thủ tục
            cmd.CommandType = CommandType.StoredProcedure;
            //gán giá trị cho các tham số
            cmd.Parameters.AddWithValue("@StudentId", StudentId);
            cmd.Parameters.AddWithValue("@ClassId", ClassId);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@BirthDay", BirthDay);
            cmd.Parameters.AddWithValue("@Gender", Gender);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Picture", filename);
            cmd.Parameters.Add("@Err", SqlDbType.NVarChar, 100);
            //đăng ký tham số đầu ra
            cmd.Parameters["@Err"].Direction = ParameterDirection.Output;
            //mở kết nối
            conn.Open();
            //thực thi câu lệnh
            cmd.ExecuteNonQuery();
            //lấy thông báo lỗi
            string err = cmd.Parameters["@Err"].Value.ToString();
            //nếu có lỗi thì quay lại view báo lỗi
            if (err != "")
            {
                if (filename != "" &&
               System.IO.File.Exists(Server.MapPath("~/Content/Images/" + filename)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Content/Images/"
                   + filename));
                }
                @ViewBag.listclass = GetClass();
                ViewBag.error = err;
                return View();
            }
            //tới action hiển thị sinh viên
            return RedirectToAction("ShowStudent");
        }
        public ActionResult ShowStudent()
        {
            //tạo data Adapter lấy dữ liệu trong Bảng Student
            SqlDataAdapter da = new SqlDataAdapter("select * from Student",
           strcon);
            //tạo dataset
            DataSet dsStudent = new DataSet();
            //lấy dữ liệu từ adapter ra dataset
            da.Fill(dsStudent, "Student");
            //chuyển dataset ra view
            return View(dsStudent.Tables["Student"].AsEnumerable());
        }
        public ActionResult Delete(string id)
        {
            //tạo đối tượng command
            SqlCommand cmd = conn.CreateCommand();
            //gán câu lệnh xóa
            cmd.CommandText = "delete from student where studentId=@id";
            //gán tham số
            cmd.Parameters.AddWithValue("@id", id);
            //mở connect
            conn.Open();
            //thực thi truy vấn
            cmd.ExecuteNonQuery();
            //chuyển tới hành động hiển thị sinh viên
            return RedirectToAction("ShowStudent");
        }
    }
}
