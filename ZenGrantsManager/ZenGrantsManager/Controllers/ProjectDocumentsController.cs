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
    public class ProjectDocumentsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();
        string clientpath = "http://localhost:12953/DocumentManagement/ProjectDocument/";

        // GET: ProjectDocuments
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            List<ProjectDocument> projectDocument = new List<ProjectDocument>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProjectDocuments");
                if (Res.IsSuccessStatusCode)
                {
                    var projectDocumentResponse = Res.Content.ReadAsStringAsync().Result;
                    projectDocument = JsonConvert.DeserializeObject<List<ProjectDocument>>(projectDocumentResponse);
                    return View(projectDocument);
                }
                else
                {
                    this.AddNotification("Project Document could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(projectDocument);
                }

            }
        }

        // GET: ProjectDocuments/Details/5
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
            List<ProjectDocument> projectDocument = new List<ProjectDocument>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectDocumentResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectDocument myProjectDocument = JsonConvert.DeserializeObject<ProjectDocument>(projectDocumentResponse);
                    return View(myProjectDocument);
                }
                else
                {
                    this.AddNotification("Unable to display Project Documents,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProjectDocuments/Create
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

        // POST: ProjectDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DocumentName,DocumentDescription,LocalFilePath,FileName,OrganizationID,ProjectID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectDocument projectDocument)
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
                    var path = Path.Combine(Server.MapPath("~/DocumentManagement/ProjectDocument"), fileName);
                    file.SaveAs(path);
                    localfilepath = clientpath + fileName;
                    filename = fileName.ToString();
                };
                projectDocument.CreatedDate = DateTime.Now;
                projectDocument.isDeleted = false;
                projectDocument.LocalFilePath = localfilepath;
                projectDocument.FileName = filename;
                projectDocument.UserId = userID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProjectDocuments", projectDocument);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Comment Posted successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Comment cannot be posted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectDocument.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectDocument.ProjectID);
            return View(projectDocument);
        }

        // GET: ProjectDocuments/Edit/5
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ProjectDocument> projectDocument = new List<ProjectDocument>();
            ProjectDocument myProjectDocument = new ProjectDocument();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectDocumentResponse = Res.Content.ReadAsStringAsync().Result;
                    myProjectDocument = JsonConvert.DeserializeObject<ProjectDocument>(projectDocumentResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Documents,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProjectDocument.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, myProjectDocument.ProjectID);
            return View(myProjectDocument);
        }

        // POST: ProjectDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DocumentName,DocumentDescription,LocalFilePath,FileName,OrganizationID,ProjectID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectDocument projectDocument)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProjectDocuments/{projectDocument.ID}", projectDocument);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Documents modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Documents cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectDocument.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectDocument.ProjectID);
            return View(projectDocument);
        }

        // GET: ProjectDocuments/Delete/5
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
            List<ProjectDocument> projectDocument = new List<ProjectDocument>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectDocumentResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectDocument myProjectDocument = JsonConvert.DeserializeObject<ProjectDocument>(projectDocumentResponse);
                    return View(myProjectDocument);
                }
                else
                {
                    this.AddNotification("Unable to display Project Documents,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProjectDocuments/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProjectDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Document deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Document cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }
        
    }
}
