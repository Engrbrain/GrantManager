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
    public class ProjectDocumentsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectDocuments
        public ActionResult Index()
        {
            var projectDocuments = db.ProjectDocuments.Include(p => p.Organization).Include(p => p.Project);
            return View(projectDocuments.ToList());
        }

        // GET: ProjectDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDocument projectDocument = db.ProjectDocuments.Find(id);
            if (projectDocument == null)
            {
                return HttpNotFound();
            }
            return View(projectDocument);
        }

        // GET: ProjectDocuments/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference");
            return View();
        }

        // POST: ProjectDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DocumentName,DocumentDescription,DocumentFile,OrganizationID,ProjectID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectDocument projectDocument)
        {
            if (ModelState.IsValid)
            {
                db.ProjectDocuments.Add(projectDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectDocument.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectDocument.ProjectID);
            return View(projectDocument);
        }

        // GET: ProjectDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDocument projectDocument = db.ProjectDocuments.Find(id);
            if (projectDocument == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectDocument.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectDocument.ProjectID);
            return View(projectDocument);
        }

        // POST: ProjectDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DocumentName,DocumentDescription,DocumentFile,OrganizationID,ProjectID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectDocument projectDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectDocument.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectDocument.ProjectID);
            return View(projectDocument);
        }

        // GET: ProjectDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDocument projectDocument = db.ProjectDocuments.Find(id);
            if (projectDocument == null)
            {
                return HttpNotFound();
            }
            return View(projectDocument);
        }

        // POST: ProjectDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectDocument projectDocument = db.ProjectDocuments.Find(id);
            db.ProjectDocuments.Remove(projectDocument);
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
