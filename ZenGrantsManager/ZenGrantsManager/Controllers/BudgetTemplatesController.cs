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
    public class BudgetTemplatesController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: BudgetTemplates
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<BudgetTemplate> budgetTemplate = new List<BudgetTemplate>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/BudgetTemplates");
                if (Res.IsSuccessStatusCode)
                {
                    var budgetTemplateResponse = Res.Content.ReadAsStringAsync().Result;
                    budgetTemplate = JsonConvert.DeserializeObject<List<BudgetTemplate>>(budgetTemplateResponse);
                    return View(budgetTemplate);
                }
                else
                {
                    this.AddNotification("Budget Template could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(budgetTemplate);
                }

            }
        }

        // GET: BudgetTemplates/Details/5
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
            List<BudgetTemplate> budgetTemplate = new List<BudgetTemplate>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/BudgetTemplates/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var budgetTemplateResponse = Res.Content.ReadAsStringAsync().Result;
                    BudgetTemplate mybudgetTemplate = JsonConvert.DeserializeObject<BudgetTemplate>(budgetTemplateResponse);
                    return View(mybudgetTemplate);
                }
                else
                {
                    this.AddNotification("Unable to display Budget Temnplate information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: BudgetTemplates/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            ViewBag.OrganizationID = await OrganizationSelectList(token);

            return View();
        }

        // POST: BudgetTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,BudgetItem,BudgetItemDesc,PercentageAllocated,OrganizationID,CreatedDate,isDeleted,TimeStamp")] BudgetTemplate budgetTemplate)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (ModelState.IsValid)
            {
                budgetTemplate.CreatedDate = DateTime.Now;
                budgetTemplate.isDeleted = false;
                budgetTemplate.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/BudgetTemplates", budgetTemplate);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Budget Template created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Budget Template cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, budgetTemplate.OrganizationID);
            
            return View(budgetTemplate);
        }

        // GET: BudgetTemplates/Edit/5
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
            List<BudgetTemplate> budgetTemplate = new List<BudgetTemplate>();
            BudgetTemplate myBudgetTemplate = new BudgetTemplate();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/BudgetTemplates/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var budgetTemplateResponse = Res.Content.ReadAsStringAsync().Result;
                    myBudgetTemplate = JsonConvert.DeserializeObject<BudgetTemplate>(budgetTemplateResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Budget Template information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myBudgetTemplate.OrganizationID);
            return View(myBudgetTemplate);
        }

        // POST: BudgetTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,BudgetItem,BudgetItemDesc,PercentageAllocated,OrganizationID,CreatedDate,isDeleted,TimeStamp")] BudgetTemplate budgetTemplate)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/BudgetTemplates/{budgetTemplate.ID}", budgetTemplate);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Budget Template information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Budget Template information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, budgetTemplate.OrganizationID);
            return View(budgetTemplate);
        }

        // GET: BudgetTemplates/Delete/5
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
            List<BudgetTemplate> budgetTemplate = new List<BudgetTemplate>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/BudgetTemplates/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var assessorResponse = Res.Content.ReadAsStringAsync().Result;
                    Assessor myassessor = JsonConvert.DeserializeObject<Assessor>(assessorResponse);
                    return View(myassessor);
                }
                else
                {
                    this.AddNotification("Unable to display Budget Template information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: BudgetTemplates/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/BudgetTemplates/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Budget Template deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Budget Template  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }
        
    }
}
