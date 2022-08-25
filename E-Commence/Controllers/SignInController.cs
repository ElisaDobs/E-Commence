using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using E_Commence.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;
using E_Commence.Common;

namespace E_Commence.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        public async Task<ActionResult> LogIn()
        {
            if (Settings.AppSession.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = (string)Settings.AppSession["ErrorMessage"];
                Settings.AppSession.Remove("ErrorMessage");
            }
            else
            {
                SysUser sysUser = new SysUser();
                sysUser.Username = ConfigurationManager.AppSettings["auth_username"];
                sysUser.Password = ConfigurationManager.AppSettings["auth_password"];
                var token = await ConsumeApi.PostTokenAsync($"api/Login/Authorize", sysUser);
                Settings.AppSession.Add("token", token);
            }
            return View();
        }

        public async Task<ActionResult> ValidateUser(LogIn logIn)
        {
            try
            {
                var response = await ConsumeApi.PostAsync($"api/Login/ValidateUser", logIn, Settings.AppSession["token"].ToString());
                if (response != null)
                {
                    ResponseObject responseObject = JsonConvert.DeserializeObject<ResponseObject>(response.ToString());
                    if (Guid.Empty != Guid.Parse(responseObject.Message))
                    {
                        return RedirectToAction("ListProduct", "Product", new { uid = responseObject.Message });
                    }
                }
                Settings.AppSession.Add("ErrorMessage", "Please provide the correct username and password!");

                return RedirectToAction("LogIn");
            }
            catch(Exception)
            {
                Settings.AppSession.Add("ErrorMessage", "Please provide the correct username and password!");

                return RedirectToAction("LogIn");
            }
        }

        public async Task<ActionResult> RegisterUser(SignIn signIn)
        {
            if (signIn.Password == signIn.ConfirmPassword)
            {
                var response = await ConsumeApi.PostAsync($"api/RegisterUser/CreateUser", signIn, Settings.AppSession["token"].ToString());
                if (response != null)
                {
                    return RedirectToAction("LogIn", "SignIn");
                }
            }

            return RedirectToAction("Register", "SignIn");
        }
    }
}