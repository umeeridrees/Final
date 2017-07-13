using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final.Models;

namespace Final.Controllers
{
    [Authorize(Roles = "admin")]
    public class TakeController : Controller
    {
        private McqsDatabaseEntities db = new McqsDatabaseEntities();

        // GET: /Take/
        public ActionResult Index()
        {
            var takes = db.Takes.Include(t => t.Student).Include(t => t.Test);
            return View(takes.ToList());
        }

        // GET: /Take/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Take take = db.Takes.Find(id);
            if (take == null)
            {
                return HttpNotFound();
            }
            return View(take);
        }

        // GET: /Take/Create
        public ActionResult Create()
        {
            ViewBag.s_id = new SelectList(db.Students, "s_id", "password");
            ViewBag.test_id = new SelectList(db.Tests, "test_id", "name");
            return View();
        }

        // POST: /Take/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="take_id,s_id,test_id,marks")] Take take)
        {
            if (ModelState.IsValid)
            {
                db.Takes.Add(take);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.s_id = new SelectList(db.Students, "s_id", "password", take.s_id);
            ViewBag.test_id = new SelectList(db.Tests, "test_id", "name", take.test_id);
            return View(take);
        }

        // GET: /Take/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Take take = db.Takes.Find(id);
            if (take == null)
            {
                return HttpNotFound();
            }
            ViewBag.s_id = new SelectList(db.Students, "s_id", "password", take.s_id);
            ViewBag.test_id = new SelectList(db.Tests, "test_id", "name", take.test_id);
            return View(take);
        }

        // POST: /Take/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="take_id,s_id,test_id,marks")] Take take)
        {
            if (ModelState.IsValid)
            {
                db.Entry(take).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.s_id = new SelectList(db.Students, "s_id", "password", take.s_id);
            ViewBag.test_id = new SelectList(db.Tests, "test_id", "name", take.test_id);
            return View(take);
        }

        // GET: /Take/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Take take = db.Takes.Find(id);
            if (take == null)
            {
                return HttpNotFound();
            }
            return View(take);
        }

        // POST: /Take/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Take take = db.Takes.Find(id);
            db.Takes.Remove(take);
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
