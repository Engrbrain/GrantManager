using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZenGrantsManager.Models;

namespace ZenGrantsManager.Controllers
{
    public class ProjectActivitiesController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectActivities
        public ActionResult Index()
        {
            var projectActivities = db.ProjectActivities.Include(p => p.Dependency).Include(p => p.Organization).Include(p => p.Project).Include(p => p.ProjectMeeting).Include(p => p.ProjectTeam);
            return View(projectActivities.ToList());
        }

        // GET: ProjectActivities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectActivity projectActivity = db.ProjectActivities.Find(id);
            if (projectActivity == null)
            {
                return HttpNotFound();
            }
            return View(projectActivity);
        }

        // GET: ProjectActivities/Create
        public ActionResult Create()
        {
            ViewBag.DependencyID = new SelectList(db.ProjectActivities, "ID", "ActivityTitle");
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference");
            ViewBag.ProjectMeetingID = new SelectList(db.ProjectMeetings, "ID", "MeetingTitle");
            ViewBag.ProjectTeamID = new SelectList(db.ProjectTeams, "ID", "TeamMemberReference");
            return View();
        }

        // POST: ProjectActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ActivityTitle,ActivityDescription,ActivityDocumentID,PhoneNumber,Address,StartDate,EndDate,Milestone,Priority,DependencyID,ActivityStatus,ProjectMeetingID,ProjectID,ProjectTeamID,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectActivity projectActivity)
        {
            if (ModelState.IsValid)
            {
                db.ProjectActivities.Add(projectActivity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DependencyID = new SelectList(db.ProjectActivities, "ID", "ActivityTitle", projectActivity.DependencyID);
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectActivity.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectActivity.ProjectID);
            ViewBag.ProjectMeetingID = new SelectList(db.ProjectMeetings, "ID", "MeetingTitle", projectActivity.ProjectMeetingID);
            ViewBag.ProjectTeamID = new SelectList(db.ProjectTeams, "ID", "TeamMemberReference", projectActivity.ProjectTeamID);
            return View(projectActivity);
        }

        // GET: ProjectActivities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectActivity projectActivity = db.ProjectActivities.Find(id);
            if (projectActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.DependencyID = new SelectList(db.ProjectActivities, "ID", "ActivityTitle", projectActivity.DependencyID);
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectActivity.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectActivity.ProjectID);
            ViewBag.ProjectMeetingID = new SelectList(db.ProjectMeetings, "ID", "MeetingTitle", projectActivity.ProjectMeetingID);
            ViewBag.ProjectTeamID = new SelectList(db.ProjectTeams, "ID", "TeamMemberReference", projectActivity.ProjectTeamID);
            return View(projectActivity);
        }

        // POST: ProjectActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ActivityTitle,ActivityDescription,ActivityDocumentID,PhoneNumber,Address,StartDate,EndDate,Milestone,Priority,DependencyID,ActivityStatus,ProjectMeetingID,ProjectID,ProjectTeamID,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectActivity projectActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DependencyID = new SelectList(db.ProjectActivities, "ID", "ActivityTitle", projectActivity.DependencyID);
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectActivity.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectActivity.ProjectID);
            ViewBag.ProjectMeetingID = new SelectList(db.ProjectMeetings, "ID", "MeetingTitle", projectActivity.ProjectMeetingID);
            ViewBag.ProjectTeamID = new SelectList(db.ProjectTeams, "ID", "TeamMemberReference", projectActivity.ProjectTeamID);
            return View(projectActivity);
        }

        // GET: ProjectActivities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectActivity projectActivity = db.ProjectActivities.Find(id);
            if (projectActivity == null)
            {
                return HttpNotFound();
            }
            return View(projectActivity);
        }

        // POST: ProjectActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectActivity projectActivity = db.ProjectActivities.Find(id);
            db.ProjectActivities.Remove(projectActivity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
