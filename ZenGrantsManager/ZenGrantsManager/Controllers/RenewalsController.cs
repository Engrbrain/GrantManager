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
    public class RenewalsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: Renewals
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<Renewal> renewal = new List<Renewal>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/Renewals");
                if (Res.IsSuccessStatusCode)
                {
                    var RenewalsResponse = Res.Content.ReadAsStringAsync().Result;
                    renewal = JsonConvert.DeserializeObject<List<Renewal>>(RenewalsResponse);
                    return View(renewal);
                }
                else
                {
                    this.AddNotification("Renewals could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(renewal);
                }

            }
        }

        // GET: Renewals/Details/5
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
            List<Renewal> renewal = new List<Renewal>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Renewals/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var RenewalResponse = Res.Content.ReadAsStringAsync().Result;
                    Renewal myRenewal = JsonConvert.DeserializeObject<Renewal>(RenewalResponse);
                    return View(myRenewal);
                }
                else
                {
                    this.AddNotification("Unable to display Renewals information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: Renewals/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            ViewBag.OrganizationID = await OrganizationSelectList(token);
            ViewBag.SubscriptionID = await SubscriptionSelectList(token);

            return View();
        }

        // POST: Renewals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,RenewalCode,RenewalName,RenewalEmail,RenewalPassword,OrganizationID,CreatedDate,isDeleted,TimeStamp")] Renewal renewal)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (ModelState.IsValid)
            {
                renewal.UserId = userID;
                renewal.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/Renewals", renewal);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Renewal created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Renewal cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, renewal.OrganizationID);
            ViewBag.SubscriptionID = await SubscriptionSelectListByModel(token, renewal.SubscriptionID);
            return View(renewal);
        }

        // GET: Renewals/Edit/5
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
            List<Renewal> renewal = new List<Renewal>();
            Renewal myRenewal = new Renewal();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Renewals/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var RenewalResponse = Res.Content.ReadAsStringAsync().Result;
                    myRenewal = JsonConvert.DeserializeObject<Renewal>(RenewalResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Renewals information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, myRenewal.OrganizationID);
            ViewBag.SubscriptionID = await SubscriptionSelectListByModel(token, myRenewal.SubscriptionID);

            return View(myRenewal);

        }

        // POST: Renewals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RenewalCode,RenewalName,RenewalEmail,RenewalPassword,OrganizationID,CreatedDate,isDeleted,TimeStamp")] Renewal renewal)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/Renewals/{renewal.ID}", renewal);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Renewals information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Renewals information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, renewal.OrganizationID);
            ViewBag.SubscriptionID = await SubscriptionSelectListByModel(token, renewal.SubscriptionID);
            return View(renewal);
        }

        // GET: Renewals/Delete/5
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
            List<Renewal> renewal = new List<Renewal>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Renewals/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var RenewalResponse = Res.Content.ReadAsStringAsync().Result;
                    Renewal myRenewal = JsonConvert.DeserializeObject<Renewal>(RenewalResponse);
                    return View(myRenewal);
                }
                else
                {
                    this.AddNotification("Unable to display Renewals information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: Renewals/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/Renewals/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Renewals deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Renewals  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
