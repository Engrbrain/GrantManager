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
    public class ProjectTeamsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: ProjectTeams
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion


            List<ProjectTeam> projectTeam = new List<ProjectTeam>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProjectTeams");
                if (Res.IsSuccessStatusCode)
                {
                    var ProjectTeamResponse = Res.Content.ReadAsStringAsync().Result;
                    projectTeam = JsonConvert.DeserializeObject<List<ProjectTeam>>(ProjectTeamResponse);
                    return View(projectTeam);
                }
                else
                {
                    this.AddNotification("ProjectTeam  could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(projectTeam);
                }

            }

        }

        // GET: ProjectTeams/Details/5
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
            List<ProjectTeam> projectTeam = new List<ProjectTeam>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectTeams/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var ProjectTeamResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectTeam myProjectTeam = JsonConvert.DeserializeObject<ProjectTeam>(ProjectTeamResponse);
                    return View(myProjectTeam);
                }
                else
                {
                    this.AddNotification("Unable to display ProjectTeam  information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProjectTeams/Create
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

        // POST: ProjectTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,TeamMemberReference,Fullname,EmailAddress,PhoneNumber,Address,State,Country,KPI,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID,TeamMemberPhoto,UserId")] ProjectTeam projectTeam)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            if (ModelState.IsValid)
            {
                projectTeam.CreatedDate = DateTime.Now;
                projectTeam.isDeleted = false;
                projectTeam.TimeStamp = DateTime.Now;
                HttpPostedFileBase file = Request.Files["ImageData"];
                projectTeam.TeamMemberPhoto = ConvertToBytes(file);
                projectTeam.UserId = userID;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProjectTeams", projectTeam);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("ProjectTeam  created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("ProjectTeam  cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectTeam.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectTeam.ProjectID);



            return View(projectTeam);
        }

        // GET: ProjectTeams/Edit/5
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
            List<ProjectTeam> ProjectTeam = new List<ProjectTeam>();
            ProjectTeam myProjectTeam = new ProjectTeam();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectTeams/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var ProjectTeamResponse = Res.Content.ReadAsStringAsync().Result;
                    myProjectTeam = JsonConvert.DeserializeObject<ProjectTeam>(ProjectTeamResponse);
                }
                else
                {
                    this.AddNotification("Unable to display ProjectTeam  information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProjectTeam.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, myProjectTeam.ProjectID);
            return View(myProjectTeam);
        }

        // POST: ProjectTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,TeamMemberReference,Fullname,EmailAddress,PhoneNumber,Address,State,Country,KPI,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID,TeamMemberPhoto,UserId")] ProjectTeam projectTeam)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProjectTeams/{projectTeam.ID}", projectTeam);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("ProjectTeam  information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("ProjectTeam  information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectTeam.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectTeam.ProjectID);

            return View(projectTeam);
        }

        // GET: ProjectTeams/Delete/5
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
            List<ProjectTeam> projectTeam = new List<ProjectTeam>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectTeams/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var ProjectTeamResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectTeam myProjectTeam = JsonConvert.DeserializeObject<ProjectTeam>(ProjectTeamResponse);
                    return View(myProjectTeam);
                }
                else
                {
                    this.AddNotification("Unable to display ProjectTeam  information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProjectTeams/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProjectTeams/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("ProjectTeam  deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("ProjectTeam  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
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
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectTeams/{Id}");
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
