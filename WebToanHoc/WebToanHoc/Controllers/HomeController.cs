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
               var list_document = db.tbl_file.Where(x => x.status == 1).OrderByDescending(x=>x.time_up).ToList().Take(12);
               ViewBag.list_document = list_document;
               //lấy danh sách các tài liệu cấp 2
               ViewBag.list_document_grade2= db.tbl_file.Where(x => x.status == 1 && x.id_cate>=1 && x.id_cate<=4).OrderByDescending(x => x.time_up).ToList().Take(12);
               ViewBag.list_document_grade3 = db.tbl_file.Where(x => x.status == 1 && x.id_cate >= 5 && x.id_cate <= 7).OrderByDescending(x => x.time_up).ToList().Take(12);
               ViewBag.list_document_grade4 = db.tbl_file.Where(x => x.status == 1 && x.id_cate ==8).OrderByDescending(x => x.time_up).ToList().Take(12);//sửa 
               ViewBag.list_download=db.tbl_file.Where(x=>x.status==1).OrderByDescending(x => x.num_view).ToList().Take(6);
               return View();
          }
          public ActionResult Category_Document(int id)
          {                         
               //lấy danh sách theo lớp
               var listDoc_cate = db.tbl_file.Where(x => x.status == 1 && x.id_cate == id).ToList();
               ViewBag.listDoc_cate = listDoc_cate;
               //lấy tên lớp
               var category = db.tbl_category.Where(x => x.id_cate == id).Select(x=>x.cate_name).FirstOrDefault();
               ViewBag.category = category;
               //lấy các lớp liên quan
               if (id>=1 &&id<=4)
               {
                    ViewBag.grade = 2;
                    ViewBag.list_class=db.tbl_category.Where(x=>x.id_cate>=1 && x.id_cate<=4).ToList();
               }
               else if (id>=5 && id<=7)
               {
                    ViewBag.grade = 3;
                    ViewBag.list_class = db.tbl_category.Where(x => x.id_cate >= 5 && x.id_cate <= 7).ToList();
               }
               else if (id==8)
               {
                    ViewBag.grade = 4;
                    ViewBag.list_class = db.tbl_category.Where(x => x.id_cate==8).ToList();
               }
               return View();
          }
        //public ActionResult Detail_Document(int id = 1)
          public ActionResult Detail_Document(int id)
          {
               //lấy thông tin tài liệu có id cho trước
               var document = db.tbl_file.Where(x => x.id_doc == id).FirstOrDefault();
               ViewBag.document = document;
               //lấy thông tin của các mục cùng id
               var id_Cate = db.tbl_file.Where(x => x.id_doc == id).Select(x => x.id_cate).FirstOrDefault();
               if (id_Cate >= 1 && id_Cate <= 4)
               {
                    ViewBag.grade = "Toán cấp 2";                    
               }
               else if (id_Cate >= 5 && id_Cate <= 7)
               {
                    ViewBag.grade = "Toán cấp 3";                   
               }
               else if (id_Cate==8)
               {
                    ViewBag.grade = "Toán Thi THPT";
               }
               //lấy thông tin lớp nào
               ViewBag.classGrade = db.tbl_category.Where(x => x.id_cate == id_Cate).Select(x => x.cate_name).FirstOrDefault();
               ViewBag._idclassGrade = id_Cate;
               //lấy thông tin tài liệu liên quan
               var list_relate= db.tbl_file.Where(x => x.id_cate == id_Cate).OrderByDescending(x => x.time_up).ToList().Take(8);
               List<tbl_file> lst1 = new List<tbl_file>();
               List<tbl_file> lst2 = new List<tbl_file>();
               var count = 0;
               foreach (var item in list_relate)
               {
                    if (count<4)
                    {
                         lst1.Add(item);
                    }
                    else
                    {
                         lst2.Add(item);
                    }
                    count++;
               }
               ViewBag.lst1 = lst1;
               ViewBag.lst2 = lst2;
               return View();
          }

     }
}