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
    public class ProjectBudgetsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: ProjectBudgets
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion


            List<ProjectBudget> projectBudget = new List<ProjectBudget>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/ProjectBudgets");
                if (Res.IsSuccessStatusCode)
                {
                    var projectBudgetResponse = Res.Content.ReadAsStringAsync().Result;
                    projectBudget = JsonConvert.DeserializeObject<List<ProjectBudget>>(projectBudgetResponse);
                    return View(projectBudget);
                }
                else
                {
                    this.AddNotification("Project Budget could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(projectBudget);
                }

            }

        }

        // GET: ProjectBudgets/Details/5
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
            List<ProjectBudget> projectBudget = new List<ProjectBudget>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectBudgets/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectBudgetResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectBudget myProjectBudget = JsonConvert.DeserializeObject<ProjectBudget>(projectBudgetResponse);
                    return View(myProjectBudget);
                }
                else
                {
                    this.AddNotification("Unable to display Project Budget information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: ProjectBudgets/Create
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

        // POST: ProjectBudgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,BudgetItem,BudgetItemDesc,PercentageAllocated,BudgetAmount,ItemActual,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectBudget projectBudget)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            if (ModelState.IsValid)
            {
                projectBudget.CreatedDate = DateTime.Now;
                projectBudget.isDeleted = false;
                projectBudget.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/ProjectBudgets", projectBudget);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project Budget created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project Budget cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }


            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectBudget.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectBudget.ProjectID);

            return View(projectBudget);
        }

        // GET: ProjectBudgets/Edit/5
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
            List<ProjectBudget> projectBudget = new List<ProjectBudget>();
            ProjectBudget myProjectBudget = new ProjectBudget();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectBudgets/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectBudgetResponse = Res.Content.ReadAsStringAsync().Result;
                    myProjectBudget = JsonConvert.DeserializeObject<ProjectBudget>(projectBudgetResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Project Budget information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myProjectBudget.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, myProjectBudget.ProjectID);
            return View(myProjectBudget);
        }

        // POST: ProjectBudgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,BudgetItem,BudgetItemDesc,PercentageAllocated,BudgetAmount,ItemActual,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectBudget projectBudget)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/ProjectBudgets/{projectBudget.ID}", projectBudget);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Project Budget information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Project Budget information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, projectBudget.OrganizationID);
            ViewBag.ProjectID = await ProjectSelectListByModel(token, projectBudget.ProjectID);

            return View(projectBudget);
        }

        // GET: ProjectBudgets/Delete/5
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
            List<ProjectBudget> projectBudget = new List<ProjectBudget>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/ProjectBudgets/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var projectBudgetResponse = Res.Content.ReadAsStringAsync().Result;
                    ProjectBudget myProjectBudget = JsonConvert.DeserializeObject<ProjectBudget>(projectBudgetResponse);
                    return View(myProjectBudget);
                }
                else
                {
                    this.AddNotification("Unable to display Project Budget information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: ProjectBudgets/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/ProjectBudgets/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Project Budget deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Project Budget cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
