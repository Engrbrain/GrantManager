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
    public class ProjectActivityCommentsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();
        string clientpath = "http://localhost:12953/DocumentManagement/ProjectActivityComments/";

        // GET: ProjectActivityComments
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            List<ProjectActivityComment> projectActivityComment = new List<ProjectActivityComment>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProjectActivityComments");
                if (Res.IsSuccessStatusCode)
                {
                    var projectActivityResponse = Res.Content.ReadAsStringAsync().Result;
                    projectActivityComment = JsonConvert.DeserializeObject<List<ProjectActivityComment>>(projectActivityResponse);
                    return View(projectActivityComment);
                }
                else
                {
                    this.AddNotification("Project Comments could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(projectActivityComment);
                }

            }
        }

        // GET: ProjectActivityComments/Details/5
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
            List<ProjectActivityComment> projectActivityComment = new List<ProjectActivityComment>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectActivityComments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectActivityCommentResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectActivityComment myProjectActivityComment = JsonConvert.DeserializeObject<ProjectActivityComment>(projectActivityCommentResponse);
                    return View(myProjectActivityComment);
                }
                else
                {
                    this.AddNotification("Unable to display Project Activity Comments,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProjectActivityComments/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            ViewBag.OrganizationID = await OrganizationSelectList(token);
            ViewBag.ProjectActivityID = await ProjectActivitySelectList(token);
            return View();
        }

        // POST: ProjectActivityComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CommentTitle,CommentDescription,LocalFilePath,FileName,ProjectActivityID,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectActivityComment projectActivityComment)
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
                    var path = Path.Combine(Server.MapPath("~/DocumentManagement/ProjectActivityComments"), fileName);
                    file.SaveAs(path);
                    localfilepath = clientpath + fileName;
                    filename = fileName.ToString();
                };
                projectActivityComment.CreatedDate = DateTime.Now;
                projectActivityComment.isDeleted = false;
                projectActivityComment.LocalFilePath = localfilepath;
                projectActivityComment.FileName = filename;
                projectActivityComment.UserId = userID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProjectActivityComments", projectActivityComment);
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
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectActivityComment.OrganizationID);
            ViewBag.ProjectActivityID = await ProjectActivitySelectListByModel(token, projectActivityComment.ProjectActivityID);
            return View(projectActivityComment);
        }

        // GET: ProjectActivityComments/Edit/5
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<ProjectActivityComment> projectActivityComment = new List<ProjectActivityComment>();
            ProjectActivityComment myProjectActivityComment = new ProjectActivityComment();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectActivityComments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectActivityCommentResponse = Res.Content.ReadAsStringAsync().Result;
                    myProjectActivityComment = JsonConvert.DeserializeObject<ProjectActivityComment>(projectActivityCommentResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Comments,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProjectActivityComment.OrganizationID);
            ViewBag.ProjectActivityID = await ProjectActivitySelectListByModel(token, myProjectActivityComment.ProjectActivityID);
            return View(myProjectActivityComment);
        }

        // POST: ProjectActivityComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CommentTitle,CommentDescription,ProjectActivityID,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectActivityComment projectActivityComment)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProjectActivityComments/{projectActivityComment.ID}", projectActivityComment);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("comment modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("comment cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectActivityComment.OrganizationID);
            ViewBag.ProjectActivityID = await ProjectActivitySelectListByModel(token, projectActivityComment.ProjectActivityID);

            return View(projectActivityComment);
        }

        // GET: ProjectActivityComments/Delete/5
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
            List<ProjectActivityComment> projectActivityComment = new List<ProjectActivityComment>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectActivityComments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectActivityCommentResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectActivityComment myProjectActivityComment = JsonConvert.DeserializeObject<ProjectActivityComment>(projectActivityCommentResponse);
                    return View(myProjectActivityComment);
                }
                else
                {
                    this.AddNotification("Unable to display Project Activity Comments,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProjectActivityComments/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProjectActivityComments/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Comment deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Comment cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }
        
    }
}
