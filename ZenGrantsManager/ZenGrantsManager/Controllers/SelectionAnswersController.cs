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
    public class SelectionAnswersController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: SelectionAnswers
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<SelectionAnswer> selectionAnswer = new List<SelectionAnswer>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/SelectionAnswers");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionAnswersResponse = Res.Content.ReadAsStringAsync().Result;
                    selectionAnswer = JsonConvert.DeserializeObject<List<SelectionAnswer>>(SelectionAnswersResponse);
                    return View(selectionAnswer);
                }
                else
                {
                    this.AddNotification("SelectionAnswers could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(selectionAnswer);
                }

            }
        }

        // GET: SelectionAnswers/Details/5
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
            List<SelectionAnswer> selectionAnswer = new List<SelectionAnswer>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionAnswers/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionAnswerResponse = Res.Content.ReadAsStringAsync().Result;
                    SelectionAnswer mySelectionAnswer = JsonConvert.DeserializeObject<SelectionAnswer>(SelectionAnswerResponse);
                    return View(mySelectionAnswer);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionAnswers information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: SelectionAnswers/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            ViewBag.OrganizationID = await OrganizationSelectList(token);
            ViewBag.AssessorID = await AssessorSelectList(token);
            ViewBag.SelectionQuestionID = await SelectionQuestionSelectList(token);
            ViewBag.ProgApplicationID = await ProgApplicationSelectList(token);

            return View();
        }

        // POST: SelectionAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,SelectionQuestionID,AssessorID,ProgApplicationID,AssignedScore,CreatedDate,isDeleted,TimeStamp,OrganizationID")] SelectionAnswer selectionAnswer)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (ModelState.IsValid)
            {
                selectionAnswer.CreatedDate = DateTime.Now;
                selectionAnswer.isDeleted = false;
                selectionAnswer.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/SelectionAnswers", selectionAnswer);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("SelectionAnswer created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("SelectionAnswer cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, selectionAnswer.OrganizationID);
            ViewBag.AssessorID = await AssessorSelectListByModel(token, selectionAnswer.AssessorID);
            ViewBag.SelectionQuestionID = await SelectionQuestionSelectListByModel(token, selectionAnswer.SelectionQuestionID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, selectionAnswer.ProgApplicationID);
            return View(selectionAnswer);
        }

        // GET: SelectionAnswers/Edit/5
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
            List<SelectionAnswer> selectionAnswer = new List<SelectionAnswer>();
            SelectionAnswer mySelectionAnswer = new SelectionAnswer();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionAnswers/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionAnswerResponse = Res.Content.ReadAsStringAsync().Result;
                    mySelectionAnswer = JsonConvert.DeserializeObject<SelectionAnswer>(SelectionAnswerResponse);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionAnswers information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, mySelectionAnswer.OrganizationID);
            ViewBag.AssessorID = await AssessorSelectListByModel(token, mySelectionAnswer.AssessorID);
            ViewBag.SelectionQuestionID = await SelectionQuestionSelectListByModel(token, mySelectionAnswer.SelectionQuestionID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, mySelectionAnswer.ProgApplicationID);
            return View(mySelectionAnswer);

        }

        // POST: SelectionAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,SelectionQuestionID,AssessorID,ProgApplicationID,AssignedScore,CreatedDate,isDeleted,TimeStamp,OrganizationID")] SelectionAnswer selectionAnswer)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/SelectionAnswers/{selectionAnswer.ID}", selectionAnswer);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("SelectionAnswers information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("SelectionAnswers information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, selectionAnswer.OrganizationID);
            ViewBag.AssessorID = await AssessorSelectListByModel(token, selectionAnswer.AssessorID);
            ViewBag.SelectionQuestionID = await SelectionQuestionSelectListByModel(token, selectionAnswer.SelectionQuestionID);
            ViewBag.ProgApplicationID = await ProgApplicationSelectListByModel(token, selectionAnswer.ProgApplicationID);
            return View(selectionAnswer);
        }

        // GET: SelectionAnswers/Delete/5
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
            List<SelectionAnswer> selectionAnswer = new List<SelectionAnswer>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionAnswers/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionAnswerResponse = Res.Content.ReadAsStringAsync().Result;
                    SelectionAnswer mySelectionAnswer = JsonConvert.DeserializeObject<SelectionAnswer>(SelectionAnswerResponse);
                    return View(mySelectionAnswer);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionAnswers information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: SelectionAnswers/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/SelectionAnswers/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("SelectionAnswers deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("SelectionAnswers  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
