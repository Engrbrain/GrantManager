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
    public class SelectionAnswersController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: SelectionAnswers
        public ActionResult Index()
        {
            var selectionAnswers = db.SelectionAnswers.Include(s => s.Organization).Include(s => s.ProgApplication).Include(s => s.SelectionQuestion);
            return View(selectionAnswers.ToList());
        }

        // GET: SelectionAnswers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionAnswer selectionAnswer = db.SelectionAnswers.Find(id);
            if (selectionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(selectionAnswer);
        }

        // GET: SelectionAnswers/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProgApplicationID = new SelectList(db.ProgApplications, "ID", "ApplicantReference");
            ViewBag.SelectionQuestionID = new SelectList(db.SelectionQuestions, "ID", "Question");
            return View();
        }

        // POST: SelectionAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SelectionQuestionID,AssesorID,ProgApplicationID,AssignedScore,CreatedDate,isDeleted,TimeStamp,OrganizationID")] SelectionAnswer selectionAnswer)
        {
            if (ModelState.IsValid)
            {
                db.SelectionAnswers.Add(selectionAnswer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionAnswer.OrganizationID);
            ViewBag.ProgApplicationID = new SelectList(db.ProgApplications, "ID", "ApplicantReference", selectionAnswer.ProgApplicationID);
            ViewBag.SelectionQuestionID = new SelectList(db.SelectionQuestions, "ID", "Question", selectionAnswer.SelectionQuestionID);
            return View(selectionAnswer);
        }

        // GET: SelectionAnswers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionAnswer selectionAnswer = db.SelectionAnswers.Find(id);
            if (selectionAnswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionAnswer.OrganizationID);
            ViewBag.ProgApplicationID = new SelectList(db.ProgApplications, "ID", "ApplicantReference", selectionAnswer.ProgApplicationID);
            ViewBag.SelectionQuestionID = new SelectList(db.SelectionQuestions, "ID", "Question", selectionAnswer.SelectionQuestionID);
            return View(selectionAnswer);
        }

        // POST: SelectionAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SelectionQuestionID,AssesorID,ProgApplicationID,AssignedScore,CreatedDate,isDeleted,TimeStamp,OrganizationID")] SelectionAnswer selectionAnswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(selectionAnswer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", selectionAnswer.OrganizationID);
            ViewBag.ProgApplicationID = new SelectList(db.ProgApplications, "ID", "ApplicantReference", selectionAnswer.ProgApplicationID);
            ViewBag.SelectionQuestionID = new SelectList(db.SelectionQuestions, "ID", "Question", selectionAnswer.SelectionQuestionID);
            return View(selectionAnswer);
        }

        // GET: SelectionAnswers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectionAnswer selectionAnswer = db.SelectionAnswers.Find(id);
            if (selectionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(selectionAnswer);
        }

        // POST: SelectionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SelectionAnswer selectionAnswer = db.SelectionAnswers.Find(id);
            db.SelectionAnswers.Remove(selectionAnswer);
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
