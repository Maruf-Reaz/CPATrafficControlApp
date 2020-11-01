using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPATCMSApp.Models.Common.Authentication;
using CPATCMSApp.Models.Common.ViewModels;
using jQueryDatatableServerSideNetCore22.Extensions;
using jQueryDatatableServerSideNetCore22.Models.AuxiliaryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPATCMSApp.Controllers
{
    [Authorize(Roles = "SuperAdmin, HarbourAndMarine")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        //public async Task<IActionResult> Index()
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var editUserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles
            };

            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditUserViewModel editUserViewModel)
        {
            if (id != editUserViewModel.Id)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.UserName = editUserViewModel.UserName;
                user.Email = editUserViewModel.Email;

                var result = await _userManager.UpdateAsync(user);

                //Redirect User
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Users");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(editUserViewModel);
            }
        }

        //GET
        [HttpGet]
        public async Task<IActionResult> EditRolesOfUser(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.userId = userId;
            ViewBag.userName = user.UserName;

            //var loggedInUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var userRoleViewModels = new List<UserRoleViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                userRoleViewModels.Add(userRoleViewModel);
            }

            return View(userRoleViewModels);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> EditRolesOfUser(string userId, List<UserRoleViewModel> userRoleViewModels)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            for (int i = 0; i < userRoleViewModels.Count; i++)
            {
                var role = await _roleManager.FindByIdAsync(userRoleViewModels[i].RoleId);
                IdentityResult result = null;
                if (userRoleViewModels[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRoleViewModels[i].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < userRoleViewModels.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("Edit", new { Id = userId });
                    }
                }
            }

            return RedirectToAction("Edit", new { Id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody]DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                try
                {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                }
                catch (Exception)
                {

                    orderCriteria = "Id";
                    orderAscendingDirection = true;
                }
            }
            else
            {
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            //here
            var result = await _userManager.Users.Include(m => m.Gate).Include(m => m.Yard).ToListAsync();

            //here
            List<ApplicationUser> Users = new List<ApplicationUser>();

            //here
            foreach (var item in result)
            {
                ApplicationUser tempIdentityUser = new ApplicationUser
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    //GateName = item.Gate.Name,
                    //YardName = item.Yard.Name,
                    //UserTypeName = item.UserType.Name
                };
                Users.Add(tempIdentityUser);
            }

            //here
            if (!string.IsNullOrEmpty(searchBy))
            {
                Users = Users.Where(r => 
                r.UserName != null && r.UserName.ToUpper().Contains(searchBy.ToUpper()) ||
                //r.GateName != null && r.GateName.ToUpper().Contains(searchBy.ToUpper()) ||
                //r.YardName != null && r.YardName.ToUpper().Contains(searchBy.ToUpper()) ||
                //r.UserTypeName != null && r.UserTypeName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Email != null && r.Email.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            //here
            Users = orderAscendingDirection ? Users.AsQueryable().OrderByDynamic(orderCriteria,
                LinqExtensions.Order.Asc).ToList() : Users.AsQueryable().OrderByDynamic(orderCriteria,
                LinqExtensions.Order.Desc).ToList();

            var filteredResultsCount = result.Count();
            //here
            var totalResultsCount = await _userManager.Users.Include(m => m.Gate).Include(m => m.Yard).CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    //here
                    data = Users
                   .Skip(dtParameters.Start)
                   .ToList()
                });
            }
            else
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    //here
                    data = Users
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }
    }
}