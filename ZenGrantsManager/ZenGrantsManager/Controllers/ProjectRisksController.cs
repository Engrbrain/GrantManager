using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class ProjectRisksController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();
        string clientpath = "http://localhost:12953/DocumentManagement/ProjectRisk/";

        // GET: ProjectRisks
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion


            List<ProjectRisk> projectRisk = new List<ProjectRisk>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProjectRisks");
                if (Res.IsSuccessStatusCode)
                {
                    var projectRiskResponse = Res.Content.ReadAsStringAsync().Result;
                    projectRisk = JsonConvert.DeserializeObject<List<ProjectRisk>>(projectRiskResponse);
                    return View(projectRisk);
                }
                else
                {
                    this.AddNotification("Project Risk could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(projectRisk);
                }

            }

        }

        // GET: ProjectRisks/Details/5
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
            List<ProjectRisk> projectRisk = new List<ProjectRisk>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectRisks/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectRiskResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectRisk myProjectRisk = JsonConvert.DeserializeObject<ProjectRisk>(projectRiskResponse);
                    return View(myProjectRisk);
                }
                else
                {
                    this.AddNotification("Unable to display Project Risk information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProjectRisks/Create
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

        // POST: ProjectRisks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,RiskTitle,RiskDescription,RiskMitigation,RiskStatus,LocalFilePath,FileName,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] ProjectRisk projectRisk)
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
                    var path = Path.Combine(Server.MapPath("~/DocumentManagement/Risk"), fileName);
                    file.SaveAs(path);
                    localfilepath = clientpath + fileName;
                    filename = fileName.ToString();
                };

                projectRisk.CreatedDate = DateTime.Now;
                projectRisk.isDeleted = false;
                projectRisk.TimeStamp = DateTime.Now;
                projectRisk.UserId = userID;
                projectRisk.LocalFilePath = localfilepath;
                projectRisk.FileName = filename;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProjectRiskss", projectRisk);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project Risk created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project Risk cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }


            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectRisk.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectRisk.ProjectID);

            return View(projectRisk);
        }

        // GET: ProjectRisks/Edit/5
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
            List<ProjectRisk> projectRisk = new List<ProjectRisk>();
            ProjectRisk myProjectRisk = new ProjectRisk();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectRisks/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectRiskResponse = Res.Content.ReadAsStringAsync().Result;
                    myProjectRisk = JsonConvert.DeserializeObject<ProjectRisk>(projectRiskResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Project Risk information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProjectRisk.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, myProjectRisk.ProjectID);
            return View(myProjectRisk);
        }

        // POST: ProjectRisks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RiskTitle,RiskDescription,RiskMitigation,RiskStatus,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] ProjectRisk projectRisk)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProjectRisks/{projectRisk.ID}", projectRisk);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project Risk information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project Risk information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectRisk.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectRisk.ProjectID);

            return View(projectRisk);
        }

        // GET: ProjectRisks/Delete/5
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
            List<ProjectRisk> projectRisk = new List<ProjectRisk>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectRisks/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectRiskResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectRisk myProjectRisk = JsonConvert.DeserializeObject<ProjectRisk>(projectRiskResponse);
                    return View(myProjectRisk);
                }
                else
                {
                    this.AddNotification("Unable to display Project Risk information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProjectRisks/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProjectRisks/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Project Risk deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Project Risk cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
