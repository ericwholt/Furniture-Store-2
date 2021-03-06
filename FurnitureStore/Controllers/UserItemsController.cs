﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FurnitureStore.Models;

namespace FurnitureStore.Controllers
{
    public class UserItemsController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: UserItems
        public ActionResult Index()
        {
            var userItems = db.UserItems.Include(u => u.Item).Include(u => u.User);
            return View(userItems.ToList());
        }

        // GET: UserItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserItem userItem = db.UserItems.Find(id);
            if (userItem == null)
            {
                return HttpNotFound();
            }
            return View(userItem);
        }

        // GET: UserItems/Create
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Items, "id", "itemName");
            ViewBag.UserID = new SelectList(db.Users, "id", "userName");
            return View();
        }

        // POST: UserItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserItemId,UserID,ItemID")] UserItem userItem)
        {
            if (ModelState.IsValid)
            {
                db.UserItems.Add(userItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Items, "id", "itemName", userItem.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "id", "userName", userItem.UserID);
            return View(userItem);
        }

        // GET: UserItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserItem userItem = db.UserItems.Find(id);
            if (userItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "id", "itemName", userItem.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "id", "userName", userItem.UserID);
            return View(userItem);
        }

        // POST: UserItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserItemId,UserID,ItemID")] UserItem userItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "id", "itemName", userItem.ItemID);
            ViewBag.UserID = new SelectList(db.Users, "id", "userName", userItem.UserID);
            return View(userItem);
        }

        // GET: UserItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserItem userItem = db.UserItems.Find(id);
            if (userItem == null)
            {
                return HttpNotFound();
            }
            return View(userItem);
        }

        // POST: UserItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserItem userItem = db.UserItems.Find(id);
            User user = db.Users.Find(userItem.UserID);
            Item item = db.Items.Find(userItem.ItemID);
            user.funds += item.price;
            item.quantity++;
            db.Users.AddOrUpdate(user);
            db.Items.AddOrUpdate(item);
            db.UserItems.Remove(userItem);
            db.SaveChanges();
            Session["User"] = user;
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
