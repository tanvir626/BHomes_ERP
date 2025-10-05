﻿using Bhomes_ERP.Models;
using Bhomes_ERP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bhomes_ERP.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TestPage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
            return View(model);
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = new ApplicationUser
        //    {
        //        FullName = model.Name,
        //        UserName = model.Email,
        //        NormalizedUserName = model.Email.ToUpper(),
        //        Email = model.Email,
        //        NormalizedEmail = model.Email.ToUpper()
        //    };

        //    var result = await userManager.CreateAsync(user, model.Password);

        //    if (result.Succeeded)
        //    {
        //        var roleExist = await roleManager.RoleExistsAsync("User");

        //        if (!roleExist)
        //        {
        //            var role = new IdentityRole("User");
        //            await roleManager.CreateAsync(role);
        //        }

        //        await userManager.AddToRoleAsync(user, "User");

        //        await signInManager.SignInAsync(user, isPersistent: false);
        //        return RedirectToAction("Login", "Account");
        //    }

        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    return View(model);
        //}

        [HttpGet]
        [Route("User/User List/")]
        public IActionResult UsetList()
        {
            // Load roles from AspNetRoles table
            var userListData = userManager.Users.ToList();
            return View(userListData);
        }

        [HttpGet]
        [Route("User/Add User/")]
        public IActionResult Register()
        {
            // Load roles from AspNetRoles table
            var roles = roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                })
                .ToList();

            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("User/Add User/")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload roles in case of validation failure
                ViewBag.Roles = roleManager.Roles
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    })
                    .ToList();

                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.Name,
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper()
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // If no role is selected, default to "User"
                var selectedRole = string.IsNullOrEmpty(model.Role) ? "User" : model.Role;

                // Ensure role exists
                if (!await roleManager.RoleExistsAsync(selectedRole))
                {
                    var role = new IdentityRole(selectedRole);
                    await roleManager.CreateAsync(role);
                }

                // Assign selected role
                await userManager.AddToRoleAsync(user, selectedRole);

                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Reload roles before returning view
            ViewBag.Roles = roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                })
                .ToList();

            return View(model);
        }





        //[HttpGet]
        //public IActionResult VerifyEmail()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await userManager.FindByNameAsync(model.Email);

        //    if (user == null)
        //    {
        //        ModelState.AddModelError("", "User not found!");
        //        return View(model);
        //    }
        //    else
        //    {
        //        return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string username)
        {
            var user = await userManager.GetUserAsync(User);

            return View(new ChangePasswordViewModel { Email = user.Email });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return View(model);
            }

            var result = await userManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                result = await userManager.AddPasswordAsync(user, model.NewPassword);
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
