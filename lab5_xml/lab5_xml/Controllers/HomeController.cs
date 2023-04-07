using lab5_xml.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace lab5_xml.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //Nếu không tồn tại tệp cd_catalog_xml thì tạo tệp tin
            if
           (!System.IO.File.Exists(Server.MapPath("~/Models/cd_catalog.xml")))
            {
                StreamWriter sw = new StreamWriter(Server.MapPath("~/Models/cd_catalog.xml"));
                sw.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sw.Write("<CATALOG/>");
                sw.Close();
            }
            return View();
        }
        public ActionResult ShowCD()
        {
            //tạo danh sách chứa cd
            List<Cd> listCD = new List<Cd>();
            //khởi tạo xmldocument
            XmlDocument doc = new XmlDocument();
            //load tệp tin xml
            doc.Load(Server.MapPath("~/Models/cd_catalog.xml"));
            //vào p hần tử gốc
            XmlNode root = doc.DocumentElement;
            //lấy danh sách các phần tử CD
            XmlNodeList nodesCD = root.ChildNodes;
            //duyệt các phần tử CD
            foreach (XmlNode cd in nodesCD)
            {
                //khởi tạo đối tượng CD
                Cd c = new Cd();
                //đọc thông tin phần tử CD ra đối tượng CD
                c.Title = cd.ChildNodes[0].InnerText;
                c.Artist = cd.ChildNodes[1].InnerText;

                c.Country = cd.ChildNodes[2].InnerText;
                c.Company = cd.ChildNodes[3].InnerText;
                c.Price = double.Parse(cd.ChildNodes[4].InnerText);
                c.Year = int.Parse(cd.ChildNodes[5].InnerText);
                //đưa cd vào list
                listCD.Add(c);
            }
            //chuyển list ra view để hiển thị
            return View(listCD.AsEnumerable());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Cd cd)
        {
            //nếu có lỗi thì quay ra view báo lỗi
            if (!ModelState.IsValid)
                return View();
            else
            {
                //tạo và load tài liệu xml
                XmlDocument doc = new XmlDocument();
                doc.Load(Server.MapPath("~/Models/cd_catalog.xml"));
                //truy cấp phần tử gốc
                XmlNode root = doc.DocumentElement;
                //tạo phần tử cd mới
                XmlNode cdobject = doc.CreateElement("CD");
                //tạo phần tử title
                XmlNode title = doc.CreateElement("TITLE");
                title.InnerText = cd.Title;
                //thêm title vào cd
                cdobject.AppendChild(title);
                //tạo phần tử artist
                XmlNode artist = doc.CreateElement("ARTIST");
                artist.InnerText = cd.Artist;
                //thêm title vào cd
                cdobject.AppendChild(artist);
                //tạo phần tử country
                XmlNode country = doc.CreateElement("COUNTRY");
                country.InnerText = cd.Country;
                //thêm country vào cd
                cdobject.AppendChild(country);
                //tạo phần tử company
                XmlNode company = doc.CreateElement("COMPANY");
                company.InnerText = cd.Company;
                //thêm company vào cd
                cdobject.AppendChild(company);
                //tạo phần tử price
                XmlNode price = doc.CreateElement("PRICE");
                price.InnerText = cd.Price.ToString();
                //thêm price vào cd
                cdobject.AppendChild(price);
                //tạo phần tử year
                XmlNode year = doc.CreateElement("YEAR");
                year.InnerText = cd.Year.ToString();
                //thêm year vào cd
                cdobject.AppendChild(year);
                //thêm vào phần tử gốc
                root.AppendChild(cdobject);
                //ghi dữ liệu
                doc.Save(Server.MapPath("~/Models/cd_catalog.xml"));
            }
            return RedirectToAction("ShowCD");
        }
        public ActionResult Delete(string id)
        {
            //tạo vào load tài liệu xml
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/Models/cd_catalog.xml"));
            //truy cập phần tử gốc
            XmlNode root = doc.DocumentElement;
            //lấy danh sách các nút cd
            XmlNodeList nodesCD = root.ChildNodes;
            //duyệt vào kiểm ta tiêu đề
            foreach (XmlNode cd in nodesCD)
            {
                if (cd.ChildNodes[0].InnerText.Trim().Equals(id))
                {
                    //xóa khi tìm thấy
                    root.RemoveChild(cd);
                    break;
                }
            }
            //ghi lại
            doc.Save(Server.MapPath("~/Models/cd_catalog.xml"));
            //chuyển đến action Show CD
            return RedirectToAction("ShowCD");
        }
    }

}


