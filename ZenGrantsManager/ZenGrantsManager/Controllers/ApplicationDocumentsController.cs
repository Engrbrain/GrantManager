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
    public class ApplicationDocumentsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();
        string clientpath = "http://localhost:12953/DocumentManagement/ApplicationDocument/";

        // GET: ApplicationDocuments
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            List<ApplicationDocument> applicationdocument = new List<ApplicationDocument>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ApplicationDocuments");
                if (Res.IsSuccessStatusCode)
                {
                    var AppDocResponse = Res.Content.ReadAsStringAsync().Result;
                    applicationdocument = JsonConvert.DeserializeObject<List<ApplicationDocument>>(AppDocResponse);
                    return View(applicationdocument);
                }
                else
                {
                    this.AddNotification("Application Documents could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(applicationdocument);
                }

            }

        }

        // GET: ApplicationDocuments/Details/5
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
            List<ApplicationDocument> applicationDocument = new List<ApplicationDocument>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ApplicationDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var appDocResponse = Res.Content.ReadAsStringAsync().Result;
                    ApplicationDocument myapplicationdocument = JsonConvert.DeserializeObject<ApplicationDocument>(appDocResponse);
                    return View(myapplicationdocument);
                }
                else
                {
                    this.AddNotification("Unable to display Application Documents information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ApplicationDocuments/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            ViewBag.OrganizationID = await OrganizationSelectList(token);
            ViewBag.ProgApplicationID = await ProgApplicationSelectList(token);
            return View();
        }

        // POST: ApplicationDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DocumentName,DocumentDescription,LocalFilePath,FileName,OrganizationID,ProgApplicationID,CreatedDate,isDeleted,TimeStamp")] ApplicationDocument applicationDocument)
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
                applicationDocument.CreatedDate = DateTime.Now;
                applicationDocument.isDeleted = false;
                applicationDocument.LocalFilePath = localfilepath;
                applicationDocument.FileName = filename;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ApplicationDocuments", applicationDocument);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Application Document created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Application Document cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, applicationDocument.OrganizationID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, applicationDocument.ProgApplicationID);
            return View(applicationDocument);
        }

        // GET: ApplicationDocuments/Edit/5
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ApplicationDocument> applicationDocument = new List<ApplicationDocument>();
            ApplicationDocument myApplicationDocument = new ApplicationDocument();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ApplicationDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var appDocResponse = Res.Content.ReadAsStringAsync().Result;
                    myApplicationDocument = JsonConvert.DeserializeObject<ApplicationDocument>(appDocResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Activity document information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myApplicationDocument.OrganizationID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, myApplicationDocument.ProgApplicationID);
            return View(myApplicationDocument);
        }

        // POST: ApplicationDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DocumentName,DocumentDescription,OrganizationID,ProgApplicationID,CreatedDate,isDeleted,TimeStamp")] ApplicationDocument applicationDocument)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ApplicationDocuments/{applicationDocument.ID}", applicationDocument);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Application Document information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Application Document information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    } 

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, applicationDocument.OrganizationID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, applicationDocument.ProgApplicationID);

            return View(applicationDocument);
        }

        // GET: ApplicationDocuments/Delete/5
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Delete(int? id)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<ApplicationDocument> applicationDocument = new List<ApplicationDocument>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ApplicationDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var appDocResponse = Res.Content.ReadAsStringAsync().Result;
                    ApplicationDocument myapplicationdocument = JsonConvert.DeserializeObject<ApplicationDocument>(appDocResponse);
                    return View(myapplicationdocument);
                }
                else
                {
                    this.AddNotification("Unable to display Application Documents information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ApplicationDocuments/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ApplicationDocuments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Application Document deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Application Document  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }
    }
}
