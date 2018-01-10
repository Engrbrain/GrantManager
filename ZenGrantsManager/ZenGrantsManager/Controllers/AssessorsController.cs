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
    public class AssessorsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: Assessors
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

                HttpResponseMessage Res = await client.GetAsync("api/Assessors");
                if (Res.IsSuccessStatusCode)
                {
                    var assessorsResponse = Res.Content.ReadAsStringAsync().Result;
                    assessor = JsonConvert.DeserializeObject<List<Assessor>>(assessorsResponse);
                    return View(assessor);
                }
                else
                {
                    this.AddNotification("Assessors could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(assessor);
                }

            }
        }

        // GET: Assessors/Details/5
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
            List<Assessor> assessor = new List<Assessor>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Assessors/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var assessorResponse = Res.Content.ReadAsStringAsync().Result;
                    Assessor myassessor = JsonConvert.DeserializeObject<Assessor>(assessorResponse);
                    return View(myassessor);
                }
                else
                {
                    this.AddNotification("Unable to display Assessors information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: Assessors/Create
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

        // POST: Assessors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AssessorCode,AssessorName,AssessorEmail,AssessorPassword,OrganizationID,CreatedDate,isDeleted,TimeStamp")] Assessor assessor)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (ModelState.IsValid)
            {
                assessor.CreatedDate = DateTime.Now;
                assessor.isDeleted = false;
                assessor.UserId = userID;
                assessor.TimeStamp = DateTime.Now;

                     using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/Assessors", assessor);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Assessor created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Assessor cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, assessor.OrganizationID);
            return View(assessor);
        }

        // GET: Assessors/Edit/5
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
            List<Assessor> assessor = new List<Assessor>();
            Assessor myAssessor = new Assessor();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Assessors/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var assessorResponse = Res.Content.ReadAsStringAsync().Result;
                    myAssessor = JsonConvert.DeserializeObject<Assessor>(assessorResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Assessors information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myAssessor.OrganizationID);

            return View(myAssessor);

        }

        // POST: Assessors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AssessorCode,AssessorName,AssessorEmail,AssessorPassword,OrganizationID,CreatedDate,isDeleted,TimeStamp")] Assessor assessor)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/Assessors/{assessor.ID}", assessor);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Assessors information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Assessors information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, assessor.OrganizationID);
            return View(assessor);
        }

        // GET: Assessors/Delete/5
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
            List<Assessor> assessor = new List<Assessor>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Assessors/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var assessorResponse = Res.Content.ReadAsStringAsync().Result;
                    Assessor myassessor = JsonConvert.DeserializeObject<Assessor>(assessorResponse);
                    return View(myassessor);
                }
                else
                {
                    this.AddNotification("Unable to display Assessors information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: Assessors/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/Assessors/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Assessors deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Assessors  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }
        
    }
}
