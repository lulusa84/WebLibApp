using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebLibApp.Models;
using WebLibApp.ViewModels;

namespace WebLibApp.Controllers

{
    //[Authorize(Policy = "AdminRolePolicy")]
    [Authorize(Policy = "EditRolePolicy")]   
    /* if
    [Authorize(Roles = "SuperUser, "User")]
    [Authorize(Roles = "User")] no access allowes */
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {  // find User By userId
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            // find UserClaims asssociated to an UserId
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }
                //Assign claim to UserId
                model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {  //find User by UserId
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }
            // find Claims assigned to UserId
            var claims = await userManager.GetClaimsAsync(user);
            // deleting all assigned claims
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }
            //in case of new assigmente request,
            //AddClaimsAsync method
            // update model
            result = await userManager.AddClaimsAsync(user,
                model.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }
            //render up to date view
            return RedirectToAction("EditUser", new { Id = model.UserId });
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            //New List from model 
            var model = new List<UserRolesViewModel>();
            // loop through each role in table 
            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }
            //when we have every instace populated
            //data are passed to the View
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            //get all Roles a specific User is enrolled
            var roles = await userManager.GetRolesAsync(user);
            //remove all existing user Roles
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            //in case of failure
            if (!result.Succeeded)
            {  //error view is rendered
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            // in case of new Roles selected
            // procedure is AddToRoleAsync
            //  method that get a List of 
            // selected items
            // up to date View has to be rendered 
            result = await userManager.AddToRolesAsync(user,
            model.Where(x => x.IsSelected).Select(y => y.RoleName));
            // in case of failure
            if (!result.Succeeded)
            {   //error view is rendered
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            //if is successful proceed
            return RedirectToAction("EditUser", new { Id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {  //find User By Id
            var user = await userManager.FindByIdAsync(id);
            // if null 
            if (user == null)
            {  //user doesn't exist 
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {  // if user exist proceed with delete action
                var result = await userManager.DeleteAsync(user);
                // if action is succefull
                if (result.Succeeded)
                {  //  Up to date View ListUsers is rendered  
                    return RedirectToAction("ListUsers");
                }
                //in case of errors
                foreach (var error in result.Errors)
                {  // mostra ogni errore e la descrizione
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        // [Authorize(Roles = ... )]
        public async Task<IActionResult> DeleteRole(string id)
        {
            //find role by Id
            var role = await roleManager.FindByIdAsync(id);
            //if role doesn't exist
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    //procees with role Delete
                    var result = await roleManager.DeleteAsync(role);
                    // if succesful
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    // in case of error not included in DbUpdateException 
                    // class, throw new Exception("Test Exception");

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListRoles");
                }

                // in case of DbUpdateException, show errore messagge
                // and related RoleId. NOTICE: In case of Users enroled please 
                // remove them from it before proceding 
                catch (DbUpdateException ex)
                {
                    string m = $" Error deleting role {ex}";
                    ex.Message.Append(m);
                    Console.WriteLine(ex.Message.Append(m));

                    logger.LogError($"Error deleting role {ex}");

                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users " +
                        $"in this role. If you want to delete this role, please remove the users from " +
                        $"the role and then try to delete";
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {  //retrieve the user by Id
            var user = await userManager.FindByIdAsync(id);
            // if use is non foud Not foud view is rendered 
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            /* if user exist new Edit User is Shown
               istancing userClaims and userRoles
               The first are in fact a NAME-VALUE 
               pair, it' a piece of in information
               about the user: privileged
               or prerogative actions can be based
               on them, and have Type and Value attributes
               example(apply to Maternity is allowe only 
               to women at 7 months of pregnancy) 
               e-commerce, access to > 18 aged
               a Role is a particular Type of Claim in ASP.NET
               examples: Crete Role, Edit Role, Delete Role */

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select(c => c.Type + " : " + c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            /* roleManger is a reposotry wich include 
             * method to Retrive and Manage Role data */

            var role = await roleManager.FindByIdAsync(roleId);
            /* if role can't be found Role doesn't exist */
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            /* if Role is found a new view based on 
             * userRoleviewModel and containing
             * a List of User enrolled, based n the same model,
             * is created */

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                /* if one ore more User in List are
                 * selected, add that to the the view
                 * to be rendered */

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {  /* find a RoleId */
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            /* Verify whether an enrollerd User is selected or not */
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;
                /* selected, not enrolled */
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    /* Add to the Role */
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                /* not selected,enrolled */
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    /* remove from */
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    /* selected ad already enrolled,
                     * unselected an already removed */
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        /* redirect to action and edit Role */
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            /* if Role in not found crete it by Editing role function */
            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
