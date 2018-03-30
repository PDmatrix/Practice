﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Pract.Models;

namespace Pract.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // GET: Role
        public ActionResult Index()
        {
            //return View(RoleManager.Roles.ToArray());
            var users = UserManager.Users.AsEnumerable().ToArray();
            return View(users.Select(x => new UserManagementViewModel
            {
                Id = x.Id,
                Username = x.UserName,
                Role = UserManager.GetRoles(x.Id).FirstOrDefault(),
            }).ToArray());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Username };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                var newUser = await UserManager.FindByNameAsync(model.Username);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(newUser.Id, model.Role);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            return View(model);
        }
 
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(new UserManagementViewModel() { Id=user.Id, Role = UserManager.GetRoles(user.Id).FirstOrDefault(), Username = user.UserName} );
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> Edit(UserManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.Username;
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                    await UserManager.AddToRoleAsync(user.Id, model.Role);
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
            }
            return View(model);
        }
 
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(new UserManagementViewModel { Id = user.Id,  Username = user.UserName, Role = UserManager.GetRoles(user.Id).FirstOrDefault() });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleConfirmed(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            IdentityResult result = await UserManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}