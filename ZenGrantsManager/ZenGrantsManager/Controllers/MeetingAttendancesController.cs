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
    public class MeetingAttendancesController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: MeetingAttendances
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<MeetingAttendance> meetingAttendance = new List<MeetingAttendance>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/MeetingAttendances");
                if (Res.IsSuccessStatusCode)
                {
                    var meetingAttendanceResponse = Res.Content.ReadAsStringAsync().Result;
                    meetingAttendance = JsonConvert.DeserializeObject<List<MeetingAttendance>>(meetingAttendanceResponse);
                    return View(meetingAttendance);
                }
                else
                {
                    this.AddNotification("Meeting Attendance could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(meetingAttendance);
                }

            }
        }

        // GET: MeetingAttendances/Details/5
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
            List<MeetingAttendance> meetingAttendance = new List<MeetingAttendance>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/MeetingAttendances/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var meetingAttendanceResponse = Res.Content.ReadAsStringAsync().Result;
                    MeetingAttendance myMeetingAttendance = JsonConvert.DeserializeObject<MeetingAttendance>(meetingAttendanceResponse);
                    return View(myMeetingAttendance);
                }
                else
                {
                    this.AddNotification("Unable to display Meeting Attendance information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: MeetingAttendances/Create
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
            ViewBag.ProjectMeetingID = await ProjectMeetingSelectList(token);
            return View();
        }

        // POST: MeetingAttendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,FullName,Designation,EmailAddress,PhoneNumber,AttendanceStatus,ProjectMeetingID,ProjectID,ProjectTeamID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] MeetingAttendance meetingAttendance)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            if (ModelState.IsValid)
            {
                meetingAttendance.CreatedDate = DateTime.Now;
                meetingAttendance.isDeleted = false;
                meetingAttendance.TimeStamp = DateTime.Now;
                meetingAttendance.UserId = userID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/MeetingAttendances", meetingAttendance);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Meeting Attendance created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Meeting Attendance cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, meetingAttendance.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, meetingAttendance.ProjectID);
            ViewBag.ProjectMeetingID = await ProjectMeetingSelectListByModel(token, meetingAttendance.ProjectMeetingID);

            return View(meetingAttendance);
        }

        // GET: MeetingAttendances/Edit/5
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
            List<MeetingAttendance> meetingAttendance = new List<MeetingAttendance>();
            MeetingAttendance myMeetingAttendance = new MeetingAttendance();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/MeetingAttendances/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var meetingAttendanceResponse = Res.Content.ReadAsStringAsync().Result;
                    myMeetingAttendance = JsonConvert.DeserializeObject<MeetingAttendance>(meetingAttendanceResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Meeting Attendance information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myMeetingAttendance.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, myMeetingAttendance.ProjectID);
            ViewBag.ProjectMeetingID = await ProjectMeetingSelectListByModel(token, myMeetingAttendance.ProjectMeetingID);

            return View(myMeetingAttendance);

        }

        // POST: MeetingAttendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,FullName,Designation,EmailAddress,PhoneNumber,AttendanceStatus,ProjectMeetingID,ProjectID,ProjectTeamID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] MeetingAttendance meetingAttendance)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/MeetingAttendances/{meetingAttendance.ID}", meetingAttendance);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Meeting Attendance information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Meeting Attendance information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, meetingAttendance.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, meetingAttendance.ProjectID);
            ViewBag.ProjectMeetingID = await ProjectMeetingSelectListByModel(token, meetingAttendance.ProjectMeetingID);
            return View(meetingAttendance);

        }

        // GET: MeetingAttendances/Delete/5
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
            List<MeetingAttendance> meetingAttendance = new List<MeetingAttendance>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/MeetingAttendances/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var meetingAttendanceResponse = Res.Content.ReadAsStringAsync().Result;
                    MeetingAttendance myMeetingAttendance = JsonConvert.DeserializeObject<MeetingAttendance>(meetingAttendanceResponse);
                    return View(myMeetingAttendance);
                }
                else
                {
                    this.AddNotification("Unable to display Meeting Attendance information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: MeetingAttendances/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/MeetingAttendances/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Meeting Attendance deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Meeting Attendance  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
