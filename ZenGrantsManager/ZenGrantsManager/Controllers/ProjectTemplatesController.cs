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
    public class ProjectTemplatesController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectTemplates
        public ActionResult Index()
        {
            var projectTemplates = db.ProjectTemplates.Include(p => p.Organization);
            return View(projectTemplates.ToList());
        }

        // GET: ProjectTemplates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTemplate projectTemplate = db.ProjectTemplates.Find(id);
            if (projectTemplate == null)
            {
                return HttpNotFound();
            }
            return View(projectTemplate);
        }

        // GET: ProjectTemplates/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            return View();
        }

        // POST: ProjectTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NumberOfMilestones,ProjectReportFrequency,OrganizationID,CreatedDate,isDeleted,TimeStamp")] ProjectTemplate projectTemplate)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTemplates.Add(projectTemplate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTemplate.OrganizationID);
            return View(projectTemplate);
        }

        // GET: ProjectTemplates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTemplate projectTemplate = db.ProjectTemplates.Find(id);
            if (projectTemplate == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTemplate.OrganizationID);
            return View(projectTemplate);
        }

        // POST: ProjectTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NumberOfMilestones,ProjectReportFrequency,OrganizationID,CreatedDate,isDeleted,TimeStamp")] ProjectTemplate projectTemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTemplate.OrganizationID);
            return View(projectTemplate);
        }

        // GET: ProjectTemplates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTemplate projectTemplate = db.ProjectTemplates.Find(id);
            if (projectTemplate == null)
            {
                return HttpNotFound();
            }
            return View(projectTemplate);
        }

        // POST: ProjectTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectTemplate projectTemplate = db.ProjectTemplates.Find(id);
            db.ProjectTemplates.Remove(projectTemplate);
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
