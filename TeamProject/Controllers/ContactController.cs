﻿//JF Might be ok now?

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamProject.Models;

namespace TeamProject.Controllers
{
    public class ContactController : Controller
    {
        private ContactContext context { get; set; }

        public ContactController(ContactContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = context.Categories.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }).ToList();
            return View(new Contact());
        }

        [HttpPost]
        public IActionResult Add(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateAdded = DateTime.Now;

                context.Contacts.Add(contact);
                context.SaveChanges();

                return RedirectToAction("Index", "");
            }

            ViewBag.Categories = context.Categories.OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }).ToList();
            return View(new Contact());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var contact = context.Contacts.Include(c => c.Category).FirstOrDefault(c => c.ContactId == id);
            return View(contact);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Categories = context.Categories.OrderBy(c => c.Name).ToList();
            var contact = context.Contacts.Find(id);
            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (contact.ContactId == 0)
                    context.Contacts.Add(contact);
                else
                    context.Contacts.Update(contact);

                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Action = (contact.ContactId == 0) ? "Add" : "Edit";
                ViewBag.Categories = context.Categories.OrderBy(c => c.Name).ToList();
                return View(contact);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Fetch the contact to confirm deletion
            var contact = context.Contacts.FirstOrDefault(c => c.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var contact = context.Contacts.Find(id);
            if (contact != null)
            {
                context.Contacts.Remove(contact);
                context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}