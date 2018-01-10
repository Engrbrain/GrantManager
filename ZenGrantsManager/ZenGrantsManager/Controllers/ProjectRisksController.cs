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
    public class ProjectRisksController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectRisks
        public ActionResult Index()
        {
            var projectRisks = db.ProjectRisks.Include(p => p.Organization).Include(p => p.Project);
            return View(projectRisks.ToList());
        }

        // GET: ProjectRisks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRisk projectRisk = db.ProjectRisks.Find(id);
            if (projectRisk == null)
            {
                return HttpNotFound();
            }
            return View(projectRisk);
        }

        // GET: ProjectRisks/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference");
            return View();
        }

        // POST: ProjectRisks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RiskTitle,RiskDescription,RiskMitigation,RiskStatus,RiskDocument,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] ProjectRisk projectRisk)
        {
            if (ModelState.IsValid)
            {
                db.ProjectRisks.Add(projectRisk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectRisk.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectRisk.ProjectID);
            return View(projectRisk);
        }

        // GET: ProjectRisks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRisk projectRisk = db.ProjectRisks.Find(id);
            if (projectRisk == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectRisk.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectRisk.ProjectID);
            return View(projectRisk);
        }

        // POST: ProjectRisks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RiskTitle,RiskDescription,RiskMitigation,RiskStatus,RiskDocument,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] ProjectRisk projectRisk)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectRisk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectRisk.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectRisk.ProjectID);
            return View(projectRisk);
        }

        // GET: ProjectRisks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRisk projectRisk = db.ProjectRisks.Find(id);
            if (projectRisk == null)
            {
                return HttpNotFound();
            }
            return View(projectRisk);
        }

        // POST: ProjectRisks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectRisk projectRisk = db.ProjectRisks.Find(id);
            db.ProjectRisks.Remove(projectRisk);
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
