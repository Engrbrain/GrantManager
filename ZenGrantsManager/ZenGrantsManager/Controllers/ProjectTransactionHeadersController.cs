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
    public class ProjectTransactionHeadersController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectTransactionHeaders
        public ActionResult Index()
        {
            var projectTransactionHeaders = db.ProjectTransactionHeaders.Include(p => p.Organization).Include(p => p.Project).Include(p => p.ProjectBudget);
            return View(projectTransactionHeaders.ToList());
        }

        // GET: ProjectTransactionHeaders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTransactionHeader projectTransactionHeader = db.ProjectTransactionHeaders.Find(id);
            if (projectTransactionHeader == null)
            {
                return HttpNotFound();
            }
            return View(projectTransactionHeader);
        }

        // GET: ProjectTransactionHeaders/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference");
            ViewBag.ProjectBudgetID = new SelectList(db.ProjectBudgets, "ID", "BudgetItem");
            return View();
        }

        // POST: ProjectTransactionHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectBudgetID,TransactionRef,ShortText,FiscalYear,Period,TransactionDate,EntryDate,TotalAmount,CreatedDate,isDeleted,TimeStamp,OrganizationID,ProjectID,UserId")] ProjectTransactionHeader projectTransactionHeader)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTransactionHeaders.Add(projectTransactionHeader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTransactionHeader.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectTransactionHeader.ProjectID);
            ViewBag.ProjectBudgetID = new SelectList(db.ProjectBudgets, "ID", "BudgetItem", projectTransactionHeader.ProjectBudgetID);
            return View(projectTransactionHeader);
        }

        // GET: ProjectTransactionHeaders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTransactionHeader projectTransactionHeader = db.ProjectTransactionHeaders.Find(id);
            if (projectTransactionHeader == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTransactionHeader.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectTransactionHeader.ProjectID);
            ViewBag.ProjectBudgetID = new SelectList(db.ProjectBudgets, "ID", "BudgetItem", projectTransactionHeader.ProjectBudgetID);
            return View(projectTransactionHeader);
        }

        // POST: ProjectTransactionHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectBudgetID,TransactionRef,ShortText,FiscalYear,Period,TransactionDate,EntryDate,TotalAmount,CreatedDate,isDeleted,TimeStamp,OrganizationID,ProjectID,UserId")] ProjectTransactionHeader projectTransactionHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTransactionHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectTransactionHeader.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectTransactionHeader.ProjectID);
            ViewBag.ProjectBudgetID = new SelectList(db.ProjectBudgets, "ID", "BudgetItem", projectTransactionHeader.ProjectBudgetID);
            return View(projectTransactionHeader);
        }

        // GET: ProjectTransactionHeaders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTransactionHeader projectTransactionHeader = db.ProjectTransactionHeaders.Find(id);
            if (projectTransactionHeader == null)
            {
                return HttpNotFound();
            }
            return View(projectTransactionHeader);
        }

        // POST: ProjectTransactionHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectTransactionHeader projectTransactionHeader = db.ProjectTransactionHeaders.Find(id);
            db.ProjectTransactionHeaders.Remove(projectTransactionHeader);
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
