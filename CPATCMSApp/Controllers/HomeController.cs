using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CPATCMSApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CPATCMSApp.Models.Common.Authentication;
using CPATCMSApp.Data;
using CPATCMSApp.Models.ViewModels;

namespace CPATCMSApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        //[Authorize(Roles = "GateAdmin, HarbourAndMarine, Mechanical, Admin, TMOffice")]
        public async Task<IActionResult> Index()
        {
            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing
            if (await _userManager.IsInRoleAsync(user, "OneStop"))
            {
                return RedirectToAction("Index", "Assignments");
            }
            else if (await _userManager.IsInRoleAsync(user, "Yard"))
            {
                return RedirectToAction("YardAssignmentIndex", "Assignments");
            }
            else if (await _userManager.IsInRoleAsync(user, "GateSergent"))
            {
                return RedirectToAction("GateTruckIndex", "Assignments");
            }
            else if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                return View();
            }
            else if (await _userManager.IsInRoleAsync(user, "Cnf"))
            {
                return RedirectToAction("CnfAssignments", "CnFProfiles");
            }
            else if (await _userManager.IsInRoleAsync(user, "GateAdmin") || await _userManager.IsInRoleAsync(user, "HarbourAndMarine") || await _userManager.IsInRoleAsync(user, "Mechanical") || await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "TMOffice"))
            {
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult AssignmentData(DateTime getDate, DateTime fromDate)
        {
            if (fromDate == default(DateTime))
            {
                fromDate = DateTime.Now.Date;
            }

            ViewData["Date"] = fromDate.Date;
            return View();
        }

        [HttpPost]
        public IActionResult Fetch(FromDateToDateViewModel datesVM)
        {
            ViewData["Date"] = datesVM.FromDate;
            return RedirectToAction("AssignmentData", new { fromDate = datesVM.FromDate });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
