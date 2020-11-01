using System;
using System.Linq;
using CPATCMSApp.Data;
using CPATCMSApp.Models.CnFs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CPATCMSApp.Models.Assignments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CPATCMSApp.Models.Common.Authentication;
using jQueryDatatableServerSideNetCore22.Extensions;
using jQueryDatatableServerSideNetCore22.Models.AuxiliaryModels;

namespace CPATCMSApp.Controllers
{
    public class CnFProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CnFProfilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CnFProfiles
        [Authorize(Roles = "GateAdmin, HarbourAndMarine, Mechanical, Admin, SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: CnFProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cnFProfile = await _context.CnFProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cnFProfile == null)
            {
                return NotFound();
            }

            return View(cnFProfile);
        }


        // GET: CnFProfiles/Edit/5
        [Authorize(Roles = "Cnf, SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var loggedInUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (loggedInUser == null)
            {
                return NotFound();
            }

            var cnFProfile = await _context.CnFProfiles.Where(m => m.ApplicationUserId == loggedInUser.Id).FirstOrDefaultAsync();
            if (cnFProfile == null)
            {
                return NotFound();
            }

            return View(cnFProfile);
        }

        // POST: CnFProfiles/Edit/5
        [Authorize(Roles = "Cnf, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CnFProfile cnFProfile)
        {
            if (id != cnFProfile.Id)
            {
                return NotFound();
            }

            var profile = await _context.CnFProfiles.Where(m => m.Id == id).FirstOrDefaultAsync();
            profile.Address = cnFProfile.Address;
            profile.Name = cnFProfile.Name;
            profile.PhoneNumber = cnFProfile.PhoneNumber;
            profile.VerificationNumber = cnFProfile.VerificationNumber;
            cnFProfile.ApplicationUserId = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)).Id; //same thing
            try
            {
                _context.Update(profile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CnFProfileExists(cnFProfile.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(CnfAssignments));
        }

        private bool CnFProfileExists(int id)
        {
            return _context.CnFProfiles.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Cnf, SuperAdmin")]
        public IActionResult CnfAssignments()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SaveTruckQty(int assignmentItemId, int truckQty)
        {
            var assignmentItem = await _context.AssignmentItems.Where(m => m.Id == assignmentItemId).FirstOrDefaultAsync();
            assignmentItem.EstimatedTruckQty = truckQty;
            _context.Update(assignmentItem);
            await _context.SaveChangesAsync();
            return Json(true);

        }

        [HttpPost]
        public async Task<JsonResult> CheckBalance(int truckId)
        {

            var truckEntity = await _context.TruckEntities.Where(m => m.Id == truckId)
                .Include(m => m.AssignmentItem)
                .ThenInclude(m => m.CnFProfile)
                .FirstOrDefaultAsync();
            if (truckEntity.AssignmentItem.CnFProfile.Balance >= 1000)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }


        }

        [Authorize(Roles = "Cnf, SuperAdmin")]
        public async Task<IActionResult> AddTruckDetails(int id)
        {
            var assignmetItem = await _context.AssignmentItems
                .Include(a => a.Assignment)
                .ThenInclude(a => a.AssignmentSlot)
                .Include(a => a.TruckEntities)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmetItem == null)
            {
                return NotFound();
            }

            var loggedInUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var cnfProfile = await _context.CnFProfiles.FirstOrDefaultAsync(m => m.ApplicationUserId == loggedInUser.Id);
            if (assignmetItem.CnFProfileId != cnfProfile.Id)
            {
                return NotFound();
            }

            //var items = assignmetItems.OrderBy(m => m.Id).ToList();
            ViewData["AssignmetItem"] = assignmetItem;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SaveNewTruckNumber(int assignmentItemId, string newTruckNumber, string newIdCardNumberOfDriver, string newIdCardNumberOfAssistant)
        {
            //var truckEntity = new TruckEntity();
            var truckEntity = new TruckEntity
            {
                AssignmentItemId = assignmentItemId,
                TruckNumer = newTruckNumber,
                IdCardNumberOfDriver = newIdCardNumberOfDriver,
                IdCardNumberOfAssistant = newIdCardNumberOfAssistant,
                Status = 1
            };
            //truckEntity.AssignmentItemId = assignmentItemId;
            //truckEntity.TruckNumer = newTruckNumber;
            //truckEntity.Status = 1;
            bool isSavedSuccessfully;
            try
            {
                _context.Add(truckEntity);
                int count = await _context.SaveChangesAsync();
                if (count == 1)
                {
                    isSavedSuccessfully = true;
                }
                else
                {
                    isSavedSuccessfully = false;
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            return Json(isSavedSuccessfully);
        }

        [HttpPost]
        public async Task<JsonResult> SaveTruckNumber(int truckEntityId, string truckNumber, string idCardNumberOfDriver, string idCardNumberOfAssistant)
        {

            var truckEntity = await _context.TruckEntities
                .Where(m => m.Id == truckEntityId)
                .FirstOrDefaultAsync();

            truckEntity.TruckNumer = truckNumber;
            truckEntity.IdCardNumberOfDriver = idCardNumberOfDriver;
            truckEntity.IdCardNumberOfAssistant = idCardNumberOfAssistant;

            _context.Update(truckEntity);
            await _context.SaveChangesAsync();

            return Json(true);
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
            var result = await _context.CnFProfiles.ToListAsync();
            var cnFProfiles = new List<CnFProfile>();
            foreach (var item in result)
            {
                var cnFProfile = new CnFProfile
                {
                    Id = item.Id,
                    Name = item.Name,
                    PhoneNumber = item.PhoneNumber,
                    VerificationNumber = item.VerificationNumber,
                    Balance = item.Balance,
                    Address = item.Address,
                };
                cnFProfiles.Add(cnFProfile);
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                cnFProfiles = cnFProfiles.Where(r =>
                r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                r.PhoneNumber != null && r.PhoneNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.VerificationNumber != null && r.VerificationNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Balance.ToString().Contains(searchBy.ToUpper()) ||
                r.Address != null && r.Address.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            cnFProfiles = orderAscendingDirection ? cnFProfiles.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : cnFProfiles.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            var filteredResultsCount = cnFProfiles.Count();

            //here
            var totalResultsCount = await _context.CnFProfiles.CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = cnFProfiles
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
                    data = cnFProfiles
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable2([FromBody]DTParameters dtParameters)
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
                    orderAscendingDirection = false;
                }
            }
            else
            {
                orderCriteria = "Id";
                orderAscendingDirection = false;
            }

            //here
            var id = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)).Id;
            var result = await _context.AssignmentItems
                .Include(a => a.Assignment)
                .ThenInclude(a => a.AssignmentSlot)
                .Include(a => a.CnFProfile)
                .Include(a => a.Yard)
                .Where(a => a.Status >= 1 && a.CnFProfile.ApplicationUserId == id && a.YardId == 2).ToListAsync();

            //here
            List<AssignmentItem> AssignmentItems = new List<AssignmentItem>();

            //here
            foreach (var item in result)
            {
                AssignmentItem tempAssignmentItem = new AssignmentItem
                {
                    Id = item.Id,
                    Vessel = item.Vessel,
                    ImpReg = item.ImpReg,
                    MLO = item.MLO,
                    ContainerNumber = item.ContainerNumber,
                    Size = item.Size,
                    Height = item.Height,
                    LineNumber = item.LineNumber,
                    Dst = item.Dst,
                    Remarks = item.Remarks,
                    VerifyNumber = item.VerifyNumber,
                    ExitNumber = item.ExitNumber,
                    EstimatedTruckQty = item.EstimatedTruckQty,
                    Status = item.Status,
                    AssignmentDate = item.Assignment.Date,
                    AssignmentAssignmentSlotAssignmentName = item.Assignment.AssignmentSlot.AssignmentName,
                    YardName = item.Yard.Name,
                };
                AssignmentItems.Add(tempAssignmentItem);
            }

            //here
            if (!string.IsNullOrEmpty(searchBy))
            {
                AssignmentItems = AssignmentItems.Where(r =>
                r.Vessel != null && r.Vessel.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ImpReg != null && r.ImpReg.ToUpper().Contains(searchBy.ToUpper()) ||
                r.MLO != null && r.MLO.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ContainerNumber != null && r.ContainerNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.LineNumber != null && r.LineNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Dst != null && r.Dst.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Remarks != null && r.Remarks.ToUpper().Contains(searchBy.ToUpper()) ||
                r.VerifyNumber != null && r.VerifyNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ExitNumber != null && r.ExitNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Size.ToString().Contains(searchBy.ToUpper()) ||
                r.Height.ToString().Contains(searchBy.ToUpper()) ||
                r.EstimatedTruckQty.ToString().Contains(searchBy.ToUpper()) ||
                r.AssignmentDate != null && r.AssignmentDate.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.AssignmentAssignmentSlotAssignmentName != null && r.AssignmentAssignmentSlotAssignmentName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.YardName != null && r.YardName.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            //here
            AssignmentItems = orderAscendingDirection ? AssignmentItems.AsQueryable().OrderByDynamic(orderCriteria,
                LinqExtensions.Order.Asc).ToList() : AssignmentItems.AsQueryable().OrderByDynamic(orderCriteria,
                LinqExtensions.Order.Desc).ToList();

            var filteredResultsCount = result.Count();
            //here
            var totalResultsCount = await _context.AssignmentItems
                .Include(a => a.Assignment)
                .ThenInclude(a => a.AssignmentSlot)
                .Include(a => a.CnFProfile)
                .Include(a => a.Yard)
                .Where(a => a.Status >= 1 && a.CnFProfile.ApplicationUserId == id && a.YardId == 2).CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    //here
                    data = AssignmentItems
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
                    data = AssignmentItems
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }
    }
}
