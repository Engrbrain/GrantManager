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
using System.IO;

namespace ZenGrantsManager.Controllers
{

    public class ActivityDocumentsController : Controller
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();
        string clientpath = "http://localhost:12953/DocumentManagement/ProjectActivityDocument/";

        // GET: ActivityDocuments
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            List<ActivityDocument> activitydocument = new List<ActivityDocument>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ActivityDocuments");
                if (Res.IsSuccessStatusCode)
                {
                    var ActDocResponse = Res.Content.ReadAsStringAsync().Result;
                    activitydocument = JsonConvert.DeserializeObject<List<ActivityDocument>>(ActDocResponse);
                    return View(activitydocument);
                }
                else
                {
                    this.AddNotification("Activity Documents could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(activitydocument);
                }

            }
        }

        // GET: ActivityDocuments/Details/5
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Details(int? id)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<ActivityDocument> activitydocument = new List<ActivityDocument>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ActivityDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var actDocResponse = Res.Content.ReadAsStringAsync().Result;
                    ActivityDocument myactivitydocument = JsonConvert.DeserializeObject<ActivityDocument>(actDocResponse);
                    return View(myactivitydocument);
                }
                else
                {
                    this.AddNotification("Unable to display Activity Documents information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ActivityDocuments/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            //Get Organization to select List
            List<Organization> organization = new List<Organization>();
            List<ProjectActivity> projectactivity = new List<ProjectActivity>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetOrgSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName");
                }
               

            }

            // Get Project Activity Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProActSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProActResponse = Res.Content.ReadAsStringAsync().Result;
                    projectactivity = JsonConvert.DeserializeObject<List<ProjectActivity>>(ProActResponse);
                    ViewBag.ProjectActivityID = new SelectList(projectactivity, "ID", "ActivityTitle");
                }


            }

            return View();

          
        }

        // POST: ActivityDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DocumentName,DocumentDescription,LocalFilePath,FileName,ProjectActivityID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] ActivityDocument activityDocument)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["file"];
                string localfilepath = string.Empty;
                string filename = string.Empty;
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/DocumentManagement/ProjectActivityDocument"), fileName);
                    file.SaveAs(path);
                    localfilepath = clientpath + fileName;
                    filename = fileName.ToString();
                };
                activityDocument.UserId = userID;
                activityDocument.CreatedDate = DateTime.Now;
                activityDocument.isDeleted = false;
                activityDocument.LocalFilePath = localfilepath;
                activityDocument.FileName = filename;

                using (var client = new HttpClient())
                {
                        client.BaseAddress = new Uri(baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                       
                        HttpResponseMessage Res = await client.PostAsJsonAsync("api/ActivityDocuments", activityDocument);
                        if (Res.IsSuccessStatusCode)
                        {
                            this.AddNotification("Activity Document created successfully", NotificationType.SUCCESS);
                            return RedirectToAction("Index");

                        }
                        else
                        {
                            this.AddNotification("Activity Document cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                            return View();
                        }
                    
                }
            }

            List<Organization> organization = new List<Organization>();
            List<ProjectActivity> projectactivity = new List<ProjectActivity>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetOrgSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", activityDocument.OrganizationID);

                }


            }

            // Get Project Activity Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProActSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProActResponse = Res.Content.ReadAsStringAsync().Result;
                    projectactivity = JsonConvert.DeserializeObject<List<ProjectActivity>>(ProActResponse);
                    ViewBag.ProjectActivityID = new SelectList(projectactivity, "ID", "ActivityTitle", activityDocument.ProjectActivityID);

                }


            }
            return View(activityDocument);
        }

        // GET: ActivityDocuments/Edit/5
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Edit(int? id)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ActivityDocument> activityDocument = new List<ActivityDocument>();
            ActivityDocument myActivityDocument = new ActivityDocument();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ActivityDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var actDocResponse = Res.Content.ReadAsStringAsync().Result;
                     myActivityDocument = JsonConvert.DeserializeObject<ActivityDocument>(actDocResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Activity document information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            //Get Organization to select List
            List<Organization> organization = new List<Organization>();
            List<ProjectActivity> projectactivity = new List<ProjectActivity>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetOrgSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", myActivityDocument.OrganizationID);

                }


            }

            // Get Project Activity Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProActSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProActResponse = Res.Content.ReadAsStringAsync().Result;
                    projectactivity = JsonConvert.DeserializeObject<List<ProjectActivity>>(ProActResponse);
                    ViewBag.ProjectActivityID = new SelectList(projectactivity, "ID", "ActivityTitle", myActivityDocument.ProjectActivityID);

                }


            }

            return View(myActivityDocument);
        }

        // POST: ActivityDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DocumentName,DocumentDescription,ProjectActivityID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] ActivityDocument activityDocument)
        {
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ActivityDocuments/{activityDocument.ID}", activityDocument);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Activity Document information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Activity Document information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }
                    //returning the employee list to view  

                }

            }
            //Get Organization to select List
            List<Organization> organization = new List<Organization>();
            List<ProjectActivity> projectactivity = new List<ProjectActivity>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetOrgSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", activityDocument.OrganizationID);

                }


            }

            // Get Project Activity Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProActSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProActResponse = Res.Content.ReadAsStringAsync().Result;
                    projectactivity = JsonConvert.DeserializeObject<List<ProjectActivity>>(ProActResponse);
                    ViewBag.ProjectActivityID = new SelectList(projectactivity, "ID", "ActivityTitle", activityDocument.ProjectActivityID);

                }


            }
            return View(activityDocument);
        }

        // GET: ActivityDocuments/Delete/5
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Delete(int? id)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            List<ActivityDocument> activitydocument = new List<ActivityDocument>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ActivityDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var actDocResponse = Res.Content.ReadAsStringAsync().Result;
                    ActivityDocument myactivitydocument = JsonConvert.DeserializeObject<ActivityDocument>(actDocResponse);
                    return View(myactivitydocument);
                }
                else
                {
                    this.AddNotification("Unable to display Activity Documents information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ActivityDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.DeleteAsync($"api/ActivityDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Activity Document deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Activity Document  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                } 

            }
        }

       


    }
}
