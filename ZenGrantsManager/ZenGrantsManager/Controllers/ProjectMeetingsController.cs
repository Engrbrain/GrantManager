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
    public class ProjectMeetingsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectMeetings
        public ActionResult Index()
        {
            var projectMeetings = db.ProjectMeetings.Include(p => p.Organization).Include(p => p.Project);
            return View(projectMeetings.ToList());
        }

        // GET: ProjectMeetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMeeting projectMeeting = db.ProjectMeetings.Find(id);
            if (projectMeeting == null)
            {
                return HttpNotFound();
            }
            return View(projectMeeting);
        }

        // GET: ProjectMeetings/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference");
            return View();
        }

        // POST: ProjectMeetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MeetingTitle,MeetingDetails,MeetingPurpose,MeetingMedium,DiscussionSummary,MeetingMinutes,MeetingStatus,ProjectID,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectMeeting projectMeeting)
        {
            if (ModelState.IsValid)
            {
                db.ProjectMeetings.Add(projectMeeting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectMeeting.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectMeeting.ProjectID);
            return View(projectMeeting);
        }

        // GET: ProjectMeetings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMeeting projectMeeting = db.ProjectMeetings.Find(id);
            if (projectMeeting == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectMeeting.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectMeeting.ProjectID);
            return View(projectMeeting);
        }

        // POST: ProjectMeetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MeetingTitle,MeetingDetails,MeetingPurpose,MeetingMedium,DiscussionSummary,MeetingMinutes,MeetingStatus,ProjectID,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectMeeting projectMeeting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectMeeting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectMeeting.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectMeeting.ProjectID);
            return View(projectMeeting);
        }

        // GET: ProjectMeetings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMeeting projectMeeting = db.ProjectMeetings.Find(id);
            if (projectMeeting == null)
            {
                return HttpNotFound();
            }
            return View(projectMeeting);
        }

        // POST: ProjectMeetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectMeeting projectMeeting = db.ProjectMeetings.Find(id);
            db.ProjectMeetings.Remove(projectMeeting);
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
