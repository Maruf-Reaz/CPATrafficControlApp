using System;
using System.Linq;
using CPATCMSApp.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CPATCMSApp.Models.Assignments;
using Microsoft.AspNetCore.Identity;
using CPATCMSApp.Models.TrialAssignments;
using Microsoft.AspNetCore.Authorization;
using CPATCMSApp.Models.Common.Authentication;
using jQueryDatatableServerSideNetCore22.Extensions;
using jQueryDatatableServerSideNetCore22.Models.AuxiliaryModels;

namespace CPATCMSApp.Controllers
{
    [Authorize]
    public class DtAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DtAssignmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody]DTParameters dtParameters, DateTime? fromDate, DateTime? toDate)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
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
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                // orderCriteria = "Id";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = false;
            }

            var result = new List<Assignment>();
            if (fromDate == null || toDate == null)
            {
                result = await _context.Assignments.Include(a => a.AssignmentSlot).ToListAsync();
            }
            else
            {
                result = await _context.Assignments.Include(a => a.AssignmentSlot).Where(m => (m.Date >= fromDate && m.Date <= toDate)).ToListAsync();
            }

            var assignments = new List<Assignment>();

            foreach (var item in result)
            {
                var tempAssignment = new Assignment
                {
                    Id = item.Id,
                    Date = item.Date,
                    AssignmentSlotAssignmentName = item.AssignmentSlot.AssignmentName,
                    NumberOfAssignmentItems = item.NumberOfAssignmentItems,
                    Status = item.Status
                };
                assignments.Add(tempAssignment);
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                assignments = assignments.Where(r =>
                r.Date != null && r.Date.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.AssignmentSlotAssignmentName != null && r.AssignmentSlotAssignmentName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.NumberOfAssignmentItems.ToString().ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            assignments = orderAscendingDirection ? assignments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : assignments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = assignments.Count();
            var totalResultsCount = await _context.Assignments.CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = assignments.Skip(dtParameters.Start).ToList()
                });
            }
            else
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = assignments.Skip(dtParameters.Start).Take(dtParameters.Length).ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignmentItem([FromBody]DTParameters dtParameters, int? id)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
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
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                // orderCriteria = "Id";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }


            if (id == null)
            {
                return NotFound();
            }

            var results = await _context.AssignmentItems
                .Include(a => a.Assignment.AssignmentSlot)
                .Include(a => a.Assignment)
                .Include(a => a.CnFProfile)
                .Include(a => a.Yard)
                .ThenInclude(a => a.Bay)
                .Where(m => m.Assignment.Id == id)
                .ToListAsync();

            List<AssignmentItem> assignmentItems = new List<AssignmentItem>();

            foreach (var item in results)
            {
                AssignmentItem tempAssignmentItem = new AssignmentItem
                {
                    Id = item.Id,
                    CnFProfileName = item.CnFProfile.Name,
                    CnFProfileVerificationNumber = item.CnFProfile.VerificationNumber,
                    Vessel = item.Vessel,
                    ImpReg = item.ImpReg,
                    MLO = item.MLO,
                    ContainerNumber = item.ContainerNumber,
                    YardName = item.Yard.Name,
                    BayName = item.Yard.Bay.Name,
                    Size = item.Size,
                    Height = item.Height,
                    LineNumber = item.LineNumber,
                    Dst = item.Dst,
                    Remarks = item.Remarks,
                    VerifyNumber = item.VerifyNumber,
                    ExitNumber = item.ExitNumber,
                    AssignmentType = item.AssignmentType,
                };
                assignmentItems.Add(tempAssignmentItem);
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                assignmentItems = assignmentItems.Where(r =>
                r.Vessel != null && r.Vessel.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentDate != null && r.AssignmentDate.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.ImpReg != null && r.ImpReg.ToUpper().Contains(searchBy.ToUpper()) ||
                r.MLO != null && r.MLO.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ContainerNumber != null && r.ContainerNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.LineNumber != null && r.LineNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Remarks != null && r.Remarks.ToUpper().Contains(searchBy.ToUpper()) ||
                r.VerifyNumber != null && r.VerifyNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ExitNumber != null && r.ExitNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentAssignmentSlotAssignmentName != null && r.AssignmentAssignmentSlotAssignmentName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CnFProfileName != null && r.CnFProfileName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Dst != null && r.Dst.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Size.ToString().Contains(searchBy.ToUpper()) ||
                r.Height.ToString().Contains(searchBy.ToUpper())
                ).ToList();
            }

            assignmentItems = orderAscendingDirection ? assignmentItems.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : assignmentItems.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = assignmentItems.Count();

            var totalResultsCount = await _context.AssignmentItems.Where(m => m.Assignment.Id == id).CountAsync();


            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = assignmentItems
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
                    data = assignmentItems
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GateTruckIndex([FromBody]DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
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
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                // orderCriteria = "Id";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = false;
            }

            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var result = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Include(a => a.AssignmentItem.Assignment)
                .Include(a => a.AssignmentItem.CnFProfile)
                .Include(a => a.AssignmentItem.Yard)
                .Include(a => a.AssignmentItem.Yard.Gate)
                .Where(a => (a.AssignmentItem.Status == 3 && a.AssignmentItem.Assignment.DeliveryDate.Date == DateTime.Now.Date && a.AssignmentItem.Yard.GateId == user.GateId && (a.AssignmentItem.Assignment.DeliveryStart <= totalTime && totalTime <= a.AssignmentItem.Assignment.DeliveryEnd)) || ((a.Status == 2 || a.Status == 3) && a.AssignmentItem.Yard.GateId == user.GateId))
                .ToListAsync();

            List<TruckEntity> truckEntities = new List<TruckEntity>();

            foreach (var item in result)
            {
                TruckEntity tempTruckEntity = new TruckEntity
                {
                    Id = item.Id,
                    TruckNumer = item.TruckNumer,
                    AssignmentItemCnFProfileName = item.AssignmentItem.CnFProfile.Name,
                    AssignmentItemContainerNumber = item.AssignmentItem.ContainerNumber,
                    AssignmentItemVerifyNumber = item.AssignmentItem.VerifyNumber,
                    AssignmentItemExitNumber = item.AssignmentItem.ExitNumber,
                    Status = item.Status,
                };
                truckEntities.Add(tempTruckEntity);
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                truckEntities = truckEntities.Where(r =>
                r.TruckNumer != null && r.TruckNumer.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentItemCnFProfileName != null && r.AssignmentItemCnFProfileName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentItemContainerNumber != null && r.AssignmentItemContainerNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentItemVerifyNumber != null && r.AssignmentItemVerifyNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentItemExitNumber != null && r.AssignmentItemExitNumber.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            truckEntities = orderAscendingDirection ? truckEntities.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : truckEntities.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Include(a => a.AssignmentItem.Assignment)
                .Include(a => a.AssignmentItem.CnFProfile)
                .Include(a => a.AssignmentItem.Yard)
                .Include(a => a.AssignmentItem.Yard.Gate)
                .Where(a => (a.AssignmentItem.Status == 3 && a.AssignmentItem.Assignment.DeliveryDate.Date == DateTime.Now.Date && a.AssignmentItem.Yard.GateId == user.GateId && (a.AssignmentItem.Assignment.DeliveryStart <= totalTime && totalTime <= a.AssignmentItem.Assignment.DeliveryEnd)) || ((a.Status == 2 || a.Status == 3) && a.AssignmentItem.Yard.GateId == user.GateId))
                .CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = truckEntities
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
                    data = truckEntities
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TodaysAssignmentIndex([FromBody]DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
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
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                // orderCriteria = "Id";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = false;
            }

            var result = await _context.Assignments.Include(a => a.AssignmentSlot).Where(a => a.Date == DateTime.Today.Date).ToListAsync();

            var assignments = new List<Assignment>();

            foreach (var item in result)
            {
                var tempAssignment = new Assignment
                {
                    Id = item.Id,
                    Date = item.Date,
                    AssignmentSlotAssignmentName = item.AssignmentSlot.AssignmentName,
                    NumberOfAssignmentItems = item.NumberOfAssignmentItems,
                    Status = item.Status
                };
                assignments.Add(tempAssignment);
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                assignments = assignments.Where(r =>
                r.Date != null && r.Date.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.AssignmentSlotAssignmentName != null && r.AssignmentSlotAssignmentName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.NumberOfAssignmentItems.ToString().ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            assignments = orderAscendingDirection ? assignments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : assignments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = assignments.Count();
            var totalResultsCount = await _context.Assignments.Where(a => a.Date == DateTime.Today.Date).CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = assignments.Skip(dtParameters.Start).ToList()
                });
            }
            else
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = assignments.Skip(dtParameters.Start).Take(dtParameters.Length).ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> YardAssignmentIndex([FromBody]DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
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
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                // orderCriteria = "Id";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = false;
            }

            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var result = await _context.AssignmentItems
                .Include(a => a.Assignment)
                .ThenInclude(a => a.AssignmentSlot)
                .Include(a => a.CnFProfile)
                .Include(a => a.Yard)
                .Where(a => (a.Status == 1 || a.Status == 2) && a.YardId == user.YardId)
                .ToListAsync();

            List<AssignmentItem> assignmentItems = new List<AssignmentItem>();

            foreach (var item in result)
            {
                AssignmentItem tempAssignmentItem = new AssignmentItem
                {
                    Id = item.Id,
                    Vessel = item.Vessel,
                    AssignmentDate = item.Assignment.Date,
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
                    AssignmentAssignmentSlotAssignmentName = item.Assignment.AssignmentSlot.AssignmentName,
                    CnFProfileName = item.CnFProfile.Name,
                };
                assignmentItems.Add(tempAssignmentItem);
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                assignmentItems = assignmentItems.Where(r =>
                r.Vessel != null && r.Vessel.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ImpReg != null && r.ImpReg.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentDate != null && r.AssignmentDate.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.MLO != null && r.MLO.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ContainerNumber != null && r.ContainerNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.LineNumber != null && r.LineNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Remarks != null && r.Remarks.ToUpper().Contains(searchBy.ToUpper()) ||
                r.VerifyNumber != null && r.VerifyNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ExitNumber != null && r.ExitNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentAssignmentSlotAssignmentName != null && r.AssignmentAssignmentSlotAssignmentName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CnFProfileName != null && r.CnFProfileName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Dst != null && r.Dst.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Size.ToString().Contains(searchBy.ToUpper()) ||
                r.Height.ToString().Contains(searchBy.ToUpper())
                ).ToList();
            }

            assignmentItems = orderAscendingDirection ? assignmentItems.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : assignmentItems.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.AssignmentItems
                .Include(a => a.Assignment)
                .ThenInclude(a => a.AssignmentSlot)
                .Include(a => a.CnFProfile)
                .Include(a => a.Yard)
                .Where(a => (a.Status == 1 || a.Status == 2) && a.YardId == user.YardId)
                .CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = assignmentItems
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
                    data = assignmentItems
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> YardKeptDownAssignments([FromBody]DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
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
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                // orderCriteria = "Id";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing
            var result = await _context.AssignmentItems
                .Include(a => a.Assignment)
                .ThenInclude(a => a.AssignmentSlot)
                .Include(a => a.CnFProfile)
                .Include(a => a.Yard)
                .Where(a => a.Status == 3 && a.YardId == user.YardId)
                .ToListAsync();

            List<AssignmentItem> assignmentItems = new List<AssignmentItem>();

            foreach (var item in result)
            {
                AssignmentItem tempAssignmentItem = new AssignmentItem
                {
                    Id = item.Id,
                    Vessel = item.Vessel,
                    AssignmentDate = item.Assignment.Date,
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
                    AssignmentAssignmentSlotAssignmentName = item.Assignment.AssignmentSlot.AssignmentName,
                    CnFProfileName = item.CnFProfile.Name,
                    KeepDownTime = item.KeepDownTime
                };
                assignmentItems.Add(tempAssignmentItem);
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                assignmentItems = assignmentItems.Where(r =>
                r.Vessel != null && r.Vessel.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentDate != null && r.AssignmentDate.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.KeepDownTime != null && r.KeepDownTime.ToString("dd-MM-yyyy HH:mm").Contains(searchBy.ToString()) ||
                r.ImpReg != null && r.ImpReg.ToUpper().Contains(searchBy.ToUpper()) ||
                r.MLO != null && r.MLO.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ContainerNumber != null && r.ContainerNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.LineNumber != null && r.LineNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Remarks != null && r.Remarks.ToUpper().Contains(searchBy.ToUpper()) ||
                r.VerifyNumber != null && r.VerifyNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ExitNumber != null && r.ExitNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentAssignmentSlotAssignmentName != null && r.AssignmentAssignmentSlotAssignmentName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CnFProfileName != null && r.CnFProfileName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Dst != null && r.Dst.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Size.ToString().Contains(searchBy.ToUpper()) ||
                r.Height.ToString().Contains(searchBy.ToUpper())
                ).ToList();
            }

            assignmentItems = orderAscendingDirection ? assignmentItems.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : assignmentItems.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.AssignmentItems
                .Include(a => a.Assignment)
                .ThenInclude(a => a.AssignmentSlot)
                .Include(a => a.CnFProfile)
                .Include(a => a.Yard)
                .Where(a => a.Status == 3 && a.YardId == user.YardId)
                .CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = assignmentItems
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
                    data = assignmentItems
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TrialAssignments([FromBody]DTParameters dtParameters, DateTime? assignmentDate)
        {
            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
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
                //orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                //orderCriteria = "Id";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Id";
                orderAscendingDirection = true;
            }

            var result = new List<TrialAssignmentItem>();
            if (assignmentDate == null)
            {
                result = await _context.TrialAssignmentItems.ToListAsync();
            }
            else
            {
                result = await _context.TrialAssignmentItems.Where(m => m.CreationTime == assignmentDate).ToListAsync();
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r =>
                r.BlockName != null && r.BlockName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Vessel != null && r.Vessel.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CreationTime != null && r.CreationTime.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.ImpReg != null && r.ImpReg.ToUpper().Contains(searchBy.ToUpper()) ||
                r.MLO != null && r.MLO.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ContainerNumber != null && r.ContainerNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.LineNumber != null && r.LineNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Remarks != null && r.Remarks.ToUpper().Contains(searchBy.ToUpper()) ||
                r.VerifyNumber != null && r.VerifyNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ExitNumber != null && r.ExitNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CnFCode != null && r.CnFCode.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CnFName != null && r.CnFName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Dst != null && r.Dst.ToUpper().Contains(searchBy.ToUpper()) ||
                r.AssignmentType != null && r.AssignmentType.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Size.ToString().Contains(searchBy.ToUpper()) ||
                r.Height.ToString().Contains(searchBy.ToUpper())).ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            //now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            int totalResultsCount = 0;
            if (assignmentDate == null)
            {
                totalResultsCount = await _context.TrialAssignmentItems.CountAsync();
            }
            else
            {
                totalResultsCount = await _context.TrialAssignmentItems.Where(m => m.CreationTime == assignmentDate).CountAsync();
            }
            //var totalResultsCount = await _context.TrialAssignmentItems.CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = result.Skip(dtParameters.Start).ToList()
                });
            }
            else
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = result.Skip(dtParameters.Start).Take(dtParameters.Length).ToList()
                });
            }
        }
    }
}