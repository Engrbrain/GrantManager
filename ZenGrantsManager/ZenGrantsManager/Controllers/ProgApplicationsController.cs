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
    public class ProgApplicationsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProgApplications
        public ActionResult Index()
        {
            var progApplications = db.ProgApplications.Include(p => p.Assessor).Include(p => p.Organization).Include(p => p.Programme);
            return View(progApplications.ToList());
        }

        // GET: ProgApplications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgApplication progApplication = db.ProgApplications.Find(id);
            if (progApplication == null)
            {
                return HttpNotFound();
            }
            return View(progApplication);
        }

        // GET: ProgApplications/Create
        public ActionResult Create()
        {
            ViewBag.AssessorID = new SelectList(db.Assessors, "ID", "AssessorCode");
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProgrammeID = new SelectList(db.Programmes, "ID", "ProgrammeName");
            return View();
        }

        // POST: ProgApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ApplicantReference,ApplicantName,ApplicantPhoneNumber,ApplicantEmailAddress,ApplicationSummary,Proposal,ApplicantPhoto,OrganizationID,CreatedDate,isDeleted,TimeStamp,Applicationscore,ProgrammeID,AssessorID,applicationStatus")] ProgApplication progApplication)
        {
            if (ModelState.IsValid)
            {
                db.ProgApplications.Add(progApplication);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssessorID = new SelectList(db.Assessors, "ID", "AssessorCode", progApplication.AssessorID);
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", progApplication.OrganizationID);
            ViewBag.ProgrammeID = new SelectList(db.Programmes, "ID", "ProgrammeName", progApplication.ProgrammeID);
            return View(progApplication);
        }

        // GET: ProgApplications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgApplication progApplication = db.ProgApplications.Find(id);
            if (progApplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssessorID = new SelectList(db.Assessors, "ID", "AssessorCode", progApplication.AssessorID);
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", progApplication.OrganizationID);
            ViewBag.ProgrammeID = new SelectList(db.Programmes, "ID", "ProgrammeName", progApplication.ProgrammeID);
            return View(progApplication);
        }

        // POST: ProgApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ApplicantReference,ApplicantName,ApplicantPhoneNumber,ApplicantEmailAddress,ApplicationSummary,Proposal,ApplicantPhoto,OrganizationID,CreatedDate,isDeleted,TimeStamp,Applicationscore,ProgrammeID,AssessorID,applicationStatus")] ProgApplication progApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(progApplication).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssessorID = new SelectList(db.Assessors, "ID", "AssessorCode", progApplication.AssessorID);
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", progApplication.OrganizationID);
            ViewBag.ProgrammeID = new SelectList(db.Programmes, "ID", "ProgrammeName", progApplication.ProgrammeID);
            return View(progApplication);
        }

        // GET: ProgApplications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgApplication progApplication = db.ProgApplications.Find(id);
            if (progApplication == null)
            {
                return HttpNotFound();
            }
            return View(progApplication);
        }

        // POST: ProgApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProgApplication progApplication = db.ProgApplications.Find(id);
            db.ProgApplications.Remove(progApplication);
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
