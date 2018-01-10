﻿using System;
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
    public class ProgrammesController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: Programmes
        public ActionResult Index()
        {
            var programmes = db.Programmes.Include(p => p.Organization).Include(p => p.ProposalTemplate);
            return View(programmes.ToList());
        }

        // GET: Programmes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programmes.Find(id);
            if (programme == null)
            {
                return HttpNotFound();
            }
            return View(programme);
        }

        // GET: Programmes/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.ProposalTemplateID = new SelectList(db.ProposalTemplates, "ID", "FieldLabel");
            return View();
        }

        // POST: Programmes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProgrammeName,ProgrammeCode,ProgrammeDesc,ApplicationStartDate,ApplicationDueDate,ProposalTemplateID,OrganizationID,MaximumApplication,CreatedDate,isDeleted,TimeStamp,ProgrammeLogo,UserId")] Programme programme)
        {
            if (ModelState.IsValid)
            {
                db.Programmes.Add(programme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", programme.OrganizationID);
            ViewBag.ProposalTemplateID = new SelectList(db.ProposalTemplates, "ID", "FieldLabel", programme.ProposalTemplateID);
            return View(programme);
        }

        // GET: Programmes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programmes.Find(id);
            if (programme == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", programme.OrganizationID);
            ViewBag.ProposalTemplateID = new SelectList(db.ProposalTemplates, "ID", "FieldLabel", programme.ProposalTemplateID);
            return View(programme);
        }

        // POST: Programmes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProgrammeName,ProgrammeCode,ProgrammeDesc,ApplicationStartDate,ApplicationDueDate,ProposalTemplateID,OrganizationID,MaximumApplication,CreatedDate,isDeleted,TimeStamp,ProgrammeLogo,UserId")] Programme programme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", programme.OrganizationID);
            ViewBag.ProposalTemplateID = new SelectList(db.ProposalTemplates, "ID", "FieldLabel", programme.ProposalTemplateID);
            return View(programme);
        }

        // GET: Programmes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programmes.Find(id);
            if (programme == null)
            {
                return HttpNotFound();
            }
            return View(programme);
        }

        // POST: Programmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programme programme = db.Programmes.Find(id);
            db.Programmes.Remove(programme);
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