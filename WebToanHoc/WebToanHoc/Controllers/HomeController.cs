using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebToanHoc.Models;

namespace WebToanHoc.Controllers
{
     public class HomeController : Controller
     {
          DbContext_WebTaiLieu db = new DbContext_WebTaiLieu();
          public ActionResult Index()
          {
               //lấy hết danh sách các tài liệu ở Bài đăng mới nhất
               var list_document = db.tbl_file.ToList();
               ViewBag.list_document = list_document;
               return View();
          }
          public ActionResult Detail_Document(int id=1)
          {
               //lấy thông tin tài liệu có id cho trước
               var document = db.tbl_file.Where(x => x.id_doc == id).FirstOrDefault();
               ViewBag.document = document;
               return View();
          }

     }
}