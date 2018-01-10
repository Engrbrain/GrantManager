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
    public class ProjectCommentsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProjectComments
        public ActionResult Index()
        {
            var projectComments = db.ProjectComments.Include(p => p.Organization).Include(p => p.Project);
            return View(projectComments.ToList());
        }

        // GET: ProjectComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectComment projectComment = db.ProjectComments.Find(id);
            if (projectComment == null)
            {
                return HttpNotFound();
            }
            return View(projectComment);
        }

        // GET: ProjectComments/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference");
            return View();
        }

        // POST: ProjectComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CommentTitle,CommentDescription,TagUser,CommentAttachment,OrganizationID,ProjectID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectComment projectComment)
        {
            if (ModelState.IsValid)
            {
                db.ProjectComments.Add(projectComment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectComment.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectComment.ProjectID);
            return View(projectComment);
        }

        // GET: ProjectComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectComment projectComment = db.ProjectComments.Find(id);
            if (projectComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectComment.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectComment.ProjectID);
            return View(projectComment);
        }

        // POST: ProjectComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CommentTitle,CommentDescription,TagUser,CommentAttachment,OrganizationID,ProjectID,CreatedDate,isDeleted,TimeStamp,UserId")] ProjectComment projectComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", projectComment.OrganizationID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectReference", projectComment.ProjectID);
            return View(projectComment);
        }

        // GET: ProjectComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectComment projectComment = db.ProjectComments.Find(id);
            if (projectComment == null)
            {
                return HttpNotFound();
            }
            return View(projectComment);
        }

        // POST: ProjectComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectComment projectComment = db.ProjectComments.Find(id);
            db.ProjectComments.Remove(projectComment);
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
