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
    public class ProjectsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: Projects
        public ActionResult Index()
        {
            var projects = db.Projects.Include(p => p.Organization).Include(p => p.ProgApplication).Include(p => p.Programme);
            return View(projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProgApplicationID = new SelectList(db.ProgApplications, "ID", "ApplicantReference");
            ViewBag.ProgrammeID = new SelectList(db.Programmes, "ID", "ProgrammeName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectReference,ProjectName,ProjectDescription,ProgrammeDesc,ProjectStartDate,ProjectDueDate,ProjectContigencyPeriod,AmountAllocated,BalanceAmount,ProjectStatus,OrganizationID,ProgApplicationID,ProgrammeID,CreatedDate,isDeleted,TimeStamp,ProjectLogo")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", project.OrganizationID);
            ViewBag.ProgApplicationID = new SelectList(db.ProgApplications, "ID", "ApplicantReference", project.ProgApplicationID);
            ViewBag.ProgrammeID = new SelectList(db.Programmes, "ID", "ProgrammeName", project.ProgrammeID);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", project.OrganizationID);
            ViewBag.ProgApplicationID = new SelectList(db.ProgApplications, "ID", "ApplicantReference", project.ProgApplicationID);
            ViewBag.ProgrammeID = new SelectList(db.Programmes, "ID", "ProgrammeName", project.ProgrammeID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectReference,ProjectName,ProjectDescription,ProgrammeDesc,ProjectStartDate,ProjectDueDate,ProjectContigencyPeriod,AmountAllocated,BalanceAmount,ProjectStatus,OrganizationID,ProgApplicationID,ProgrammeID,CreatedDate,isDeleted,TimeStamp,ProjectLogo")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", project.OrganizationID);
            ViewBag.ProgApplicationID = new SelectList(db.ProgApplications, "ID", "ApplicantReference", project.ProgApplicationID);
            ViewBag.ProgrammeID = new SelectList(db.Programmes, "ID", "ProgrammeName", project.ProgrammeID);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
