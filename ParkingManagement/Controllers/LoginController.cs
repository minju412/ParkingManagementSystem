using ParkingManagement.Models;
using ParkingManagement.ViewModel;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ParkingManagement.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Logout()
        {
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            HttpContext.Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            return Redirect("/login/login");
        }

        [HttpGet]
        public ActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model) //UserModel 말고 LoginViewModel 사용!
        {
            // ID, 비밀번호 - 필수
            if (ModelState.IsValid)
            {
                try
                {
                    model.ConvertPassword(); //비밀번호 암호화
                    var user = model.GetLoginUser();

                    //로그인에 성공했을 때
                    if (user != null)
                    {
                        // 세션에 로그인 정보 담음
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
                        {
                            Domain = "localhost",
                            Expires = DateTime.Now.AddHours(4),
                            Value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(model.Email, false, 60))
                        };
                        HttpContext.Response.Cookies.Add(cookie);

                        return Redirect("/");
                    }

                    //로그인에 실패했을 때
                    throw new Exception("사용자 ID 혹은 비밀번호가 올바르지 않습니다");
                }
                catch (Exception ex)
                {
                    return Redirect($"/login/login?msg={HttpUtility.UrlEncode(ex.Message)}");
                }
            }
            return View();
        }


        public ActionResult Register(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel input)
        {
            try
            {
                string password2 = Request.Form["password2"];

                if (input.Password != password2)
                {
                    throw new Exception("Password와 Confirm Password가 다릅니다");
                }

                // 비밀번호 암호화
                input.ConvertPassword();

                // db에 insert
                if (input.User_Name == "admin") // admin 이라면
                {
                    input.Admin_Register();
                }
                else
                {
                    input.User_Register();
                }

                // 회원가입 성공
                return Redirect("/login/login");
            }
            catch (Exception ex)
            {
                // 실패
                return Redirect($"/login/register?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }



    }
}