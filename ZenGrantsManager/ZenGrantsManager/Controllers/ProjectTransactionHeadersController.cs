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
    public class ProjectTransactionHeadersController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();
        string clientpath = "http://localhost:12953/DocumentManagement/ProjectTransactions/";

        // GET: ProjectTransactionHeaders
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion


            List<ProjectTransactionHeader> projectTransactionHeader = new List<ProjectTransactionHeader>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProjectTransactionHeaders");
                if (Res.IsSuccessStatusCode)
                {
                    var projectTransactionHeaderResponse = Res.Content.ReadAsStringAsync().Result;
                    projectTransactionHeader = JsonConvert.DeserializeObject<List<ProjectTransactionHeader>>(projectTransactionHeaderResponse);
                    return View(projectTransactionHeader);
                }
                else
                {
                    this.AddNotification("Project TransactionHeader could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(projectTransactionHeader);
                }

            }

        }

        // GET: ProjectTransactionHeaders/Details/5
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
            List<ProjectTransactionHeader> projectTransactionHeader = new List<ProjectTransactionHeader>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectTransactionHeaders/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectTransactionHeaderResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectTransactionHeader myProjectTransactionHeader = JsonConvert.DeserializeObject<ProjectTransactionHeader>(projectTransactionHeaderResponse);
                    return View(myProjectTransactionHeader);
                }
                else
                {
                    this.AddNotification("Unable to display Project TransactionHeader information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProjectTransactionHeaders/Create
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
            ViewBag.ProjectBudgetID = await ProjectBudgetSelectList(token);
            return View();
        }

        // POST: ProjectTransactionHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProjectBudgetID,TransactionRef,ShortText,LocalFilePath,FileName,FiscalYear,Period,TransactionDate,EntryDate,TotalAmount,CreatedDate, isDeleted,TimeStamp,OrganizationID,ProjectID,UserId")] ProjectTransactionHeader projectTransactionHeader)
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
                    var path = Path.Combine(Server.MapPath("~/DocumentManagement/ProjectTransactions"), fileName);
                    file.SaveAs(path);
                    localfilepath = clientpath + fileName;
                    filename = fileName.ToString();
                };

                projectTransactionHeader.CreatedDate = DateTime.Now;
                projectTransactionHeader.isDeleted = false;
                projectTransactionHeader.TimeStamp = DateTime.Now;
                projectTransactionHeader.UserId = userID;
                projectTransactionHeader.LocalFilePath = localfilepath;
                projectTransactionHeader.FileName = filename;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProjectTransactionHeaderss", projectTransactionHeader);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project TransactionHeader created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project TransactionHeader cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }


            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectTransactionHeader.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectTransactionHeader.ProjectID);
            ViewBag.ProjectBudgetID = await ProjectBudgetSelectListByModel(token, projectTransactionHeader.ProjectBudgetID);

            return View(projectTransactionHeader);
        }

        // GET: ProjectTransactionHeaders/Edit/5
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
            List<ProjectTransactionHeader> projectTransactionHeader = new List<ProjectTransactionHeader>();
            ProjectTransactionHeader myProjectTransactionHeader = new ProjectTransactionHeader();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectTransactionHeaders/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectTransactionHeaderResponse = Res.Content.ReadAsStringAsync().Result;
                    myProjectTransactionHeader = JsonConvert.DeserializeObject<ProjectTransactionHeader>(projectTransactionHeaderResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Project TransactionHeader information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProjectTransactionHeader.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, myProjectTransactionHeader.ProjectID);
            ViewBag.ProjectBudgetID = await ProjectBudgetSelectListByModel(token, myProjectTransactionHeader.ProjectBudgetID);
            return View(myProjectTransactionHeader);
        }

        // POST: ProjectTransactionHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProjectBudgetID,TransactionRef,ShortText,LocalFilePath,FileName,FiscalYear,Period,TransactionDate,EntryDate,TotalAmount,CreatedDate, isDeleted,TimeStamp,OrganizationID,ProjectID,UserId")] ProjectTransactionHeader projectTransactionHeader)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProjectTransactionHeaders/{projectTransactionHeader.ID}", projectTransactionHeader);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project TransactionHeader information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project TransactionHeader information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectTransactionHeader.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectTransactionHeader.ProjectID);
            ViewBag.ProjectBudgetID = await ProjectBudgetSelectListByModel(token, projectTransactionHeader.ProjectBudgetID);

            return View(projectTransactionHeader);
        }

        // GET: ProjectTransactionHeaders/Delete/5
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
            List<ProjectTransactionHeader> projectTransactionHeader = new List<ProjectTransactionHeader>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectTransactionHeaders/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectTransactionHeaderResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectTransactionHeader myProjectTransactionHeader = JsonConvert.DeserializeObject<ProjectTransactionHeader>(projectTransactionHeaderResponse);
                    return View(myProjectTransactionHeader);
                }
                else
                {
                    this.AddNotification("Unable to display Project TransactionHeader information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProjectTransactionHeaders/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProjectTransactionHeaders/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Project TransactionHeader deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Project TransactionHeader cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
