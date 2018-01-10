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
    public class ProjectBudgetsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectBudgets
        public ActionResult Index()
        {
            var projectBudgets = db.ProjectBudgets.Include(p => p.Organization).Include(p => p.Project);
            return View(projectBudgets.ToList());
        }

        // GET: ProjectBudgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectBudget projectBudget = db.ProjectBudgets.Find(id);
            if (projectBudget == null)
            {
                return HttpNotFound();
            }
            return View(projectBudget);
        }

        // GET: ProjectBudgets/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference");
            return View();
        }

        // POST: ProjectBudgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BudgetItem,BudgetItemDesc,PercentageAllocated,BudgetAmount,ItemActual,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectBudget projectBudget)
        {
            if (ModelState.IsValid)
            {
                db.ProjectBudgets.Add(projectBudget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectBudget.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectBudget.ProjectID);
            return View(projectBudget);
        }

        // GET: ProjectBudgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectBudget projectBudget = db.ProjectBudgets.Find(id);
            if (projectBudget == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectBudget.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectBudget.ProjectID);
            return View(projectBudget);
        }

        // POST: ProjectBudgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BudgetItem,BudgetItemDesc,PercentageAllocated,BudgetAmount,ItemActual,ProjectID,CreatedDate,isDeleted,TimeStamp,OrganizationID")] ProjectBudget projectBudget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectBudget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectBudget.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectBudget.ProjectID);
            return View(projectBudget);
        }

        // GET: ProjectBudgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectBudget projectBudget = db.ProjectBudgets.Find(id);
            if (projectBudget == null)
            {
                return HttpNotFound();
            }
            return View(projectBudget);
        }

        // POST: ProjectBudgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectBudget projectBudget = db.ProjectBudgets.Find(id);
            db.ProjectBudgets.Remove(projectBudget);
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
