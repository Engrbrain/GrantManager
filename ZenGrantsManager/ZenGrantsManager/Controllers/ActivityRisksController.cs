using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZenGrantsManager.Extensions;
using ZenGrantsManager.Models;

namespace ZenGrantsManager.Controllers
{
    public class ActivityRisksController : Controller
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = "http://localhost:49122/";
        // GET: ActivityRisks
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            List<ActivityRisk> activityRisk = new List<ActivityRisk>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ActivityRisks");
                if (Res.IsSuccessStatusCode)
                {
                    var ActRiskresponse = Res.Content.ReadAsStringAsync().Result;
                    activityRisk = JsonConvert.DeserializeObject<List<ActivityRisk>>(ActRiskresponse);
                    return View(activityRisk);
                }
                else
                {
                    this.AddNotification("Activity Risks could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(activityRisk);
                }

            }
        }

        // GET: ActivityRisks/Details/5
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
            List<ActivityRisk> activityRisk = new List<ActivityRisk>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ActivityRisks/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var actRiskResponse = Res.Content.ReadAsStringAsync().Result;
                    ActivityRisk myActivityRisk = JsonConvert.DeserializeObject<ActivityRisk>(actRiskResponse);
                    return View(myActivityRisk);
                }
                else
                {
                    this.AddNotification("Unable to display Activity Risk information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ActivityRisks/Create
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

        // POST: ActivityRisks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,RiskTitle,RiskDescription,RiskMitigation,RiskStatus,RiskDocument,ProjectActivityID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] ActivityRisk activityRisk)
        {
            if (ModelState.IsValid)
            {
                activityRisk.UserId = userID;
                activityRisk.CreatedDate = DateTime.Now;
                activityRisk.isDeleted = false;
               
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ActivityRisks", activityRisk);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Activity Risk created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Activity Risk cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
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
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", activityRisk.OrganizationID);

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
                    ViewBag.ProjectActivityID = new SelectList(projectactivity, "ID", "ActivityTitle", activityRisk.ProjectActivityID);

                }


            }
            return View(activityRisk);
        }

        // GET: ActivityRisks/Edit/5
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
            List<ActivityRisk> activityRisk = new List<ActivityRisk>();
            ActivityRisk myActivityRisk = new ActivityRisk();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ActivityRisks/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var actRiskResponse = Res.Content.ReadAsStringAsync().Result;
                    myActivityRisk = JsonConvert.DeserializeObject<ActivityRisk>(actRiskResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Activity Risk information,please contact Administrator" + Res, NotificationType.ERROR);
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
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", myActivityRisk.OrganizationID);

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
                    ViewBag.ProjectActivityID = new SelectList(projectactivity, "ID", "ActivityTitle", myActivityRisk.ProjectActivityID);

                }


            }

            return View(myActivityRisk);
        }

        // POST: ActivityRisks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RiskTitle,RiskDescription,RiskMitigation,RiskStatus,RiskDocument,ProjectActivityID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] ActivityRisk activityRisk)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ActivityRisks/{activityRisk.ID}", activityRisk);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Activity Risk information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Activity Risk information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
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
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", activityRisk.OrganizationID);

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
                    ViewBag.ProjectActivityID = new SelectList(projectactivity, "ID", "ActivityTitle", activityRisk.ProjectActivityID);

                }


            }
            return View(activityRisk);
        }

        // GET: ActivityRisks/Delete/5
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
            List<ActivityRisk> activityRisk = new List<ActivityRisk>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ActivityRisks/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var actRiskResponse = Res.Content.ReadAsStringAsync().Result;
                    ActivityRisk myActivityRisk = JsonConvert.DeserializeObject<ActivityRisk>(actRiskResponse);
                    return View(myActivityRisk);
                }
                else
                {
                    this.AddNotification("Unable to display Activity Risk information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

        }

        // POST: ActivityRisks/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ActivityRisks/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Activity Risk Record deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Activity Risk cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                } 

            }
        }

       
    }
}
