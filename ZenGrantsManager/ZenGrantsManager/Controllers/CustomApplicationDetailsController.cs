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
    public class CustomApplicationDetailsController : Controller
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();

        // GET: CustomApplicationDetails
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion

            List<CustomApplicationDetails> customApplicationDetails = new List<CustomApplicationDetails>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("api/CustomApplicationDetails");
                if (Res.IsSuccessStatusCode)
                {
                    var customApplicationDetailsResponse = Res.Content.ReadAsStringAsync().Result;
                    customApplicationDetails = JsonConvert.DeserializeObject<List<CustomApplicationDetails>>(customApplicationDetailsResponse);
                    return View(customApplicationDetails);
                }
                else
                {
                    this.AddNotification("Custom Application Details could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                    return View(budgetTemplate);
                }

            }
        }

        // GET: CustomApplicationDetails/Details/5
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
            List<CustomApplicationDetails> customApplicationDetails = new List<CustomApplicationDetails>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/CustomApplicationDetails/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var customApplicationDetailsResponse = Res.Content.ReadAsStringAsync().Result;
                    CustomApplicationDetails myCustomApplicationDetails = JsonConvert.DeserializeObject<CustomApplicationDetails>(customApplicationDetailsResponse);
                    return View(myCustomApplicationDetails);
                }
                else
                {
                    this.AddNotification("Unable to display Custom Application Details information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // GET: CustomApplicationDetails/Create
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            //Get Organization to select List
            List<Organization> organization = new List<Organization>();
            List<ProgApplication> progapplication = new List<ProgApplication>();
            List<Programme> programme = new List<Programme>();
            List<ProposalTemplate> proposalTemplate = new List<ProposalTemplate>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetOrgSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName");
                }


            }

            // Get Programme Application Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgApplicationSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "ApplicantName");
                }


            }

            // Get Programme  Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgrammeSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "ProgrammeName");
                }


            }

            // Get Proposal Template  Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProposalTemplateSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "FieldLabel");
                }


            }
            return View();
        }

        // POST: CustomApplicationDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ProposalTemplateID,FieldUserInput,ProgApplicationID,ProgrammeID,OrganizationID,CreatedDate,isDeleted,TimeStamp")] CustomApplicationDetails customApplicationDetails)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            if (ModelState.IsValid)
            {
                customApplicationDetails.CreatedDate = DateTime.Now;
                customApplicationDetails.isDeleted = false;
                customApplicationDetails.TimeStamp = DateTime.Now;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/CustomApplicationDetails", customApplicationDetails);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Custom Application Details created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Custom Application Details cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }

                }
            }
            List<Organization> organization = new List<Organization>();
            List<ProgApplication> progapplication = new List<ProgApplication>();
            List<Programme> programme = new List<Programme>();
            List<ProposalTemplate> proposalTemplate = new List<ProposalTemplate>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetOrgSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", customApplicationDetails.OrganizationID);

                }


            }

            // Get Programme Application Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgApplicationSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "ApplicantName", customApplicationDetails.ProgApplicationID);
                }


            }

            // Get Programme  Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgrammeSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "ProgrammeName", customApplicationDetails.ProgrammeID);
                }


            }

            // Get Proposal Template  Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProposalTemplateSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "FieldLabel", customApplicationDetails.ProposalTemplateID);
                }


            }

            return View(customApplicationDetails);
        }

        // GET: CustomApplicationDetails/Edit/5
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
            List<CustomApplicationDetails> customApplicationDetails = new List<CustomApplicationDetails>();
            CustomApplicationDetails myCustomApplicationDetails = new CustomApplicationDetails();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/CustomApplicationDetails/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var customApplicationDetailsResponse = Res.Content.ReadAsStringAsync().Result;
                    myCustomApplicationDetails = JsonConvert.DeserializeObject<CustomApplicationDetails>(customApplicationDetailsResponse);
                }
                else
                {
                    this.AddNotification("Unable to display Custom Application Details information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }

            List<Organization> organization = new List<Organization>();
            List<ProgApplication> progapplication = new List<ProgApplication>();
            List<Programme> programme = new List<Programme>();
            List<ProposalTemplate> proposalTemplate = new List<ProposalTemplate>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetOrgSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", myCustomApplicationDetails.OrganizationID);

                }


            }
            // Get Programme Application Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgApplicationSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "ApplicantName", myCustomApplicationDetails.ProgApplicationID);
                }


            }

            // Get Programme  Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgrammeSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "ProgrammeName", myCustomApplicationDetails.ProgrammeID);
                }


            }

            // Get Proposal Template  Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProposalTemplateSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "FieldLabel", myCustomApplicationDetails.ProposalTemplateID);
                }


            }

            return View(myCustomApplicationDetails);
        }

        // POST: CustomApplicationDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ProposalTemplateID,FieldUserInput,ProgApplicationID,ProgrammeID,OrganizationID,CreatedDate,isDeleted,TimeStamp")] CustomApplicationDetails customApplicationDetails)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/CustomApplicationDetails/{assessor.ID}", assessor);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Custom Application Details information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Custom Application Details information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }

                }
            }

            List<Organization> organization = new List<Organization>();
            List<ProgApplication> progapplication = new List<ProgApplication>();
            List<Programme> programme = new List<Programme>();
            List<ProposalTemplate> proposalTemplate = new List<ProposalTemplate>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetOrgSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;
                    organization = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);
                    ViewBag.OrganizationID = new SelectList(organization, "ID", "OrgName", customApplicationDetails.OrganizationID);

                }


            }
            // Get Programme Application Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgApplicationSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "ApplicantName", customApplicationDetails.ProgApplicationID);
                }


            }

            // Get Programme  Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgrammeSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "ProgrammeName", customApplicationDetails.ProgrammeID);
                }


            }

            // Get Proposal Template  Select List
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProposalTemplateSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProAppResponse = Res.Content.ReadAsStringAsync().Result;
                    progapplication = JsonConvert.DeserializeObject<List<ProgApplication>>(ProAppResponse);
                    ViewBag.ProjectActivityID = new SelectList(progapplication, "ID", "FieldLabel", customApplicationDetails.ProposalTemplateID);
                }


            }
            return View(customApplicationDetails);
        }

        // GET: CustomApplicationDetails/Delete/5
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
            List<CustomApplicationDetails> customApplicationDetails = new List<CustomApplicationDetails>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/CustomApplicationDetails/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var customApplicationDetailsResponse = Res.Content.ReadAsStringAsync().Result;
                    CustomApplicationDetails myCustomApplicationDetails = JsonConvert.DeserializeObject<CustomApplicationDetails>(customApplicationDetailsResponse);
                    return View(myCustomApplicationDetails);
                }
                else
                {
                    this.AddNotification("Unable to display Custom Application Details information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: CustomApplicationDetails/Delete/5
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/CustomApplicationDetails/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Custom Application Details deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Custom Application Details  cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }
    }
}
