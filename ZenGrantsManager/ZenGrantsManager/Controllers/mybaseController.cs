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

    public class mybaseController : Controller
    {
        string baseurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"].ToString();
        List<ProjectActivity> projectactivity = new List<ProjectActivity>();
        List<Organization> organization = new List<Organization>();
        List<ProgApplication> progapplication = new List<ProgApplication>();
        List<ProposalTemplate> proposalTemplate = new List<ProposalTemplate>();
        List<Programme> programme = new List<Programme>();
        List<Project> project = new List<Project>();
        List<ProjectMeeting> projectMeeting = new List<ProjectMeeting>();
        List<Assessor> assessor = new List<Assessor>();

        public async Task<SelectList> OrganizationSelectList(string token)
        {
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
                    return new SelectList(organization, "ID", "OrgName");
                }
                return new SelectList(organization, "ID", "OrgName");
            }
        }

        public async Task<SelectList> OrganizationSelectListByModel(string token, object selectedValue)
        {
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
                    return new SelectList(organization, "ID", "OrgName", selectedValue);

                }

                return new SelectList(organization, "ID", "OrgName", selectedValue);
            }
        }

        public async Task<SelectList> ProjectActivitySelectList(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProActSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProActResponse = Res.Content.ReadAsStringAsync().Result;
                    projectactivity = JsonConvert.DeserializeObject<List<ProjectActivity>>(ProActResponse);
                    return new SelectList(projectactivity, "ID", "ActivityTitle");
                }
                return new SelectList(projectactivity, "ID", "ActivityTitle");

            }
        }

        public async Task<SelectList> ProjectActivitySelectListByModel(string token, object selectedValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProActSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProActResponse = Res.Content.ReadAsStringAsync().Result;
                    projectactivity = JsonConvert.DeserializeObject<List<ProjectActivity>>(ProActResponse);
                    return new SelectList(projectactivity, "ID", "ActivityTitle", selectedValue);
                }
                return new SelectList(projectactivity, "ID", "ActivityTitle", selectedValue);

            }

        }

        public async Task<SelectList> ProgApplicationSelectList(string token)
        {
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
                    return new SelectList(progapplication, "ID", "ApplicantName");
                }
                return new SelectList(progapplication, "ID", "ApplicantName");
            }
        }
        public async Task<SelectList> ProgApplicationSelectListByModel(string token, object selectedValue)
        {
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
                    return new SelectList(progapplication, "ID", "ApplicantName", selectedValue);
                }
                return new SelectList(progapplication, "ID", "ApplicantName", selectedValue);
            }



        }
        public async Task<SelectList> ProgrammeSelectList(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgrammeSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var programmeResponse = Res.Content.ReadAsStringAsync().Result;
                    programme = JsonConvert.DeserializeObject<List<Programme>>(programmeResponse);
                    return new SelectList(programme, "ID", "ProgrammeName");
                }
                return new SelectList(programme, "ID", "ProgrammeName");
            }
        }
        public async Task<SelectList> ProgrammeSelectListByModel(string token, object selectedValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProgrammeSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var programmeResponse = Res.Content.ReadAsStringAsync().Result;
                    programme = JsonConvert.DeserializeObject<List<Programme>>(programmeResponse);
                    return new SelectList(programme, "ID", "ProgrammeName", selectedValue);
                }
                return new SelectList(programme, "ID", "ProgrammeName", selectedValue);
            }
        }

        public async Task<SelectList> ProposalTemplateSelectList(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProposalTemplateSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProposalTemplateResponse = Res.Content.ReadAsStringAsync().Result;
                    proposalTemplate = JsonConvert.DeserializeObject<List<ProposalTemplate>>(ProposalTemplateResponse);
                    return new SelectList(proposalTemplate, "ID", "FieldLabel");
                }
                return new SelectList(proposalTemplate, "ID", "FieldLabel");
            }
        }
        public async Task<SelectList> ProposalTemplateSelectListByModel(string token, object selectedValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProposalTemplateSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProposalTemplateResponse = Res.Content.ReadAsStringAsync().Result;
                    proposalTemplate = JsonConvert.DeserializeObject<List<ProposalTemplate>>(ProposalTemplateResponse);
                    return new SelectList(proposalTemplate, "ID", "FieldLabel", selectedValue);
                }
                return new SelectList(proposalTemplate, "ID", "FieldLabel", selectedValue);
            }
        }

        public async Task<SelectList> ProjectSelectList(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProjectSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProjectResponse = Res.Content.ReadAsStringAsync().Result;
                    project = JsonConvert.DeserializeObject<List<Project>>(ProjectResponse);
                    return new SelectList(project, "ID", "ProjectName");
                }
                return new SelectList(project, "ID", "ProjectName");
            }
        }
        public async Task<SelectList> ProjectSelectListByModel(string token, object selectedValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProjectSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProjectResponse = Res.Content.ReadAsStringAsync().Result;
                    project = JsonConvert.DeserializeObject<List<Project>>(ProjectResponse);
                    return new SelectList(project, "ID", "ProjectName", selectedValue);
                }
                return new SelectList(project, "ID", "ProjectName", selectedValue);
            }
        }

        public async Task<SelectList> ProjectMeetingSelectList(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProjectMeetingSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProjectMeetingResponse = Res.Content.ReadAsStringAsync().Result;
                    projectMeeting = JsonConvert.DeserializeObject<List<ProjectMeeting>>(ProjectMeetingResponse);
                    return new SelectList(projectMeeting, "ID", "MeetingTitle");
                }
                return new SelectList(projectMeeting, "ID", "MeetingTitle");
            }
        }
        public async Task<SelectList> ProjectMeetingSelectListByModel(string token, object selectedValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetProjectMeetingSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var ProjectMeetingResponse = Res.Content.ReadAsStringAsync().Result;
                    projectMeeting = JsonConvert.DeserializeObject<List<ProjectMeeting>>(ProjectMeetingResponse);
                    return new SelectList(projectMeeting, "ID", "MeetingTitle", selectedValue);
                }
                return new SelectList(projectMeeting, "ID", "MeetingTitle", selectedValue);
            }
        }

        public async Task<SelectList> AssessorSelectList(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetAssessorSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var AssessorResponse = Res.Content.ReadAsStringAsync().Result;
                    assessor = JsonConvert.DeserializeObject<List<Assessor>>(AssessorResponse);
                    return new SelectList(assessor, "ID", "AssessorName");
                }
                return new SelectList(assessor, "ID", "AssessorName");
            }
        }
        public async Task<SelectList> AssessorSelectListByModel(string token, object selectedValue)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage Res = await client.GetAsync("GetAssessorSelectList");
                if (Res.IsSuccessStatusCode)
                {
                    var AssessorResponse = Res.Content.ReadAsStringAsync().Result;
                    assessor = JsonConvert.DeserializeObject<List<Assessor>>(AssessorResponse);
                    return new SelectList(assessor, "ID", "AssessorName", selectedValue);
                }
                return new SelectList(assessor, "ID", "AssessorName", selectedValue);
            }
        }

    }
    }