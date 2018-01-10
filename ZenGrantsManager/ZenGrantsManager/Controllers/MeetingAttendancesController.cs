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
    public class MeetingAttendancesController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: MeetingAttendances
        public ActionResult Index()
        {
            var meetingAttendances = db.MeetingAttendances.Include(m => m.Organization).Include(m => m.Project).Include(m => m.ProjectMeeting).Include(m => m.ProjectTeam);
            return View(meetingAttendances.ToList());
        }

        // GET: MeetingAttendances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeetingAttendance meetingAttendance = db.MeetingAttendances.Find(id);
            if (meetingAttendance == null)
            {
                return HttpNotFound();
            }
            return View(meetingAttendance);
        }

        // GET: MeetingAttendances/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference");
            ViewBag.ProjectMeetingID = new SelectList(db.ProjectMeetings, "ID", "MeetingTitle");
            ViewBag.ProjectTeamID = new SelectList(db.Projects, "ID", "ProjectReference");
            return View();
        }

        // POST: MeetingAttendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FullName,Designation,EmailAddress,PhoneNumber,AttendanceStatus,ProjectMeetingID,ProjectID,ProjectTeamID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] MeetingAttendance meetingAttendance)
        {
            if (ModelState.IsValid)
            {
                db.MeetingAttendances.Add(meetingAttendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", meetingAttendance.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", meetingAttendance.ProjectID);
            ViewBag.ProjectMeetingID = new SelectList(db.ProjectMeetings, "ID", "MeetingTitle", meetingAttendance.ProjectMeetingID);
            ViewBag.ProjectTeamID = new SelectList(db.Projects, "ID", "ProjectReference", meetingAttendance.ProjectTeamID);
            return View(meetingAttendance);
        }

        // GET: MeetingAttendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeetingAttendance meetingAttendance = db.MeetingAttendances.Find(id);
            if (meetingAttendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", meetingAttendance.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", meetingAttendance.ProjectID);
            ViewBag.ProjectMeetingID = new SelectList(db.ProjectMeetings, "ID", "MeetingTitle", meetingAttendance.ProjectMeetingID);
            ViewBag.ProjectTeamID = new SelectList(db.Projects, "ID", "ProjectReference", meetingAttendance.ProjectTeamID);
            return View(meetingAttendance);
        }

        // POST: MeetingAttendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,Designation,EmailAddress,PhoneNumber,AttendanceStatus,ProjectMeetingID,ProjectID,ProjectTeamID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] MeetingAttendance meetingAttendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meetingAttendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", meetingAttendance.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", meetingAttendance.ProjectID);
            ViewBag.ProjectMeetingID = new SelectList(db.ProjectMeetings, "ID", "MeetingTitle", meetingAttendance.ProjectMeetingID);
            ViewBag.ProjectTeamID = new SelectList(db.Projects, "ID", "ProjectReference", meetingAttendance.ProjectTeamID);
            return View(meetingAttendance);
        }

        // GET: MeetingAttendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeetingAttendance meetingAttendance = db.MeetingAttendances.Find(id);
            if (meetingAttendance == null)
            {
                return HttpNotFound();
            }
            return View(meetingAttendance);
        }

        // POST: MeetingAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MeetingAttendance meetingAttendance = db.MeetingAttendances.Find(id);
            db.MeetingAttendances.Remove(meetingAttendance);
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
