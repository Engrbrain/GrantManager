using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZenGrantsManager.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZenGrantsManager.Extensions;
using System.IO;

namespace ZenGrantsManager.Controllers
{
    

    public class OrganizationsController : Controller
    {
        public string token = String.Empty;
        public string userID = String.Empty;
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();
        // GET: Organizations
        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
            List<Organization> organization = new List<Organization>();
         

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                    HttpResponseMessage Res = await client.GetAsync("api/Organizations");
                    if (Res.IsSuccessStatusCode)
                    {  
                        var orgResponse = Res.Content.ReadAsStringAsync().Result;  
                        organization = JsonConvert.DeserializeObject<List<Organization>>(orgResponse);
                        return View(organization);
                    }
                    else
                    {
                         this.AddNotification("Organization List could not be displayed at this time, Please contact administrator" + Res, NotificationType.ERROR);
                        return View(organization);
                    }
                
                }
        }
        // GET: Organizations/Details/5
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
            
            List<Organization> organization = new List<Organization>();
           

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Organizations/{id}"); 
                if (Res.IsSuccessStatusCode)
                {  
                    var orgResponse = Res.Content.ReadAsStringAsync().Result;
                    Organization myorganization = JsonConvert.DeserializeObject<Organization>(orgResponse);
                    return View(myorganization);
                }
                else {
                    this.AddNotification("Unable to display organization information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }
                
            }
            
        }

        // GET: Organizations/Create
        [HttpGet]
        [SessionTimeout]
        public ActionResult Create()
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);
            #endregion
           return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,OrgName,OrgAddress,OrgState,OrgCountry,OrgPhone,OrgEmail,OrgWebsite,CreatedDate,isDeleted,TimeStamp,OrgLogo,UserID")] Organization organization)
        {
            #region USERVALIDATION
            token = (string)(Session["accessToken"]);
            string userID = (string)(Session["UserID"]);

            #endregion
            if (ModelState.IsValid)
            {
                organization.UserId = userID;
                organization.CreatedDate = DateTime.Now;
                organization.isDeleted = false;
                //Prepare uploaded Image
                HttpPostedFileBase file = Request.Files["ImageData"];
                organization.OrgLogo = ConvertToBytes(file);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
                    HttpResponseMessage Res = await client.PostAsJsonAsync("api/Organizations", organization);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Organization record created successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else { this.AddNotification("Organization cannot be created at this time. Please contact Administrator" + Res, NotificationType.ERROR);
                        return View();
                    }
                    
                }
            }

            return View(organization);
        }

        // GET: Organizations/Edit/5
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
            
            List<Organization> organization = new List<Organization>();
            

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Organizations/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var orgResponse = Res.Content.ReadAsStringAsync().Result;
                    Organization myorganization = JsonConvert.DeserializeObject<Organization>(orgResponse);
                    return View(myorganization);
                }
                else
                {
                    this.AddNotification("Unable to display organization information,please contact Administrator" + Res, NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> Edit([Bind(Include = "ID,OrgName,OrgAddress,OrgState,OrgCountry,OrgPhone,OrgEmail,OrgWebsite,CreatedDate,isDeleted,TimeStamp,OrgLogo")] Organization organization)
        {

            
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["ImageData"];
                organization.OrgLogo = ConvertToBytes(file);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.PutAsJsonAsync($"api/Organizations/{organization.ID}", organization);
                    if (Res.IsSuccessStatusCode)
                    {
                        this.AddNotification("Organization information modified successfully", NotificationType.SUCCESS);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        this.AddNotification("Organization information cannot be modified at this time. Please contact Administrator", NotificationType.ERROR);
                        return View();
                    }
                    //returning the employee list to view  

                }
            }
            return View(organization);
        }

        // GET: Organizations/Delete/5
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
            List<Organization> organization = new List<Organization>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync($"api/Organizations/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    var orgResponse = Res.Content.ReadAsStringAsync().Result;
                    Organization myorganization = JsonConvert.DeserializeObject<Organization>(orgResponse);
                    return View(myorganization);
                }
                else
                {
                    this.AddNotification("Unable to display organization information,please contact Administrator", NotificationType.ERROR);
                    return View();
                }

            }
        }

        // POST: Organizations/Delete/5
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> DeleteConfirmed(int id)
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
                HttpResponseMessage Res = await client.DeleteAsync($"api/Organizations/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    this.AddNotification("Organization record deleted successfully", NotificationType.SUCCESS);
                    return RedirectToAction("Index");

                }
                else
                {
                    this.AddNotification("Organization record cannot be deleted at this time. Please contact Administrator" + Res, NotificationType.ERROR);
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

            {return File(Logo, "image/jpg");}

            else {   return null;}

        }

        public async Task<byte[]>  GetImageFromDataBase(int Id)

        {
            List<Organization> organization = new List<Organization>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res =  await client.GetAsync($"api/Organizations/{Id}");
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
