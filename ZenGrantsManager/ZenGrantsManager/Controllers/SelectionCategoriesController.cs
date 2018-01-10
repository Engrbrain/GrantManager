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
    public class SelectionCategoriesController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: SelectionCategories
        public ActionResult Index()
        {
            var selectionCategories = db.SelectionCategories.Include(s => s.Organization);
            return View(selectionCategories.ToList());
        }

        // GET: SelectionCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionCategory selectionCategory = db.SelectionCategories.Find(id);
            if (selectionCategory == null)
            {
                return HttpNotFound();
            }
            return View(selectionCategory);
        }

        // GET: SelectionCategories/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            return View();
        }

        // POST: SelectionCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryName,CategoryDescription,CategoryWeight,OrganizationID,CreatedDate,isDeleted,TimeStamp")] SelectionCategory selectionCategory)
        {
            if (ModelState.IsValid)
            {
                db.SelectionCategories.Add(selectionCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionCategory.OrganizationID);
            return View(selectionCategory);
        }

        // GET: SelectionCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionCategory selectionCategory = db.SelectionCategories.Find(id);
            if (selectionCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionCategory.OrganizationID);
            return View(selectionCategory);
        }

        // POST: SelectionCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CategoryName,CategoryDescription,CategoryWeight,OrganizationID,CreatedDate,isDeleted,TimeStamp")] SelectionCategory selectionCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(selectionCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionCategory.OrganizationID);
            return View(selectionCategory);
        }

        // GET: SelectionCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionCategory selectionCategory = db.SelectionCategories.Find(id);
            if (selectionCategory == null)
            {
                return HttpNotFound();
            }
            return View(selectionCategory);
        }

        // POST: SelectionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SelectionCategory selectionCategory = db.SelectionCategories.Find(id);
            db.SelectionCategories.Remove(selectionCategory);
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
