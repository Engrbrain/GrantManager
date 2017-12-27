using ZenGrantsManager.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZenGrantsManager.Extensions;

namespace ZenGrantsManager.Controllers
{
    public class ManageUserController : Controller
    {
        // POST: Register
        string Baseurl = "http://localhost:49122/";
        private static HttpClient client = new HttpClient();

        public ActionResult Register()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "Email,Password,ConfirmPassword,firstname,lastname,PhoneNumber")] Register register)
        {
            if (ModelState.IsValid)
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.PostAsJsonAsync("api/Account/Register", register);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("User Registered Successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AddNotification("User could not be registered at the moment, Contact Administrator", NotificationType.ERROR);
                }

            }

            return View(register);

        }
      
    }
}