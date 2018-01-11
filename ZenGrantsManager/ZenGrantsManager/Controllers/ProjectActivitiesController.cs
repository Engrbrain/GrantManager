using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZenGrantsManager.Extensions;
using ZenGrantsManager.Models;

namespace ZenGrantsManager.Controllers
{
    public class ProjectActivitiesController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: ProjectActivities
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<ProjectActivity> projectActivity = new List<ProjectActivity>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProjectActivities");
                if (Res.IsSuccessStatusCode)
                {
                    var projectActivityResponse = Res.Content.ReadAsStringAsync().Result;
                    projectActivity = JsonConvert.DeserializeObject<List<ProjectActivity>>(projectActivityResponse);
                    return View(projectActivity);
                }
                else
                {
                    this.AddNotification("Project Activity could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(projectActivity);
                }

            }
        }

        // GET: ProjectActivities/Details/5
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
            List<ProjectActivity> projectActivity = new List<ProjectActivity>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectActivities/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectActivityResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectActivity myProjectActivity = JsonConvert.DeserializeObject<ProjectActivity>(projectActivityResponse);
                    return View(myProjectActivity);
                }
                else
                {
                    this.AddNotification("Unable to display Project Activity information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProjectActivities/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            ViewBag.DependencyID = await ProjectActivitySelectList(token);
            ViewBag.OrganizationID = await OrganizationSelectList(token);
            ViewBag.ProjectID = await ProjectSelectList(token);
            ViewBag.ProjectMeetingID = await ProjectMeetingSelectList(token);
            ViewBag.ProjectTeamID = await ProjectTeamSelectList(token);
            return View();
        }

        // POST: ProjectActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ActivityTitle,ActivityDescription,ActivityDocumentID,PhoneNumber,Address,StartDate,EndDate,Milestone,Priority,DependencyID,ActivityStatus,ProjectMeetingID,ProjectID,ProjectTeamID,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectActivity projectActivity)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            if (ModelState.IsValid)
            {
                projectActivity.CreatedDate = DateTime.Now;
                projectActivity.isDeleted = false;
                projectActivity.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProjectActivities", projectActivity);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("project Activity created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("project Activity cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.DependencyID = await ProjectActivitySelectListByModel(token, projectActivity.DependencyID);
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectActivity.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectActivity.ProjectID);
            ViewBag.ProjectMeetingID = await ProjectMeetingSelectListByModel(token, projectActivity.ProjectMeetingID);
            ViewBag.ProjectTeamID = await ProjectTeamSelectListByModel(token, projectActivity.ProjectTeamID);

            return View(projectActivity);
        }

        // GET: ProjectActivities/Edit/5
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
            List<ProjectActivity> projectActivity = new List<ProjectActivity>();
            ProjectActivity myProjectActivity = new ProjectActivity();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectActivities/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectActivityResponse = Res.Content.ReadAsStringAsync().Result;
                    myProjectActivity = JsonConvert.DeserializeObject<ProjectActivity>(projectActivityResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Project Activity information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.DependencyID = await ProjectActivitySelectListByModel(token, myProjectActivity.DependencyID);
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProjectActivity.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, myProjectActivity.ProjectID);
            ViewBag.ProjectMeetingID = await ProjectMeetingSelectListByModel(token, myProjectActivity.ProjectMeetingID);
            ViewBag.ProjectTeamID = await ProjectTeamSelectListByModel(token, myProjectActivity.ProjectTeamID);

            return View(myProjectActivity);
        }

        // POST: ProjectActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ActivityTitle,ActivityDescription,ActivityDocumentID,PhoneNumber,Address,StartDate,EndDate,Milestone,Priority,DependencyID,ActivityStatus,ProjectMeetingID,ProjectID,ProjectTeamID,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectActivity projectActivity)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProjectActivities/{projectActivity.ID}", projectActivity);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project Activity information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project Activity information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.DependencyID = await ProjectActivitySelectListByModel(token, projectActivity.DependencyID);
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectActivity.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectActivity.ProjectID);
            ViewBag.ProjectMeetingID = await ProjectMeetingSelectListByModel(token, projectActivity.ProjectMeetingID);
            ViewBag.ProjectTeamID = await ProjectTeamSelectListByModel(token, projectActivity.ProjectTeamID);
            return View(projectActivity);
        }

        // GET: ProjectActivities/Delete/5
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
            List<ProjectActivity> projectActivity = new List<ProjectActivity>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectActivities/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectActivityResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectActivity myProjectActivity = JsonConvert.DeserializeObject<ProjectActivity>(projectActivityResponse);
                    return View(myProjectActivity);
                }
                else
                {
                    this.AddNotification("Unable to display Project Activity information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProjectActivities/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProjectActivities/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Project Activity deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Project Activity cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

      
    }
}
