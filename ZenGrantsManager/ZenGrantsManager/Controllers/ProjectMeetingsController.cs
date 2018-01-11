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
    public class ProjectMeetingsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: ProjectMeetings
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion


            List<ProjectMeeting> projectMeeting = new List<ProjectMeeting>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProjectMeetings");
                if (Res.IsSuccessStatusCode)
                {
                    var projectMeetingResponse = Res.Content.ReadAsStringAsync().Result;
                    projectMeeting = JsonConvert.DeserializeObject<List<ProjectMeeting>>(projectMeetingResponse);
                    return View(projectMeeting);
                }
                else
                {
                    this.AddNotification("Project Meeting could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(projectMeeting);
                }

            }

        }

        // GET: ProjectMeetings/Details/5
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
            List<ProjectMeeting> projectMeeting = new List<ProjectMeeting>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectMeetings/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectMeetingResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectMeeting myProjectMeeting = JsonConvert.DeserializeObject<ProjectMeeting>(projectMeetingResponse);
                    return View(myProjectMeeting);
                }
                else
                {
                    this.AddNotification("Unable to display Project Meeting information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProjectMeetings/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            ViewBag.OrganizationID = await OrganizationSelectList(token);
            ViewBag.ProjectID = await ProjectSelectList(token);
            return View();
        }

        // POST: ProjectMeetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,MeetingTitle,MeetingDetails,MeetingPurpose,MeetingMedium,DiscussionSummary,MeetingMinutes,MeetingStatus,ProjectID,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectMeeting projectMeeting)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            if (ModelState.IsValid)
            {
                projectMeeting.CreatedDate = DateTime.Now;
                projectMeeting.isDeleted = false;
                projectMeeting.TimeStamp = DateTime.Now;
                projectMeeting.UserId = userID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProjectMeetingss", projectMeeting);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project Meeting created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project Meeting cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }


            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectMeeting.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectMeeting.ProjectID);

            return View(projectMeeting);
        }

        // GET: ProjectMeetings/Edit/5
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
            List<ProjectMeeting> projectMeeting = new List<ProjectMeeting>();
            ProjectMeeting myProjectMeeting = new ProjectMeeting();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectMeetings/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectMeetingResponse = Res.Content.ReadAsStringAsync().Result;
                    myProjectMeeting = JsonConvert.DeserializeObject<ProjectMeeting>(projectMeetingResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Project Meeting information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProjectMeeting.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, myProjectMeeting.ProjectID);
            return View(myProjectMeeting);
        }

        // POST: ProjectMeetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,MeetingTitle,MeetingDetails,MeetingPurpose,MeetingMedium,DiscussionSummary,MeetingMinutes,MeetingStatus,ProjectID,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectMeeting projectMeeting)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProjectMeetings/{projectMeeting.ID}", projectMeeting);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project Meeting information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project Meeting information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectMeeting.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectMeeting.ProjectID);

            return View(projectMeeting);
        }

        // GET: ProjectMeetings/Delete/5
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
            List<ProjectMeeting> projectMeeting = new List<ProjectMeeting>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectMeetings/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectMeetingResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectMeeting myProjectMeeting = JsonConvert.DeserializeObject<ProjectMeeting>(projectMeetingResponse);
                    return View(myProjectMeeting);
                }
                else
                {
                    this.AddNotification("Unable to display Project Meeting information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProjectMeetings/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProjectMeetings/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Project Meeting deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Project Meeting cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }
        
    }
}
