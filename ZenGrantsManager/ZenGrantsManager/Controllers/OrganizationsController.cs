using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZenGrantsManager.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZenGrantsManager.Extensions;

namespace ZenGrantsManager.Controllers
{
    

    public class OrganizationsController : Controller
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = "http://localhost:49122/";
        // GET: Organizations
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            string token = String.Empty;
            string userID = String.Empty;
            if (Session["accessToken"] != null)
            {
                if (!string.IsNullOrEmpty(Session["accessToken"].ToString()))
                {
                    token = (string)(Session["accessToken"]);
                    string useridsession = (string)(Session["UserID"]);
                    if (!string.IsNullOrEmpty(useridsession))
                    {
                        userID = (string)(Session["UserID"]);
                    }
                    else
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseurl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage Res = await client.GetAsync("api/Account/UserInfo");
                            if (Res.IsSuccessStatusCode)
                            {
                                string resultContent = Res.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                                string UserID = userDictionary["Userid"];
                                Session["UserID"] = UserID;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + Res, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                    }
                }
                else
                {
                    Session["redirectfrom"] = "Organizations-Index";
                    return RedirectToAction("index", "Login");
                }
            }
            else
            {
                Session["redirectfrom"] = "Organizations-Index";
                return RedirectToAction("index", "Login");
            }

