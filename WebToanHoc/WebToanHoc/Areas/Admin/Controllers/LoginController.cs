using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebToanHoc.Models;
using System.Security.Cryptography;

namespace WebToanHoc.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        DbContext_WebTaiLieu db = new DbContext_WebTaiLieu();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string username, string password, string remember_me)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = db.tbl_user.Where(s => s.username.Equals(username) && s.password.Equals(f_password)).ToList();  //So sánh đối chiếu tài khoản                 
                if (data.Count() > 0)
                {
                    if (remember_me == "True")//remember-me is set
                    {
                        //Create a cookie
                        HttpCookie usercredentialsCookie = new HttpCookie("usercredentials");
                        //add cookies
                        usercredentialsCookie.Values["username"] = username;
                        usercredentialsCookie.Values["password"] = password;
                        usercredentialsCookie.Values["UserID"] = data.FirstOrDefault().id_user.ToString();
                        //set expired
                        usercredentialsCookie.Expires = DateTime.Now.AddDays(10);
                        Response.Cookies.Add(usercredentialsCookie);
                    }
                    Session["UserID"] = data.FirstOrDefault().id_user;//lấy IDUser vào Session 
                                                                     //role user
                    //var dao = new UserDAO();
                    //var usersession = new userlogin();
                    //int id_user = data.FirstOrDefault().UserID;
                    //var info = _db.Profile_User.Where(x => x.UserID == id_user).FirstOrDefault();
                    //usersession.UserID = data.UserID;
                    //usersession.ListPermission = dao.GetListPermission(info.id_User_Type);
                    //usersession.User_Type = info.id_User_Type;
                    //usersession.Role = info.tbl_User_Type.VN_Name;
                    //Session.Add(CommonConstant.USER_SESSION, usersession);
                    return RedirectToAction("Index", "Home");


                }
                else
                {
                    ViewBag.error = "Login failed";
                    TempData["error"] = "Login failed";
                    return RedirectToAction("Index","Login");
                }
            }
            return View();
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

        public ActionResult Logout()
        {
            Session.Clear();//delete all session but keep current session
            Session.RemoveAll();
            Session.Abandon();//delete current session
            Response.Cookies["usercredentials"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Login");
        }
    }
}
