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
    public class ProposalTemplatesController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: ProposalTemplates
        public ActionResult Index()
        {
            var proposalTemplates = db.ProposalTemplates.Include(p => p.Organization);
            return View(proposalTemplates.ToList());
        }

        // GET: ProposalTemplates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProposalTemplate proposalTemplate = db.ProposalTemplates.Find(id);
            if (proposalTemplate == null)
            {
                return HttpNotFound();
            }
            return View(proposalTemplate);
        }

        // GET: ProposalTemplates/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            return View();
        }

        // POST: ProposalTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FieldLabel,FieldType,FieldValue,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProposalTemplate proposalTemplate)
        {
            if (ModelState.IsValid)
            {
                db.ProposalTemplates.Add(proposalTemplate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", proposalTemplate.OrganizationID);
            return View(proposalTemplate);
        }

        // GET: ProposalTemplates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProposalTemplate proposalTemplate = db.ProposalTemplates.Find(id);
            if (proposalTemplate == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", proposalTemplate.OrganizationID);
            return View(proposalTemplate);
        }

        // POST: ProposalTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FieldLabel,FieldType,FieldValue,OrganizationID,CreatedDate,isDeleted,TimeStamp,UserId")] ProposalTemplate proposalTemplate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proposalTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", proposalTemplate.OrganizationID);
            return View(proposalTemplate);
        }

        // GET: ProposalTemplates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProposalTemplate proposalTemplate = db.ProposalTemplates.Find(id);
            if (proposalTemplate == null)
            {
                return HttpNotFound();
            }
            return View(proposalTemplate);
        }

        // POST: ProposalTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProposalTemplate proposalTemplate = db.ProposalTemplates.Find(id);
            db.ProposalTemplates.Remove(proposalTemplate);
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
