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
        string baseurl = "http://localhost:49122/";
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
                        string token = (string)(Session["accessToken"]);
                        // Retrieve UserID of Loggedon User
                        
                        using (var userClient = new HttpClient())
                        {
                            userClient.BaseAddress = new Uri(baseurl);
                            userClient.DefaultRequestHeaders.Clear();
                            userClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            userClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage userRes = await userClient.GetAsync("api/Account/UserInfo");
                            if (userRes.IsSuccessStatusCode)
                            {
                                string userResultContent = userRes.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(userResultContent);
                                string UserID = userDictionary["Userid"];
                                string Username = userDictionary["Email"];
                                Session["UserID"] = UserID;
                                Session["Username"] = Username;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + userRes, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                        this.AddNotification("User Signed in Successfully", NotificationType.SUCCESS);

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
            Session["redirectfrom"] = null;
            return View();

        }



    }
}