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
    public class SelectionQuestionsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: SelectionQuestions
        public ActionResult Index()
        {
            var selectionQuestions = db.SelectionQuestions.Include(s => s.Organization);
            return View(selectionQuestions.ToList());
        }

        // GET: SelectionQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionQuestion selectionQuestion = db.SelectionQuestions.Find(id);
            if (selectionQuestion == null)
            {
                return HttpNotFound();
            }
            return View(selectionQuestion);
        }

        // GET: SelectionQuestions/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            return View();
        }

        // POST: SelectionQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Question,QuestionWeight,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] SelectionQuestion selectionQuestion)
        {
            if (ModelState.IsValid)
            {
                db.SelectionQuestions.Add(selectionQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionQuestion.OrganizationID);
            return View(selectionQuestion);
        }

        // GET: SelectionQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionQuestion selectionQuestion = db.SelectionQuestions.Find(id);
            if (selectionQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionQuestion.OrganizationID);
            return View(selectionQuestion);
        }

        // POST: SelectionQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Question,QuestionWeight,CreatedDate,isDeleted,TimeStamp,OrganizationID,UserId")] SelectionQuestion selectionQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(selectionQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionQuestion.OrganizationID);
            return View(selectionQuestion);
        }

        // GET: SelectionQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionQuestion selectionQuestion = db.SelectionQuestions.Find(id);
            if (selectionQuestion == null)
            {
                return HttpNotFound();
            }
            return View(selectionQuestion);
        }

        // POST: SelectionQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SelectionQuestion selectionQuestion = db.SelectionQuestions.Find(id);
            db.SelectionQuestions.Remove(selectionQuestion);
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
