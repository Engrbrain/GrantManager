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
    public class FocusAreasController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: FocusAreas
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<Assessor> assessor = new List<Assessor>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/FocusAreas");
                if (Res.IsSuccessStatusCode)
                {
                    var assessorsResponse = Res.Content.ReadAsStringAsync().Result;
                    assessor = JsonConvert.DeserializeObject<List<Assessor>>(assessorsResponse);
                    return View(assessor);
                }
                else
                {
                    this.AddNotification("Focus Areas could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(assessor);
                }

            }
        }

        // GET: FocusAreas/Details/5
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
            List<FocusArea> focusArea = new List<FocusArea>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/FocusAreas/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var focusAreaResponse = Res.Content.ReadAsStringAsync().Result;
                    FocusArea myFocusArea = JsonConvert.DeserializeObject<FocusArea>(focusAreaResponse);
                    return View(myFocusArea);
                }
                else
                {
                    this.AddNotification("Unable to display Focus Area information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: FocusAreas/Create
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

        // POST: FocusAreas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,FocusAreaName,FocusAreaDesc,OrganizationID,CreatedDate,isActive,TimeStamp,UserId")] FocusArea focusArea)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (ModelState.IsValid)
            {
                focusArea.CreatedDate = DateTime.Now;
                focusArea.UserId = userID;
                focusArea.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/FocusAreas", focusArea);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Focus Area created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Focus Area cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, focusArea.OrganizationID);
            return View(focusArea);
        }

        // GET: FocusAreas/Edit/5
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
            List<FocusArea> focusArea = new List<FocusArea>();
            FocusArea myFocusArea = new FocusArea();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/FocusAreas/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var focusAreaResponse = Res.Content.ReadAsStringAsync().Result;
                    myFocusArea = JsonConvert.DeserializeObject<FocusArea>(focusAreaResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Focus Area information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myFocusArea.OrganizationID);

            return View(myFocusArea);
        }

        // POST: FocusAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,FocusAreaName,FocusAreaDesc,OrganizationID,CreatedDate,isActive,TimeStamp,UserId")] FocusArea focusArea)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/FocusAreas/{focusArea.ID}", focusArea);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Focus Area information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Focus Area information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, focusArea.OrganizationID);

            return View(focusArea);
        }

        // GET: FocusAreas/Delete/5
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
            List<FocusArea> focusArea = new List<FocusArea>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/FocusAreas/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var focusAreaResponse = Res.Content.ReadAsStringAsync().Result;
                    FocusArea myFocusArea = JsonConvert.DeserializeObject<FocusArea>(focusAreaResponse);
                    return View(myFocusArea);
                }
                else
                {
                    this.AddNotification("Unable to display Focus Area information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: FocusAreas/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/FocusAreas/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Focus Area deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Focus Area  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
