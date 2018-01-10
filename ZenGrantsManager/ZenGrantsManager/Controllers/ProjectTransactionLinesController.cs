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
    public class ProjectTransactionLinesController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectTransactionLines
        public ActionResult Index()
        {
            var projectTransactionLines = db.ProjectTransactionLines.Include(p => p.Organization).Include(p => p.ProjectTransactionHeader);
            return View(projectTransactionLines.ToList());
        }

        // GET: ProjectTransactionLines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTransactionLine projectTransactionLine = db.ProjectTransactionLines.Find(id);
            if (projectTransactionLine == null)
            {
                return HttpNotFound();
            }
            return View(projectTransactionLine);
        }

        // GET: ProjectTransactionLines/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectTransactionHeaderID = new SelectList(db.ProjectTransactionHeaders, "ID", "TransactionRef");
            return View();
        }

        // POST: ProjectTransactionLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectTransactionHeaderID,TransactionRef,LineNumber,Narration,LineAmount,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectTransactionLine projectTransactionLine)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTransactionLines.Add(projectTransactionLine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTransactionLine.OrganizationID);
            ViewBag.ProjectTransactionHeaderID = new SelectList(db.ProjectTransactionHeaders, "ID", "TransactionRef", projectTransactionLine.ProjectTransactionHeaderID);
            return View(projectTransactionLine);
        }

        // GET: ProjectTransactionLines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTransactionLine projectTransactionLine = db.ProjectTransactionLines.Find(id);
            if (projectTransactionLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTransactionLine.OrganizationID);
            ViewBag.ProjectTransactionHeaderID = new SelectList(db.ProjectTransactionHeaders, "ID", "TransactionRef", projectTransactionLine.ProjectTransactionHeaderID);
            return View(projectTransactionLine);
        }

        // POST: ProjectTransactionLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectTransactionHeaderID,TransactionRef,LineNumber,Narration,LineAmount,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectTransactionLine projectTransactionLine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTransactionLine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTransactionLine.OrganizationID);
            ViewBag.ProjectTransactionHeaderID = new SelectList(db.ProjectTransactionHeaders, "ID", "TransactionRef", projectTransactionLine.ProjectTransactionHeaderID);
            return View(projectTransactionLine);
        }

        // GET: ProjectTransactionLines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTransactionLine projectTransactionLine = db.ProjectTransactionLines.Find(id);
            if (projectTransactionLine == null)
            {
                return HttpNotFound();
            }
            return View(projectTransactionLine);
        }

        // POST: ProjectTransactionLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectTransactionLine projectTransactionLine = db.ProjectTransactionLines.Find(id);
            db.ProjectTransactionLines.Remove(projectTransactionLine);
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
