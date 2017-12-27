using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ZenGrantsManager.Extensions;
using ZenGrantsManager.Models;
using Newtonsoft.Json;

namespace ZenGrantsManager.Controllers.UserManagement
{
    public class LoginController : Controller
    {
       
        
        string tokenurl = "http://localhost:49122/token";
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "username,password,grant_type")] Login login)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", login.username),
                    new KeyValuePair<string, string>("password", login.password)
                });
                    HttpResponseMessage result = await client.PostAsync(tokenurl, content);

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (result.IsSuccessStatusCode)
                    {
                        string resultContent = result.Content.ReadAsStringAsync().Result;
                        Dictionary<string, string> tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                        string accessToken = tokenDictionary["access_token"];
                        Session["accessToken"] = accessToken;
                        this.AddNotification("User Registered Successfully", NotificationType.SUCCESS);

                        if (Session["redirectfrom"] != null)
                        {
                            if (!string.IsNullOrEmpty(Session["redirectfrom"].ToString()))
                            {
                               string redirecturl = (string)(Session["redirectfrom"]);
                                string[] viewlist = redirecturl.Split('-');
                                return RedirectToAction(viewlist[1].ToString(), viewlist[0].ToString());

                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        
                    }
                    else
                    {
                        this.AddNotification("User could not be Logged in at the time, Contact Administrator", NotificationType.ERROR);
                    }
                }
            }

            return View();

        }



    }
}