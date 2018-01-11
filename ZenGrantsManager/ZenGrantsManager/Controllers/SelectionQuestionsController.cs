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
    public class SelectionQuestionsController : mybaseController
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: SelectionQuestions
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<SelectionQuestion> selectionQuestion = new List<SelectionQuestion>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/SelectionQuestions");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionQuestionsResponse = Res.Content.ReadAsStringAsync().Result;
                    selectionQuestion = JsonConvert.DeserializeObject<List<SelectionQuestion>>(SelectionQuestionsResponse);
                    return View(selectionQuestion);
                }
                else
                {
                    this.AddNotification("SelectionQuestions could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(selectionQuestion);
                }

            }
        }

        // GET: SelectionQuestions/Details/5
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
            List<SelectionQuestion> selectionQuestion = new List<SelectionQuestion>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionQuestions/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionQuestionResponse = Res.Content.ReadAsStringAsync().Result;
                    SelectionQuestion mySelectionQuestion = JsonConvert.DeserializeObject<SelectionQuestion>(SelectionQuestionResponse);
                    return View(mySelectionQuestion);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionQuestions information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: SelectionQuestions/Create
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

        // POST: SelectionQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Question,QuestionWeight,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] SelectionQuestion selectionQuestion)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (ModelState.IsValid)
            {
                selectionQuestion.CreatedDate = DateTime.Now;
                selectionQuestion.isDeleted = false;
                selectionQuestion.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/SelectionQuestions", selectionQuestion);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("SelectionQuestion created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("SelectionQuestion cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, selectionQuestion.OrganizationID);
            
            return View(selectionQuestion);
        }

        // GET: SelectionQuestions/Edit/5
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
            List<SelectionQuestion> selectionQuestion = new List<SelectionQuestion>();
            SelectionQuestion mySelectionQuestion = new SelectionQuestion();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionQuestions/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionQuestionResponse = Res.Content.ReadAsStringAsync().Result;
                    mySelectionQuestion = JsonConvert.DeserializeObject<SelectionQuestion>(SelectionQuestionResponse);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionQuestions information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, mySelectionQuestion.OrganizationID);
           
            return View(mySelectionQuestion);

        }

        // POST: SelectionQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Question,QuestionWeight,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] SelectionQuestion selectionQuestion)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/SelectionQuestions/{selectionQuestion.ID}", selectionQuestion);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("SelectionQuestions information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("SelectionQuestions information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            ViewBag.OrganizationID = await OrganizationSelectListByModel(token, selectionQuestion.OrganizationID);
            
            return View(selectionQuestion);
        }

        // GET: SelectionQuestions/Delete/5
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
            List<SelectionQuestion> selectionQuestion = new List<SelectionQuestion>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/SelectionQuestions/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var SelectionQuestionResponse = Res.Content.ReadAsStringAsync().Result;
                    SelectionQuestion mySelectionQuestion = JsonConvert.DeserializeObject<SelectionQuestion>(SelectionQuestionResponse);
                    return View(mySelectionQuestion);
                }
                else
                {
                    this.AddNotification("Unable to display SelectionQuestions information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: SelectionQuestions/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/SelectionQuestions/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("SelectionQuestions deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("SelectionQuestions  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

    }
}
