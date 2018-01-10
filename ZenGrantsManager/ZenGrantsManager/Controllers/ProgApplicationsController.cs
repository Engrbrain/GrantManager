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
    public class ProgApplicationsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: ProgApplications
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<ProgApplication> progApplication = new List<ProgApplication>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProgApplications");
                if (Res.IsSuccessStatusCode)
                {
                    var progApplicationResponse = Res.Content.ReadAsStringAsync().Result;
                    progApplication = JsonConvert.DeserializeObject<List<ProgApplication>>(progApplicationResponse);
                    return View(progApplication);
                }
                else
                {
                    this.AddNotification("Application could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(progApplication);
                }

            }
        }

        // GET: ProgApplications/Details/5
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
            List<ProgApplication> progApplication = new List<ProgApplication>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProgApplications/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var progApplicationResponse = Res.Content.ReadAsStringAsync().Result;
                    ProgApplication myProgApplication = JsonConvert.DeserializeObject<ProgApplication>(progApplicationResponse);
                    return View(myProgApplication);
                }
                else
                {
                    this.AddNotification("Unable to display Application information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProgApplications/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {

            ViewBag.OrganizationID = await OrganizationSelectList(token);
            ViewBag.AssessorID = await AssessorSelectList(token);
            ViewBag.ProgrammeID = await ProgrammeSelectList(token);
            return View();
        }

        // POST: ProgApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ApplicantReference,ApplicantName,ApplicantPhoneNumber,ApplicantEmailAddress,ApplicationSummary,Proposal,ApplicantPhoto,OrganizationID,CreatedDate,isDeleted,TimeStamp,Applicationscore,ProgrammeID,AssessorID,applicationStatus")] ProgApplication progApplication)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            if (ModelState.IsValid)
            {
                progApplication.CreatedDate = DateTime.Now;
                progApplication.isDeleted = false;
                progApplication.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProgApplications", progApplication);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Application created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Application cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, progApplication.OrganizationID);
            ViewBag.AssessorID = await AssessorSelectListByModel(token, progApplication.AssessorID);
            ViewBag.ProgrammeID = await ProgrammeSelectListByModel(token, progApplication.ProgrammeID);

            return View(progApplication);
        }

        // GET: ProgApplications/Edit/5
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
            List<ProgApplication> progApplication = new List<ProgApplication>();
            ProgApplication myProgApplication = new ProgApplication();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProgApplications/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var progApplicationResponse = Res.Content.ReadAsStringAsync().Result;
                    myProgApplication = JsonConvert.DeserializeObject<ProgApplication>(progApplicationResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Application information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProgApplication.OrganizationID);
            ViewBag.AssessorID = await AssessorSelectListByModel(token, myProgApplication.AssessorID);
            ViewBag.ProgrammeID = await ProgrammeSelectListByModel(token, myProgApplication.ProgrammeID);

            return View(myProgApplication);
        }

        // POST: ProgApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ApplicantReference,ApplicantName,ApplicantPhoneNumber,ApplicantEmailAddress,ApplicationSummary,Proposal,ApplicantPhoto,OrganizationID,CreatedDate,isDeleted,TimeStamp,Applicationscore,ProgrammeID,AssessorID,applicationStatus")] ProgApplication progApplication)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProgApplications/{progApplication.ID}", progApplication);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Application information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Application information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, progApplication.OrganizationID);
            ViewBag.AssessorID = await AssessorSelectListByModel(token, progApplication.AssessorID);
            ViewBag.ProgrammeID = await ProgrammeSelectListByModel(token, progApplication.ProgrammeID);
            return View(progApplication);
        }

        // GET: ProgApplications/Delete/5
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
            List<ProgApplication> progApplication = new List<ProgApplication>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProgApplications/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var progApplicationResponse = Res.Content.ReadAsStringAsync().Result;
                    ProgApplication myProgApplication = JsonConvert.DeserializeObject<ProgApplication>(progApplicationResponse);
                    return View(myProgApplication);
                }
                else
                {
                    this.AddNotification("Unable to display Application information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProgApplications/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProgApplications/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Application deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Application  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
