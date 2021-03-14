using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebToanHoc.Areas.Admin.Models;
using WebToanHoc.Models;
using WebToanHoc.EF.Model;
using System.Security.Cryptography;


namespace WebToanHoc.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DbContext_WebTaiLieu db = new DbContext_WebTaiLieu();
        // GET: Admin/Home
        public ActionResult Index(int? page)
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            //var list_cate = db.tbl_category.Where(x => x.id_subject == 1).ToList();
            //int number = list_cate.Count();
            //int min = 0;
            //int max = 0;
            //for(int i=0; i<number;i++)
            //{
            //    if (i == 0)
            //        min = list_cate[0].id_cate;
            //    if (i == number - 1)
            //        max = list_cate[i].id_cate;
            //}
            //var list_docu = db.tbl_file.Where(x => x.id_cate <= max & x.id_cate >= min & x.status == 1).ToList();

            //var list_docu = db.tbl_file.Where(x =>  x.status == 1).ToList();

            //Cach 1
            var docu = db.tbl_file.Join(db.tbl_category, c => c.id_cate, p => p.id_cate,
                (c, p) => new
                {
                    id_doc = c.id_doc,
                    file_name = c.file_name,
                    time_up = c.time_up,
                    cate_name = p.cate_name,
                    status = c.status
                }
                ).Where(c => c.status == 1).OrderByDescending(x => x.time_up).ToList();
            var list_docu = new List<list_doc>();
            foreach (var item in docu)
            {
                var x = new list_doc(item.id_doc, item.file_name, item.cate_name, item.time_up.ToString());
                list_docu.Add(x);
            }

            //Cach 2
            //     var list_file = db.tbl_file;
            //     var list_cate = db.tbl_category;
            //     var list_docu = from list_file in lf
            //                     join
            //list_cate in lc on list_file.id_cate equals list_cate.id_cate
            //                     select new
            //                     {
            //                         id_doc = list_file.id_doc
            //                     };

            //var list = db.tbl_subject.Include(x => x.tbl_category.Select(y => y.id_doc)).Include(x => x.id_category).Single(z => z.id_subject == 1).ToList();
            //var list_docu = db.tbl_file.Where(x => x.status == 1 & x.id_cate is (db.tbl_subject.Where(y=>y.id_subject==1).toList()).ToList();
            ViewBag.list_document = list_docu;
            ViewBag.qty_doc = list_docu.Count();
            if (page==null)
            {
                ViewBag.page = 1;
            }
            else
            {
                
                ViewBag.page = page;
                return View();
            }
            //ViewBag.listLink = new List<string>();
            //ViewBag.fileName = new List<string>();
            ////ViewBag.list_link = new List<string>();
            ////ViewBag.count = 0;
            //var category = db.tbl_category.ToList();
            //ViewBag.category = category;
            return View();
        }
          [HttpPost]
          public ActionResult Create(HttpPostedFileBase[] file, string folderName, string cate)
          {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            //GoogleDriveAPIHelper.CreateFolder(folderName);
            string id = GoogleDriveAPIHelper.GetLinkFolder(folderName);
               if (id == "")
               {
                    GoogleDriveAPIHelper.CreateFolder(folderName);
                    id = GoogleDriveAPIHelper.GetLinkFolder(folderName);
               }
               //GoogleDriveAPIHelper.UplaodFileOnDrive(file);

               foreach (var item in file)
               {
                    try
                    {
                         //GoogleDriveAPIHelper.UplaodFileOnDrive(item);
                         GoogleDriveAPIHelper.FileUploadInFolder(id, item);
                    }
                    catch
                    {
                         ViewBag.Success = "File Upload fail on Google Drive";
                         ViewBag.listLink = new List<string>();
                         ViewBag.fileName = new List<string>();
                         return View();
                    }
               }

            var lnk = GoogleDriveAPIHelper.linkDrive;
            var fileName = GoogleDriveAPIHelper.fileName;
            ViewBag.Success = "File Uploaded on Google Drive";
            //List<String> list_link = new List<String>();
            //foreach (var item in lnk)
            //{
            //    string[] temp = item.ToString().Split(' ');
            //    list_link.Add(temp[0]);
            //}
            //ViewBag.list_link = list_link;
            ViewBag.listLink = lnk;
            ViewBag.fileName = fileName;
            GoogleDriveAPIHelper.DeleteFolder();
            int count = lnk.Count();
            //ViewBag.count = count;sau đó thêm sản phẩm vào  danh mục ý
            // Ý tưởng viết tiếp
            //cho chọn danh mục 
            var descrip = db.tbl_description.Where(x => x.ID == 1).FirstOrDefault().description;
            for(int i=0; i<count; i++)
            {
                tbl_file new_file = new tbl_file();
                string temp = fileName[i].ToString();
                string[] temp_1 = temp.Split('.');

                new_file.file_name = temp_1[0].Replace('-',' ');
                new_file.id_cate = Convert.ToInt32(cate);
                new_file.link_drive = lnk[i];
                new_file.status = 0;
                DateTime aDateTime = DateTime.Now;
                new_file.time_up = aDateTime;
                new_file.description = temp_1[0].Replace('-', ' ')+" "+descrip;
                db.tbl_file.Add(new_file);

                db.SaveChanges();

               }
               var category = db.tbl_category.ToList();
               ViewBag.category = category;

               return View();
          }
          // GET: Admin/Home/Details/5

          public ActionResult Details()
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            if (Request.Url.Segments.Count() == 3)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int id = Convert.ToInt32(Request.Url.Segments[5]);
                var doc = db.tbl_file.Where(x => x.id_doc == id).FirstOrDefault();
                ViewBag.document = doc;

                return View();
            }
            
        }
        [HttpPost]
        public ActionResult Update(string doc_name, string description)
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            if (Request.Url.Segments.Count() == 4)
            {
                return RedirectToAction("Index");
            }
            else
            {

                int id = Convert.ToInt32(Request.Url.Segments[4]);
                tbl_file doc = db.tbl_file.Where(x => x.id_doc == id).FirstOrDefault();
                doc.file_name = doc_name;
                doc.description = description;
                db.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = doc.id_doc });
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDescription(string main_description)
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            else
            {
                tbl_description desc = db.tbl_description.Where(x => x.ID == 1).FirstOrDefault();
                desc.description = main_description;
                db.SaveChanges();
                ViewBag.Success = "Cập nhật thành công";
                var description = db.tbl_description.Where(x => x.ID == 1).FirstOrDefault();
                ViewBag.description = description;
                return View();
            }

        }
        // GET: Admin/Home/Create
        public ActionResult Create()
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            ViewBag.listLink = new List<string>();
            ViewBag.fileName = new List<string>();
            //ViewBag.list_link = new List<string>();
            //ViewBag.count = 0;
            var category = db.tbl_category.ToList();
            ViewBag.category = category;
            return View();
           
        }
        public ActionResult ChangePass()
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            else
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(string pass_old,string pass_new, string repass_new)
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            else
            {
                string pass = db.tbl_user.Where(x => x.id_user == id_user).Select(x => x.password).FirstOrDefault();
                string hash_pass_old = GetMD5(pass_old);
                if(pass.Equals(hash_pass_old) && pass_new.Length>=6 &&pass_new.Equals(repass_new))
                {
                     string hash_pass_new = GetMD5(pass_new);
                    tbl_user admin = db.tbl_user.Where(x => x.id_user == id_user).FirstOrDefault();
                    admin.password = hash_pass_new;
                    db.SaveChanges();
                    ViewBag.Success = "Đổi mật khẩu thành công";
                    return View();
                }
                else
                {
                    ViewBag.Error = "Mật khẩu nhập sai hoặc mật khảu mới quá ngắn (ít nhất 6 ký tự)!";
                }
                return View();
            }

        }
        public ActionResult EditDescription()
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            
            else
            {
                
                var description = db.tbl_description.Where(x=>x.ID==1).FirstOrDefault();
                ViewBag.description = description;

                return View();
            }
        }
        //// POST: Admin/Home/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        // GET: Admin/Home/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //POST: Admin/Home/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //    TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //public ActionResult Delete()
        //{
        //    return View("Index", "Home");
        //}
        // GET: Admin/Home/Delete/5
        public ActionResult Delete()
        {
            int id_user = get_ID_User();
            if (id_user == -1)
                return RedirectToAction("Index", "Login");
            if (Request.Url.Segments.Count() == 4)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int id =Convert.ToInt32(Request.Url.Segments[4]);
                tbl_file file = db.tbl_file.Where(s => s.id_doc == id).FirstOrDefault();
                db.tbl_file.Remove(file);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            
        }

        //POST: Admin/Home/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //    TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // lấy id của người dùng
        public int get_ID_User()
        {
            if (Request.Cookies["usercredentials"] != null)
            {
                HttpCookie reqCookie = Request.Cookies["usercredentials"];
                return Convert.ToInt32(reqCookie["UserID"].ToString());
            }
            else//if not have cookies
            {
                if (Session["UserID"] != null)
                {
                    return Convert.ToInt32(Session["UserID"].ToString());
                }

            }
            return -1;

        }
        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}
