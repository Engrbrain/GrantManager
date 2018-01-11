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
    public class ProjectsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: Projects
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion


            List<Project> project = new List<Project>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/Projects");
                if (Res.IsSuccessStatusCode)
                {
                    var projectResponse = Res.Content.ReadAsStringAsync().Result;
                    project = JsonConvert.DeserializeObject<List<Project>>(projectResponse);
                    return View(project);
                }
                else
                {
                    this.AddNotification("Project  could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(project);
                }

            }

        }

        // GET: Projects/Details/5
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
            List<Project> project = new List<Project>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Projects/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectResponse = Res.Content.ReadAsStringAsync().Result;
                    Project myProject = JsonConvert.DeserializeObject<Project>(projectResponse);
                    return View(myProject);
                }
                else
                {
                    this.AddNotification("Unable to display Project  information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: Projects/Create
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
            ViewBag.ProgrammeID = await ProgrammeSelectList(token);

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProjectReference,ProjectName,ProjectDescription,ProgrammeDesc,ProjectStartDate,ProjectDueDate,ProjectContigencyPeriod,AmountAllocated,BalanceAmount,ProjectStatus,OrganizationID,ProgApplicationID,ProgrammeID,CreatedDate,isDeleted,TimeStamp,ProjectLogo")] Project project)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            if (ModelState.IsValid)
            {
                project.CreatedDate = DateTime.Now;
                project.isDeleted = false;
                project.TimeStamp = DateTime.Now;
                HttpPostedFileBase file = Request.Files["ImageData"];
                project.ProjectLogo = ConvertToBytes(file);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/Projects", project);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project  created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project  cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, project.OrganizationID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, project.ProgApplicationID);
            ViewBag.ProgrammeID = await ProgrammeSelectListByModel(token, project.ProgrammeID);

          

            return View(project);
        }

        // GET: Projects/Edit/5
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
            List<Project> project = new List<Project>();
            Project myProject = new Project();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Projects/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectResponse = Res.Content.ReadAsStringAsync().Result;
                    myProject = JsonConvert.DeserializeObject<Project>(projectResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Project  information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProject.OrganizationID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, myProject.ProgApplicationID);
            ViewBag.ProgrammeID = await ProgrammeSelectListByModel(token, myProject.ProgrammeID);
            return View(myProject);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProjectReference,ProjectName,ProjectDescription,ProgrammeDesc,ProjectStartDate,ProjectDueDate,ProjectContigencyPeriod,AmountAllocated,BalanceAmount,ProjectStatus,OrganizationID,ProgApplicationID,ProgrammeID,CreatedDate,isDeleted,TimeStamp,ProjectLogo")] Project project)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/Projects/{project.ID}", project);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project  information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project  information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, project.OrganizationID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, project.ProgApplicationID);
            ViewBag.ProgrammeID = await ProgrammeSelectListByModel(token, project.ProgrammeID);

            return View(project);
        }

        // GET: Projects/Delete/5
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
            List<Project> project = new List<Project>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Projects/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectResponse = Res.Content.ReadAsStringAsync().Result;
                    Project myProject = JsonConvert.DeserializeObject<Project>(projectResponse);
                    return View(myProject);
                }
                else
                {
                    this.AddNotification("Unable to display Project  information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: Projects/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/Projects/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Project  deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Project  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)

        {

            byte[] imageBytes = null;

            BinaryReader reader = new BinaryReader(image.InputStream);

            imageBytes = reader.ReadBytes((int)image.ContentLength);

            return imageBytes;

        }

        public async Task<ActionResult> RetrieveImage(int id)

        {

            byte[] Logo = await GetImageFromDataBase(id);

            if (Logo != null)

            { return File(Logo, "image/jpg"); }

            else { return null; }

        }

        public async Task<byte[]> GetImageFromDataBase(int Id)

        {
            List<Organization> organization = new List<Organization>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Projects/{Id}");
                byte[] Logo = null;
                if (Res.IsSuccessStatusCode)
                {
                    var orgResponse = Res.Content.ReadAsStringAsync().Result;
                    Organization myorganization = JsonConvert.DeserializeObject<Organization>(orgResponse);
                    Logo = myorganization.OrgLogo;

                }
                return Logo;

            }



        }
    }
}