            #endregion
            List<Organization> organization = new List<Organization>();
         

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                HttpResponseMessage Res = await client.GetAsync("api/Organizations");
                if (Res.IsSuccessStatusCode)
                {  
                    var orgResponse = Res.Content.ReadAsStringAsync().Result;  
                    organization = JsonConvert.DeserializeObject<List<Organization>>(orgResponse);
                    return View(organization);
                }
                else
                {
                     this.AddNotification("Organization List could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(organization);
                }
                
            }
        }
            // GET: Organizations/Details/5
            public async Task<ActionResult> Details(int? id)
        {
            #region USERVALIDATION
            string token = String.Empty;
            string userID = String.Empty;
            if (Session["accessToken"] != null)
            {
                if (!string.IsNullOrEmpty(Session["accessToken"].ToString()))
                {
                    token = (string)(Session["accessToken"]);
                    if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
                    {
                        userID = (string)(Session["UserID"]);

                    }
                    else
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseurl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage Res = await client.GetAsync("api/Account/UserInfo");
                            if (Res.IsSuccessStatusCode)
                            {
                                string resultContent = Res.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                                string UserID = userDictionary["Userid"];
                                Session["UserID"] = UserID;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + Res, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                    }
                }
                else
                {
                    Session["redirectfrom"] = "Organizations-Index";
                    return RedirectToAction("index", "Login");
                }
            }
            else
            {
                Session["redirectfrom"] = "Organizations-Index";
                return RedirectToAction("index", "Login");
            }

            #endregion
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            List<Organization> organization = new List<Organization>();
           

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Organizations/{id}"); 
                if (Res.IsSuccessStatusCode)
                {  
                    var orgResponse = Res.Content.ReadAsStringAsync().Result; 
                    organization = JsonConvert.DeserializeObject<List<Organization>>(orgResponse);
                    return View(organization);
                }
                else {
                    this.AddNotification("Unable to display organization information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }
                
            }
            
        }

        // GET: Organizations/Create
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            string token = String.Empty;
            string userID = String.Empty;
            if (Session["accessToken"] != null)
            {
                if (!string.IsNullOrEmpty(Session["accessToken"].ToString()))
                {
                    token = (string)(Session["accessToken"]);
                    if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
                    {
                        userID = (string)(Session["UserID"]);

                    }
                    else
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseurl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage Res = await client.GetAsync("api/Account/UserInfo");
                            if (Res.IsSuccessStatusCode)
                            {
                                string resultContent = Res.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                                string UserID = userDictionary["Userid"];
                                Session["UserID"] = UserID;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + Res, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                    }
                }
                else
                {
                    Session["redirectfrom"] = "Organizations-Index";
                    return RedirectToAction("index", "Login");
                }
            }
            else
            {
                Session["redirectfrom"] = "Organizations-Index";
                return RedirectToAction("index", "Login");
            }

            #endregion
            return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,OrgName,OrgAddress,OrgState,OrgCountry,OrgPhone,OrgEmail,OrgWebsite,CreatedDate,isDeleted,TimeStamp,OrgLogo,UserID")] Organization organization)
        {
            #region USERVALIDATION
            string token = String.Empty;
            string userID = String.Empty;
            if (Session["accessToken"] != null)
            {
                if (!string.IsNullOrEmpty(Session["accessToken"].ToString()))
                {
                    token = (string)(Session["accessToken"]);
                    if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
                    {
                        userID = (string)(Session["UserID"]);

                    }
                    else
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseurl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage Res = await client.GetAsync("api/Account/UserInfo");
                            if (Res.IsSuccessStatusCode)
                            {
                                string resultContent = Res.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                                string UserID = userDictionary["Userid"];
                                Session["UserID"] = UserID;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + Res, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                    }
                }
                else
                {
                    Session["redirectfrom"] = "Organizations-Index";
                    return RedirectToAction("index", "Login");
                }
            }
            else
            {
                Session["redirectfrom"] = "Organizations-Index";
                return RedirectToAction("index", "Login");
            }

            #endregion
            if (ModelState.IsValid)
            {
                organization.UserId = userID;
                organization.CreatedDate = DateTime.Now;
                organization.isDeleted = false;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/Organizations", organization);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Organization record created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else { this.AddNotification("Organization cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }
                    //returning the employee list to view  
                    
                }
            }

            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            #region USERVALIDATION
            string token = String.Empty;
            string userID = String.Empty;
            if (Session["accessToken"] != null)
            {
                if (!string.IsNullOrEmpty(Session["accessToken"].ToString()))
                {
                    token = (string)(Session["accessToken"]);
                    if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
                    {
                        userID = (string)(Session["UserID"]);

                    }
                    else
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseurl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage Res = await client.GetAsync("api/Account/UserInfo");
                            if (Res.IsSuccessStatusCode)
                            {
                                string resultContent = Res.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                                string UserID = userDictionary["Userid"];
                                Session["UserID"] = UserID;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + Res, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                    }
                }
                else
                {
                    Session["redirectfrom"] = "Organizations-Index";
                    return RedirectToAction("index", "Login");
                }
            }
            else
            {
                Session["redirectfrom"] = "Organizations-Index";
                return RedirectToAction("index", "Login");
            }

            #endregion
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            List<Organization> organization = new List<Organization>();
            

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Organizations/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var orgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(orgResponse);
                    return View(organization);
                }
                else
                {
                    this.AddNotification("Unable to display organization information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,OrgName,OrgAddress,OrgState,OrgCountry,OrgPhone,OrgEmail,OrgWebsite,CreatedDate,isDeleted,TimeStamp,OrgLogo")] Organization organization)
        {

            #region USERVALIDATION
            string token = String.Empty;
            string userID = String.Empty;
            if (Session["accessToken"] != null)
            {
                if (!string.IsNullOrEmpty(Session["accessToken"].ToString()))
                {
                    token = (string)(Session["accessToken"]);
                    if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
                    {
                        userID = (string)(Session["UserID"]);

                    }
                    else
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseurl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage Res = await client.GetAsync("api/Account/UserInfo");
                            if (Res.IsSuccessStatusCode)
                            {
                                string resultContent = Res.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                                string UserID = userDictionary["Userid"];
                                Session["UserID"] = UserID;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + Res, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                    }
                }
                else
                {
                    Session["redirectfrom"] = "Organizations-Index";
                    return RedirectToAction("index", "Login");
                }
            }
            else
            {
                Session["redirectfrom"] = "Organizations-Index";
                return RedirectToAction("index", "Login");
            }

            #endregion
            if (ModelState.IsValid)
            {
                organization.UserId = userID;
                organization.CreatedDate = DateTime.Now;
                organization.isDeleted = false;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/Organizations/{organization.ID}", organization);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Organization information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Organization information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }
                    //returning the employee list to view  

                }
            }
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            #region USERVALIDATION
            string token = String.Empty;
            string userID = String.Empty;
            if (Session["accessToken"] != null)
            {
                if (!string.IsNullOrEmpty(Session["accessToken"].ToString()))
                {
                    token = (string)(Session["accessToken"]);
                    if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
                    {
                        userID = (string)(Session["UserID"]);

                    }
                    else
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseurl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage Res = await client.GetAsync("api/Account/UserInfo");
                            if (Res.IsSuccessStatusCode)
                            {
                                string resultContent = Res.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                                string UserID = userDictionary["Userid"];
                                Session["UserID"] = UserID;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + Res, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                    }
                }
                else
                {
                    Session["redirectfrom"] = "Organizations-Index";
                    return RedirectToAction("index", "Login");
                }
            }
            else
            {
                Session["redirectfrom"] = "Organizations-Index";
                return RedirectToAction("index", "Login");
            }

            #endregion
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Organization> organization = new List<Organization>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Organizations/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var orgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(orgResponse);
                    return View(organization);
                }
                else
                {
                    this.AddNotification("Unable to display organization information,please contact Administrator", NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> DeleteConfirmed(int id)
        {
            #region USERVALIDATION
            string token = String.Empty;
            string userID = String.Empty;
            if (Session["accessToken"] != null)
            {
                if (!string.IsNullOrEmpty(Session["accessToken"].ToString()))
                {
                    token = (string)(Session["accessToken"]);
                    if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
                    {
                        userID = (string)(Session["UserID"]);

                    }
                    else
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseurl);
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                            HttpResponseMessage Res = await client.GetAsync("api/Account/UserInfo");
                            if (Res.IsSuccessStatusCode)
                            {
                                string resultContent = Res.Content.ReadAsStringAsync().Result;
                                Dictionary<string, string> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultContent);
                                string UserID = userDictionary["Userid"];
                                Session["UserID"] = UserID;
                            }
                            else
                            {
                                this.AddNotification("Cannot Retrieve user information at this time, Please contact administrator" + Res, NotificationType.ERROR);
                                return RedirectToAction("index", "Login");
                            }

                        }

                    }
                }
                else
                {
                    Session["redirectfrom"] = "Organizations-Index";
                    return RedirectToAction("index", "Login");
                }
            }
            else
            {
                Session["redirectfrom"] = "Organizations-Index";
                return RedirectToAction("index", "Login");
            }

            #endregion

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.DeleteAsync($"api/Organizations/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Organization record deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Organization record cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }
                //returning the employee list to view  

            }
        }

     


    }
}
