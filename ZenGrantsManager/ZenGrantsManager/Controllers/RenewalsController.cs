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
    public class RenewalsController : Controller
    {
        private ZenGrantsManagerContext db = new ZenGrantsManagerContext();

        // GET: Renewals
        public ActionResult Index()
        {
            var renewals = db.Renewals.Include(r => r.Organization).Include(r => r.Subscription);
            return View(renewals.ToList());
        }

        // GET: Renewals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renewal renewal = db.Renewals.Find(id);
            if (renewal == null)
            {
                return HttpNotFound();
            }
            return View(renewal);
        }

        // GET: Renewals/Create
        public ActionResult Create()
        {
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName");
            ViewBag.SubscriptionID = new SelectList(db.Subscriptions, "ID", "UserId");
            return View();
        }

        // POST: Renewals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrganizationID,SubscriptionID,InvoiceNumber,PaymentReference,Status,PaymentMethod,Narration,PostingDate,ExpiryDate,RenewalAmount,isActive,TimeStamp,UserId")] Renewal renewal)
        {
            if (ModelState.IsValid)
            {
                db.Renewals.Add(renewal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", renewal.OrganizationID);
            ViewBag.SubscriptionID = new SelectList(db.Subscriptions, "ID", "UserId", renewal.SubscriptionID);
            return View(renewal);
        }

        // GET: Renewals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renewal renewal = db.Renewals.Find(id);
            if (renewal == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", renewal.OrganizationID);
            ViewBag.SubscriptionID = new SelectList(db.Subscriptions, "ID", "UserId", renewal.SubscriptionID);
            return View(renewal);
        }

        // POST: Renewals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrganizationID,SubscriptionID,InvoiceNumber,PaymentReference,Status,PaymentMethod,Narration,PostingDate,ExpiryDate,RenewalAmount,isActive,TimeStamp,UserId")] Renewal renewal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(renewal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.Organizations, "ID", "OrgName", renewal.OrganizationID);
            ViewBag.SubscriptionID = new SelectList(db.Subscriptions, "ID", "UserId", renewal.SubscriptionID);
            return View(renewal);
        }

        // GET: Renewals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renewal renewal = db.Renewals.Find(id);
            if (renewal == null)
            {
                return HttpNotFound();
            }
            return View(renewal);
        }

        // POST: Renewals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Renewal renewal = db.Renewals.Find(id);
            db.Renewals.Remove(renewal);
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
