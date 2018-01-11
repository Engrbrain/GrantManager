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
    public class SelectionCategoriesController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: SelectionCategories
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<SelectionCategory> selectionCategory = new List<SelectionCategory>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/SelectionCategories");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionCategoriesResponse = Res.Content.ReadAsStringAsync().Result;
                    selectionCategory = JsonConvert.DeserializeObject<List<SelectionCategory>>(SelectionCategoriesResponse);
                    return View(selectionCategory);
                }
                else
                {
                    this.AddNotification("SelectionCategories could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(selectionCategory);
                }

            }
        }

        // GET: SelectionCategories/Details/5
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
            List<SelectionCategory> selectionCategory = new List<SelectionCategory>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionCategories/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionCategoryResponse = Res.Content.ReadAsStringAsync().Result;
                    SelectionCategory mySelectionCategory = JsonConvert.DeserializeObject<SelectionCategory>(SelectionCategoryResponse);
                    return View(mySelectionCategory);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionCategories information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: SelectionCategories/Create
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

        // POST: SelectionCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,SelectionQuestionID,AssessorID,ProgApplicationID,AssignedScore,CreatedDate,isDeleted,TimeStamp,OrganizationID")] SelectionCategory selectionCategory)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (ModelState.IsValid)
            {
                selectionCategory.CreatedDate = DateTime.Now;
                selectionCategory.isDeleted = false;
                selectionCategory.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/SelectionCategories", selectionCategory);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("SelectionCategory created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("SelectionCategory cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, selectionCategory.OrganizationID);
           
            return View(selectionCategory);
        }

        // GET: SelectionCategories/Edit/5
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
            List<SelectionCategory> selectionCategory = new List<SelectionCategory>();
            SelectionCategory mySelectionCategory = new SelectionCategory();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionCategories/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionCategoryResponse = Res.Content.ReadAsStringAsync().Result;
                    mySelectionCategory = JsonConvert.DeserializeObject<SelectionCategory>(SelectionCategoryResponse);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionCategories information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, mySelectionCategory.OrganizationID);
           
            return View(mySelectionCategory);

        }

        // POST: SelectionCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,SelectionQuestionID,AssessorID,ProgApplicationID,AssignedScore,CreatedDate,isDeleted,TimeStamp,OrganizationID")] SelectionCategory selectionCategory)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/SelectionCategories/{selectionCategory.ID}", selectionCategory);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("SelectionCategories information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("SelectionCategories information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, selectionCategory.OrganizationID);
           
            return View(selectionCategory);
        }

        // GET: SelectionCategories/Delete/5
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
            List<SelectionCategory> selectionCategory = new List<SelectionCategory>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionCategories/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionCategoryResponse = Res.Content.ReadAsStringAsync().Result;
                    SelectionCategory mySelectionCategory = JsonConvert.DeserializeObject<SelectionCategory>(SelectionCategoryResponse);
                    return View(mySelectionCategory);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionCategories information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: SelectionCategories/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/SelectionCategories/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("SelectionCategories deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("SelectionCategories  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
