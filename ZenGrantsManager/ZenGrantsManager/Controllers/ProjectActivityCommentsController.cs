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
    public class ProjectActivityCommentsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectActivityComments
        public ActionResult Index()
        {
            var projectActivityComments = db.ProjectActivityComments.Include(p => p.Organization).Include(p => p.ProjectActivity);
            return View(projectActivityComments.ToList());
        }

        // GET: ProjectActivityComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectActivityComment projectActivityComment = db.ProjectActivityComments.Find(id);
            if (projectActivityComment == null)
            {
                return HttpNotFound();
            }
            return View(projectActivityComment);
        }

        // GET: ProjectActivityComments/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectActivityID = new SelectList(db.ProjectActivities, "ID", "ActivityTitle");
            return View();
        }

        // POST: ProjectActivityComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CommentTitle,CommentDescription,TagUser,CommentAttachment,ProjectActivityID,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectActivityComment projectActivityComment)
        {
            if (ModelState.IsValid)
            {
                db.ProjectActivityComments.Add(projectActivityComment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectActivityComment.OrganizationID);
            ViewBag.ProjectActivityID = new SelectList(db.ProjectActivities, "ID", "ActivityTitle", projectActivityComment.ProjectActivityID);
            return View(projectActivityComment);
        }

        // GET: ProjectActivityComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectActivityComment projectActivityComment = db.ProjectActivityComments.Find(id);
            if (projectActivityComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectActivityComment.OrganizationID);
            ViewBag.ProjectActivityID = new SelectList(db.ProjectActivities, "ID", "ActivityTitle", projectActivityComment.ProjectActivityID);
            return View(projectActivityComment);
        }

        // POST: ProjectActivityComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CommentTitle,CommentDescription,TagUser,CommentAttachment,ProjectActivityID,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectActivityComment projectActivityComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectActivityComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectActivityComment.OrganizationID);
            ViewBag.ProjectActivityID = new SelectList(db.ProjectActivities, "ID", "ActivityTitle", projectActivityComment.ProjectActivityID);
            return View(projectActivityComment);
        }

        // GET: ProjectActivityComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectActivityComment projectActivityComment = db.ProjectActivityComments.Find(id);
            if (projectActivityComment == null)
            {
                return HttpNotFound();
            }
            return View(projectActivityComment);
        }

        // POST: ProjectActivityComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectActivityComment projectActivityComment = db.ProjectActivityComments.Find(id);
            db.ProjectActivityComments.Remove(projectActivityComment);
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
