using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZenGrantsManager.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ZenGrantsManager.Controllers
{
    public class OrganizationController : Controller
    {        //Hosted web API REST Service base url  
        string Baseurl = "http://localhost:49122/";
        public async Task<ActionResult> Index()
        {
            List<Organization> OrgInfo = new List<Organization>();

            using (var client = new HttpClient())
            {
               
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                HttpResponseMessage Res = await client.GetAsync("api/Organizations");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var OrgResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    OrgInfo = JsonConvert.DeserializeObject<List<Organization>>(OrgResponse);

                }
                //returning the employee list to view  
                return View(OrgInfo);
            }
        }
    }
}