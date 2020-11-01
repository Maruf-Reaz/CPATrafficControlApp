using System;
using System.Linq;
using CPATCMSApp.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CPATCMSApp.Models.Currency;
using CPATCMSApp.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CPATCMSApp.Models.Common.File;
using CPATCMSApp.Models.Assignments;
using Microsoft.AspNetCore.Authorization;
using CPATCMSApp.Models.Common.Authentication;

namespace CPATCMSApp.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static readonly HttpClient client = new HttpClient();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment hostingEnvironment;

        public AssignmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles = "OneStop, SuperAdmin")]
        public IActionResult TodaysAssignmentIndex()
        {
            //TwilioClient

            return View();
        }

        // GET: Assignments/Create
        [Authorize(Roles = "OneStop, SuperAdmin")]
        public async Task<IActionResult> Create(DateTime? assignmentDate)
        {
            ViewData["AssignmentDate"] = assignmentDate;
            if (assignmentDate == null)
            {
                //var assignments = await _context.Assignments
                //    .Where(a => a.Date == DateTime.Today.Date)
                //    .Select(o => o.AssignmentSlot)
                //    .ToListAsync();

                //var allSlots = await _context.AssignmentSlots.ToListAsync();

                //List<AssignmentSlot> result = allSlots.Except(assignments).ToList();

                //ViewData["AssignmentSlotId"] = new SelectList(allSlots, "Id", "AssignmentName");
                ViewData["AssignmentSlots"] = null;
                ViewData["Assignments"] = null;
                ViewData["TrialAssignmentItemsCount"] = null;
                return View();
            }
            else
            {
                var assignmentSlotsOfTheDate = await _context.Assignments
                    .Where(m => m.Date == assignmentDate).Select(o => o.AssignmentSlot).ToListAsync();
                var assignmentSlots = await _context.AssignmentSlots.Except(assignmentSlotsOfTheDate).ToListAsync();
                var assignments = await _context.Assignments.Include(m => m.AssignmentSlot).Where(m => m.Date == assignmentDate).ToListAsync();
                int trialAssignmentItemsCount = await _context.TrialAssignmentItems.Where(m => m.CreationTime == assignmentDate).CountAsync();
                ViewData["AssignmentSlots"] = assignmentSlots;
                ViewData["Assignments"] = assignments;
                ViewData["TrialAssignmentItemsCount"] = trialAssignmentItemsCount;
                return View();
            }
        }

        // POST: Assignments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "OneStop, SuperAdmin")]
        public async Task<IActionResult> Create(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                if (assignment.AssignmentSlotId == 1)
                {
                    assignment.DeliveryDate = DateTime.Now.AddDays(1).Date;
                    assignment.DeliveryStart = 800;
                    assignment.DeliveryEnd = 1200;
                }

                if (assignment.AssignmentSlotId == 2)
                {
                    assignment.DeliveryDate = DateTime.Now.AddDays(1).Date;
                    assignment.DeliveryStart = 1201;
                    assignment.DeliveryEnd = 1600;
                }

                if (assignment.AssignmentSlotId == 3)
                {
                    assignment.DeliveryDate = DateTime.Now.Date;
                    assignment.DeliveryStart = 1601;
                    assignment.DeliveryEnd = 2000;
                }

                if (assignment.AssignmentSlotId == 4)
                {
                    assignment.DeliveryDate = DateTime.Now.Date;
                    assignment.DeliveryStart = 2001;
                    assignment.DeliveryEnd = 2400;
                }

                assignment.Status = 1;
                _context.Add(assignment);
                await _context.SaveChangesAsync();

                var trialAssignmentItems = await _context.TrialAssignmentItems
                    .Where(m => m.CreationTime == assignment.Date)
                    .OrderBy(m => m.Id)
                    .Take(assignment.NumberOfAssignmentItems)
                    .ToListAsync();
                foreach (var trialAssignmentItem in trialAssignmentItems)
                {
                    var assignmentItem = await _context.AssignmentItems.Where(m => m.ContainerNumber == trialAssignmentItem.ContainerNumber && m.CreationTime == trialAssignmentItem.CreationTime).FirstOrDefaultAsync();
                    if (assignmentItem == null)
                    {
                        var newAssignItem = new AssignmentItem
                        {
                            AssignmentId = assignment.Id,
                            Vessel = trialAssignmentItem.Vessel,
                            ImpReg = trialAssignmentItem.ImpReg,
                            MLO = trialAssignmentItem.MLO,
                            ContainerNumber = trialAssignmentItem.ContainerNumber,
                            AssignmentType = trialAssignmentItem.AssignmentType,
                            YardId = trialAssignmentItem.YardId,
                            Size = trialAssignmentItem.Size,
                            Height = trialAssignmentItem.Height,
                            LineNumber = trialAssignmentItem.LineNumber,
                            Dst = trialAssignmentItem.Dst,
                            Remarks = trialAssignmentItem.Remarks,
                            VerifyNumber = trialAssignmentItem.VerifyNumber,
                            ExitNumber = trialAssignmentItem.ExitNumber,
                            CreationTime = trialAssignmentItem.CreationTime,
                            Status = 0
                        };

                        var cnFProfile = await _context.CnFProfiles.Where(m => m.VerificationNumber == trialAssignmentItem.CnFCode).FirstOrDefaultAsync();
                        if (cnFProfile == null)
                        {
                            newAssignItem.CnFProfileId = 1;
                        }
                        else
                        {
                            newAssignItem.CnFProfileId = cnFProfile.Id;
                        }
                        _context.Add(newAssignItem);
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            var assignmentSlotsOfTheDate = await _context.Assignments
                    .Where(m => m.Date == assignment.Date).Select(o => o.AssignmentSlot).ToListAsync();
            var assignmentSlots = await _context.AssignmentSlots.Except(assignmentSlotsOfTheDate).ToListAsync();
            var assignments = await _context.Assignments.Include(m => m.AssignmentSlot).Where(m => m.Date == assignment.Date).ToListAsync();
            int trialAssignmentItemsCount = await _context.TrialAssignmentItems.Where(m => m.CreationTime == assignment.Date).CountAsync();
            ViewData["AssignmentSlots"] = assignmentSlots;
            ViewData["Assignments"] = assignments;
            ViewData["TrialAssignmentItemsCount"] = trialAssignmentItemsCount;
            return View(assignment);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "OneStop, SuperAdmin")]
        //public async Task<IActionResult> Create(Assignment assignment)
        //{
        //    assignment.Status = 1;
        //    if (ModelState.IsValid)
        //    {
        //        if (assignment.AssignmentSlotId == 1)
        //        {
        //            assignment.DeliveryDate = DateTime.Now.AddDays(1).Date;
        //            assignment.DeliveryStart = 800;
        //            assignment.DeliveryEnd = 1200;
        //        }

        //        if (assignment.AssignmentSlotId == 2)
        //        {
        //            assignment.DeliveryDate = DateTime.Now.AddDays(1).Date;
        //            assignment.DeliveryStart = 1201;
        //            assignment.DeliveryEnd = 1600;
        //        }

        //        if (assignment.AssignmentSlotId == 3)
        //        {
        //            assignment.DeliveryDate = DateTime.Now.Date;
        //            assignment.DeliveryStart = 1601;
        //            assignment.DeliveryEnd = 2000;
        //        }

        //        if (assignment.AssignmentSlotId == 4)
        //        {
        //            assignment.DeliveryDate = DateTime.Now.Date;
        //            assignment.DeliveryStart = 2001;
        //            assignment.DeliveryEnd = 2400;
        //        }

        //        _context.Add(assignment);
        //        await _context.SaveChangesAsync();

        //        //no need
        //        var date = string.Format("{0:yyyy-MM-dd}", assignment.Date);
        //        WebRequest request = HttpWebRequest.Create("http://122.152.54.179/dataInfo/today_assign.php?dt=" + date);
        //        WebResponse response = request.GetResponse();
        //        StreamReader reader = new StreamReader(response.GetResponseStream());
        //        string urlText = reader.ReadToEnd(); // it takes the response from your url. now you can use as your need  

        //        var assignmentApiViewModels = JsonConvert.DeserializeObject<List<AssignmentApiViewModel>>(urlText);
        //        foreach (var assignmentApiViewModel in assignmentApiViewModels)
        //        {
        //            //string cnfName = assignmentApiViewModel.CNF_Agent.Trim();
        //            if (assignmentApiViewModel != null)
        //            {
        //                var cnf = await _context.CnFProfiles.Where(m => m.VerificationNumber == assignmentApiViewModel.CNF_Code).FirstOrDefaultAsync();

        //                if (cnf != null)
        //                {
        //                    var tempAssignmentItem = await _context.AssignmentItems.Where(m => m.ContainerNumber == assignmentApiViewModel.Container_No && m.CreationTime == assignment.Date).FirstOrDefaultAsync();

        //                    if (assignmentApiViewModel.Block_No == "NCY")
        //                    {
        //                        if (tempAssignmentItem == null)
        //                        {
        //                            AssignmentItem item = new AssignmentItem();
        //                            item.AssignmentId = assignment.Id;

        //                            item.CnFProfileId = cnf.Id;
        //                            item.Vessel = assignmentApiViewModel.Vessel_Name;
        //                            item.ImpReg = assignmentApiViewModel.Import_Rotation;
        //                            item.MLO = assignmentApiViewModel.MLO;
        //                            item.ContainerNumber = assignmentApiViewModel.Container_No;
        //                            item.AssignmentType = assignmentApiViewModel.Assignment_Type;
        //                            item.YardId = 2;
        //                            item.Size = assignmentApiViewModel.Cont_Size;
        //                            item.Height = assignmentApiViewModel.Cont_Height;
        //                            item.LineNumber = assignmentApiViewModel.line_no;
        //                            item.Dst = assignmentApiViewModel.Delivery_st;
        //                            item.Remarks = assignmentApiViewModel.remarks; ;
        //                            item.VerifyNumber = assignmentApiViewModel.Verify_No;
        //                            item.ExitNumber = assignmentApiViewModel.Exit_No; ;
        //                            item.CreationTime = assignment.Date;
        //                            item.Status = 0;
        //                            _context.Add(item);
        //                            await _context.SaveChangesAsync();
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    var tempAssignmentItem = await _context.AssignmentItems.Where(m => m.ContainerNumber == assignmentApiViewModel.Container_No && m.CreationTime == assignment.Date).FirstOrDefaultAsync();

        //                    if (assignmentApiViewModel.Block_No == "NCY")
        //                    {
        //                        if (tempAssignmentItem == null)
        //                        {
        //                            AssignmentItem item = new AssignmentItem();
        //                            item.AssignmentId = assignment.Id;
        //                            item.CnFProfileId = 1;
        //                            item.Vessel = assignmentApiViewModel.Vessel_Name;
        //                            item.ImpReg = assignmentApiViewModel.Import_Rotation;
        //                            item.MLO = assignmentApiViewModel.MLO;
        //                            item.ContainerNumber = assignmentApiViewModel.Container_No;
        //                            item.AssignmentType = assignmentApiViewModel.Assignment_Type;
        //                            item.YardId = 2;
        //                            item.Size = assignmentApiViewModel.Cont_Size;
        //                            item.Height = assignmentApiViewModel.Cont_Height;
        //                            item.LineNumber = assignmentApiViewModel.line_no;
        //                            item.Dst = assignmentApiViewModel.Delivery_st;
        //                            item.Remarks = assignmentApiViewModel.remarks; ;
        //                            item.VerifyNumber = assignmentApiViewModel.Verify_No;
        //                            item.ExitNumber = assignmentApiViewModel.Exit_No;
        //                            item.AssignmentType = assignmentApiViewModel.Assignment_Type;
        //                            item.CreationTime = assignment.Date;
        //                            item.Status = 0;
        //                            _context.Add(item);
        //                            await _context.SaveChangesAsync();
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return RedirectToAction(nameof(TodaysAssignmentIndex));
        //    }

        //    ViewData["AssignmentSlotId"] = new SelectList(_context.AssignmentSlots, "Id", "AssignmentName", assignment.AssignmentSlotId);
        //    return View(assignment);
        //}

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id, int fromPage)
        {
            if (fromPage != 1 && fromPage != 2)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments.FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            _context.Remove(assignment);
            await _context.SaveChangesAsync();

            if (fromPage == 1)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(TodaysAssignmentIndex));
            }
        }

        [Authorize(Roles = "OneStop, SuperAdmin")]
        public async Task<IActionResult> AddAssignmentItem(int? id)
        {
            var assignment = await _context.Assignments.Include(a => a.AssignmentSlot).FirstOrDefaultAsync(m => m.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            ViewData["Id"] = id;
            ViewData["AssignmentId"] = assignment.Id;
            ViewData["Status"] = assignment.Status;
            ViewData["SlotName"] = assignment.AssignmentSlot.AssignmentName;
            ViewData["AssignmentDate"] = assignment.Date;

            return View();
        }

        public async Task<JsonResult> GetCnfs()
        {
            var cnfs = await _context.CnFProfiles.ToListAsync();
            return Json(cnfs);
        }

        public async Task<JsonResult> GetYards()
        {
            var yards = await _context.Yards.ToListAsync();
            return Json(yards);
        }

        [HttpPost]
        public async Task<JsonResult> SubmitAssignment(int assignmentId)
        {
            var assignment = await _context.Assignments.Where(m => m.Id == assignmentId).FirstOrDefaultAsync();

            assignment.Status = 2;
            _context.Update(assignment);
            await _context.SaveChangesAsync();

            var assignmentItems = await _context.AssignmentItems
                .Include(m => m.CnFProfile)
                .Where(m => m.AssignmentId == assignmentId)
                .ToListAsync();

            if (assignmentItems.Count != 0)
            {
                foreach (var item in assignmentItems)
                {
                    //if (item.YardId == 2)
                    //{
                    //IDictionary<string, string> values = new Dictionary<string, string>();
                    //values.Add(new KeyValuePair<string, string>("token", "a380a7ad7b19fd20c6a161cb114ba228"));
                    //values.Add(new KeyValuePair<string, string>("to", item.CnFProfile.PhoneNumber));
                    //values.Add(new KeyValuePair<string, string>("message", "Dear Concern,Your assignment for Container " + item.ContainerNumber + " is enlisted.Please provide Your Truck Details at URL.Please note that your container will not be kept down until you provide necessary truck information.After that You will get a confirmation sms with time details when your container is kept down.  Powered by NeuroStorm Soft ."));

                    //var content = new FormUrlEncodedContent(values);
                    //var response = await client.PostAsync("http://api.greenweb.com.bd/api.php?", content);

                    //var responseString = await response.Content.ReadAsStringAsync();

                    //}

                    item.Status = 1;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAssignment(int assignmentId, int cnfId, string vessel, string impReg, string mlo, string contNo, int yardId, double size, double height, string lineNo, string dst, string remarks, string verifyNo, string exitNo, string assignmentType)
        {
            AssignmentItem item = new AssignmentItem();
            item.AssignmentId = assignmentId;
            item.CnFProfileId = cnfId;
            item.Vessel = vessel;
            item.ImpReg = impReg;
            item.MLO = mlo;
            item.ContainerNumber = contNo;
            item.YardId = yardId;
            item.Size = size;
            item.Height = height;
            item.LineNumber = lineNo;
            item.Dst = dst;
            item.Remarks = remarks;
            item.VerifyNumber = verifyNo;
            item.ExitNumber = exitNo;
            item.AssignmentType = assignmentType;
            item.CreationTime = DateTime.Now.Date;
            item.Status = 0;
            _context.Add(item);
            await _context.SaveChangesAsync();

            return Json(true);
        }

        [Authorize(Roles = "GateSergent, SuperAdmin")]
        public IActionResult GateTruckIndex()
        {
            return View();
        }

        [Authorize(Roles = "Yard, SuperAdmin")]
        public IActionResult YardAssignmentIndex()
        {
            return View();
        }

        [Authorize(Roles = "Yard, SuperAdmin")]
        public async Task<IActionResult> KeepDown(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentItem = await _context.AssignmentItems
                .Include(m => m.Assignment)
                .Include(m => m.CnFProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            assignmentItem.KeepDownTime = DateTime.Now;
            assignmentItem.Status = 3;

            _context.Update(assignmentItem);
            await _context.SaveChangesAsync();

            //IDictionary<string, string> values = new Dictionary<string, string>();
            //values.Add(new KeyValuePair<string, string>("token", "a380a7ad7b19fd20c6a161cb114ba228"));
            //values.Add(new KeyValuePair<string, string>("to", assignmentItem.CnFProfile.PhoneNumber));
            //values.Add(new KeyValuePair<string, string>("message", "Dear Concern,Your Container" + assignmentItem.ContainerNumber + " has been kept down.Please collect your container between " + assignmentItem.Assignment.DeliveryStart + " to " + assignmentItem.Assignment.DeliveryEnd + " , " + assignmentItem.Assignment.DeliveryDate.ToShortDateString() + " . Powered by NeuroStorm Soft ."));

            //var content = new FormUrlEncodedContent(values);
            //var response = await client.PostAsync("http://api.greenweb.com.bd/api.php?", content);

            //var responseString = await response.Content.ReadAsStringAsync();

            if (assignmentItem == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(YardAssignmentIndex));
        }

        [Authorize(Roles = "Yard, SuperAdmin")]
        public IActionResult YardKeptDownAssignments()
        {
            return View();
        }

        [Authorize(Roles = "Yard, SuperAdmin")]
        public async Task<IActionResult> YardDeliveryAssignments()
        {
            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var tempTruckEntities = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Include(a => a.AssignmentItem.Assignment)
                .Include(a => a.AssignmentItem.CnFProfile)
                .Include(a => a.AssignmentItem.Yard)
                .Where(a => a.Status == 2 && a.AssignmentItem.YardId == user.YardId)
                .ToListAsync();

            var truckEntities = tempTruckEntities.OrderBy(m => m.AssignmentItemId).ToList();
            ViewData["TruckEntities"] = truckEntities;

            return View();
        }

        [Authorize(Roles = "GateSergent, SuperAdmin")]
        public async Task<IActionResult> TodaysGateWiseAllTruck()
        {
            //var applicationDbContext = _context.Assignments.Include(a => a.AssignmentSlot);
            var assignments = await _context.Assignments.Include(a => a.AssignmentSlot).ToListAsync();

            //var newAssignemnt = await _context.Assignments.Where(a => a.Status == 1).FirstOrDefaultAsync();
            //bool prevAssignment = false;

            //if (newAssignemnt == null)
            //{
            //    prevAssignment = true;
            //}

            //ViewData["Test"] = prevAssignment;
            //return View(await applicationDbContext.ToListAsync());
            return View(assignments);
        }

        [Authorize(Roles = "GateSergent, SuperAdmin")]
        public async Task<IActionResult> ViewTrucksAll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            //var tempTruckEntities = await _context.TruckEntities
            //  .Include(a => a.AssignmentItem)
            //  .Include(a => a.AssignmentItem.Assignment)
            //  .Include(a => a.AssignmentItem.CnFProfile)
            //  .Include(a => a.AssignmentItem.Yard)
            //  .Include(a => a.AssignmentItem.Yard.Gate)
            //  .Where(a => (a.AssignmentItem.Status == 3 && a.AssignmentItem.AssignmentId == id && a.AssignmentItem.Assignment.DeliveryDate.Date == DateTime.Now.Date && a.AssignmentItem.Yard.GateId == user.GateId) || ((a.Status == 2 || a.Status == 3) && a.AssignmentItem.Yard.GateId == user.GateId))
            //  .ToListAsync();

            var truckEntities = await _context.TruckEntities
              .Include(a => a.AssignmentItem)
              .Include(a => a.AssignmentItem.Assignment)
              .Include(a => a.AssignmentItem.CnFProfile)
              .Include(a => a.AssignmentItem.Yard)
              .Include(a => a.AssignmentItem.Yard.Gate)
              .Where(a => a.AssignmentItem.AssignmentId == id && a.AssignmentItem.Yard.GateId == user.GateId)
              .OrderBy(m => m.AssignmentItemId)
              .ToListAsync();

            //var truckEntities = tempTruckEntities.OrderBy(m => m.AssignmentItemId).ToList();

            //ViewData["TruckEntities"] = truckEntities;
            ViewData["TruckEntities"] = truckEntities;
            return View();
        }

        [Authorize(Roles = "GateSergent, GateAdmin, SuperAdmin")]
        public async Task<IActionResult> TodaysGateWiseAllTruckAdmin()
        {
            var applicationDbContext = _context.Assignments
                .Include(a => a.AssignmentSlot)
                .Where(a => a.Date == DateTime.Today.Date);

            var newAssignemnt = await _context.Assignments
              .Where(a => a.Status == 1).FirstOrDefaultAsync();
            bool prevAssignment = false;

            if (newAssignemnt == null)
            {
                prevAssignment = true;
            }

            ViewData["Test"] = prevAssignment;
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "GateSergent, GateAdmin, SuperAdmin")]
        public async Task<IActionResult> ViewTrucksAllAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            var tempTruckEntities = await _context.TruckEntities
              .Include(a => a.AssignmentItem)
              .Include(a => a.AssignmentItem.Assignment)
              .Include(a => a.AssignmentItem.CnFProfile)
              .Include(a => a.AssignmentItem.Yard)
              .Include(a => a.AssignmentItem.Yard.Gate)
              .Where(a => (a.AssignmentItem.Status == 3 && a.AssignmentItem.AssignmentId == id && a.AssignmentItem.Assignment.DeliveryDate.Date == DateTime.Now.Date) || ((a.Status == 2 || a.Status == 3)))
              .ToListAsync();

            var truckEntities = tempTruckEntities.OrderBy(m => m.AssignmentItemId).ToList();
            ViewData["TruckEntities"] = truckEntities;
            return View();
        }

        [Authorize(Roles = "GateSergent, SuperAdmin")]
        public async Task<IActionResult> GateTruckOutIndex()
        {
            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var tempTruckEntities = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Include(a => a.AssignmentItem.Assignment)
                .Include(a => a.AssignmentItem.CnFProfile)
                .Include(a => a.AssignmentItem.Yard)
                .Include(a => a.AssignmentItem.Yard.Gate)
                .Where(a => (a.Status == 2 || a.Status == 3) && a.AssignmentItem.Yard.OutGateId == user.GateId)
                .ToListAsync();

            var truckEntities = tempTruckEntities.OrderBy(m => m.AssignmentItemId).ToList();
            ViewData["TruckEntities"] = truckEntities;

            return View();
        }

        [Authorize(Roles = "GateSergent, SuperAdmin")]
        public async Task<IActionResult> TruckEnter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var truckEntity = await _context.TruckEntities
               .Include(a => a.AssignmentItem)
               .Where(a => a.Id == id)
               .FirstOrDefaultAsync();
            truckEntity.EntryDate = DateTime.Now.Date;
            truckEntity.EntryTime = totalTime;
            truckEntity.Status = 2;

            _context.Update(truckEntity);
            await _context.SaveChangesAsync();
            var cnf = await _context.CnFProfiles.Where(m => m.Id == truckEntity.AssignmentItem.CnFProfileId).FirstOrDefaultAsync();
            cnf.Balance -= 70.00;
            _context.Update(cnf);
            await _context.SaveChangesAsync();

            Payment payment = new Payment();
            payment.CnFProfileId = cnf.Id;
            payment.Amount = 72.00;
            payment.Name = "Port Fee";
            payment.PaymentMethodId = 4;
            payment.PaymentTypeId = 2;
            payment.Date = DateTime.Now;
            _context.Add(payment);
            await _context.SaveChangesAsync();

            //Payment paymentSystem = new Payment();
            //paymentSystem.CnFProfileId = cnf.Id;
            //paymentSystem.Amount = 14.50;
            //paymentSystem.Name = "System Fee";
            //paymentSystem.PaymentMethodId = 4;
            //paymentSystem.PaymentTypeId = 2;
            //paymentSystem.Date = DateTime.Now; ;
            //_context.Add(paymentSystem);
            //await _context.SaveChangesAsync();
            //var cnf1 = await _context.CnFProfiles.Where(m => m.Id == truckEntity.AssignmentItem.CnFProfileId).FirstOrDefaultAsync();
            //cnf1.Balance -= 14.50;
            //_context.Update(cnf1);
            //await _context.SaveChangesAsync();

            if (truckEntity == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(GateTruckIndex));
        }

        [Authorize(Roles = "GateSergent, SuperAdmin")]
        public async Task<IActionResult> TruckExit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var truckEntity = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            truckEntity.EntryDate = DateTime.Now.Date;
            truckEntity.ExitTime = totalTime;
            truckEntity.Status = 4;

            _context.Update(truckEntity);
            await _context.SaveChangesAsync();

            var tempTrucks = await _context.TruckEntities.Where(m => m.AssignmentItemId == truckEntity.AssignmentItemId && m.Status != 4).ToListAsync();
            if (tempTrucks.Count == 0)
            {
                var assignmentItem = await _context.AssignmentItems
                .FirstOrDefaultAsync(m => m.Id == truckEntity.AssignmentItemId);
                assignmentItem.Status = 4;
                _context.Update(assignmentItem);
                await _context.SaveChangesAsync();
            }

            if (truckEntity == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(GateTruckOutIndex));
        }

        [Authorize(Roles = "Yard, SuperAdmin")]
        public async Task<IActionResult> YardLoad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var truckEntity = await _context.TruckEntities
                .FirstOrDefaultAsync(m => m.Id == id);
            truckEntity.EntryDate = DateTime.Now.Date;
            truckEntity.LoadingTime = totalTime;
            truckEntity.Status = 3;

            _context.Update(truckEntity);
            await _context.SaveChangesAsync();
            if (truckEntity == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(YardDeliveryAssignments));
        }

        [Authorize(Roles = "Yard, GateAdmin, HarbourAndMarine, Mechanical, TMOffice, SuperAdmin")]
        public async Task<IActionResult> TrucksInsideCPA()
        {
            var tempTruckEntities = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Include(a => a.AssignmentItem.Assignment)
                .Include(a => a.AssignmentItem.CnFProfile)
                .Include(a => a.AssignmentItem.Yard)
                .Where(a => a.Status == 2 || a.Status == 3)
                .ToListAsync();

            var truckEntities = tempTruckEntities.OrderBy(m => m.EntryTime).ToList();
            ViewData["TruckEntities"] = truckEntities;

            return View();
        }

        [Authorize(Roles = "GateAdmin, HarbourAndMarine, Mechanical, TMOffice, SuperAdmin")]
        public async Task<IActionResult> AllGateTruckIndex()
        {
            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            var tempTruckEntities = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Include(a => a.AssignmentItem.Assignment)
                .Include(a => a.AssignmentItem.CnFProfile)
                .Include(a => a.AssignmentItem.Yard)
                .Include(a => a.AssignmentItem.Yard.Gate)
                .Where(a => ((a.AssignmentItem.Status == 3 || a.AssignmentItem.Status == 4) && a.AssignmentItem.Assignment.DeliveryDate.Date == DateTime.Now.Date))
                .ToListAsync();

            var truckEntities = tempTruckEntities.OrderBy(m => m.AssignmentItemId).ToList();
            ViewData["TruckEntities"] = truckEntities;

            return View();
        }

        //[Authorize(Roles = "GateAdmin, HarbourAndMarine, Mechanical, TMOffice,OneStop, SuperAdmin")]
        //public async Task<IActionResult> AllAssignmentIndex(DateTime fromDate, DateTime toDate)
        //{
        //    if (fromDate == default(DateTime))
        //    {
        //        fromDate = DateTime.Now.Date;
        //    }

        //    if (toDate == default(DateTime))
        //    {
        //        toDate = DateTime.Now.Date;
        //    }

        //    var assignments = _context.Assignments
        //        .Include(a => a.AssignmentSlot)
        //        .Include(a => a.AssignmentItems)
        //        .ThenInclude(a => a.CnFProfile)
        //        .Where(m => (m.Date >= fromDate && m.Date <= toDate));
        //    ViewData["FromDate"] = fromDate.Date;
        //    ViewData["ToDate"] = toDate.Date;
        //    ViewData["Assignments"] = assignments;
        //    return View(await assignments.ToListAsync());
        //}

        //[HttpPost]
        //public IActionResult AllAssignmentIndex(FromDateToDateViewModel datesVM)
        //{
        //    return RedirectToAction("AllAssignmentIndex", new { id = datesVM.Id, fromDate = datesVM.FromDate, toDate = datesVM.ToDate });
        //}

        [Authorize(Roles = "GateAdmin, HarbourAndMarine, Mechanical, TMOffice, SuperAdmin")]
        public async Task<IActionResult> TodaysAllAssignments()
        {
            var assignments = _context.Assignments
                .Include(a => a.AssignmentSlot)
                .Include(a => a.AssignmentItems)
                .ThenInclude(a => a.CnFProfile)
                .Where(a => a.Date == DateTime.Now.Date);
            return View(await assignments.ToListAsync());
        }

        [Authorize(Roles = "GateSergent, SuperAdmin")]
        public async Task<IActionResult> LateTruckIndex()
        {
            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var tempTruckEntities = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Include(a => a.AssignmentItem.Assignment)
                .Include(a => a.AssignmentItem.CnFProfile)
                .Include(a => a.AssignmentItem.Yard)
                .Include(a => a.AssignmentItem.Yard.Gate)
                .Where(a => ((a.AssignmentItem.Status == 3 && a.Status == 1 && a.AssignmentItem.Yard.GateId == user.GateId) && (a.AssignmentItem.Assignment.DeliveryDate < DateTime.Now || (a.AssignmentItem.Assignment.DeliveryDate < DateTime.Now && a.AssignmentItem.Assignment.DeliveryEnd < totalTime))))
                .ToListAsync();

            var truckEntities = tempTruckEntities.OrderBy(m => m.AssignmentItemId).ToList();
            ViewData["TruckEntities"] = truckEntities;

            return View();
        }

        [Authorize(Roles = "GateSergent, SuperAdmin")]
        public async Task<IActionResult> LateTruckEnter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var truckEntity = await _context.TruckEntities
               .Include(a => a.AssignmentItem)
               .Where(a => a.Id == id)
               .FirstOrDefaultAsync();
            truckEntity.EntryDate = DateTime.Now;
            truckEntity.EntryTime = totalTime;
            truckEntity.Status = 2;

            _context.Update(truckEntity);
            await _context.SaveChangesAsync();

            var cnf = await _context.CnFProfiles.Where(m => m.Id == truckEntity.AssignmentItem.CnFProfileId).FirstOrDefaultAsync();
            cnf.Balance -= 57.50;
            _context.Update(cnf);
            await _context.SaveChangesAsync();

            Payment payment = new Payment();
            payment.CnFProfileId = cnf.Id;
            payment.Amount = 57.50;
            payment.Name = "Port Fee";
            payment.PaymentMethodId = 4;
            payment.PaymentTypeId = 2;
            payment.Date = DateTime.Now; ;
            _context.Add(payment);
            await _context.SaveChangesAsync();
            Payment paymentSystem = new Payment();
            paymentSystem.CnFProfileId = cnf.Id;
            paymentSystem.Amount = 14.50;
            paymentSystem.Name = "System Fee";
            paymentSystem.PaymentMethodId = 4;
            paymentSystem.PaymentTypeId = 2;
            paymentSystem.Date = DateTime.Now; ;
            _context.Add(paymentSystem);
            await _context.SaveChangesAsync();

            var cnf1 = await _context.CnFProfiles.Where(m => m.Id == truckEntity.AssignmentItem.CnFProfileId).FirstOrDefaultAsync();
            cnf1.Balance -= 14.50;
            _context.Update(cnf1);
            await _context.SaveChangesAsync();
            //Payment finePayment = new Payment();
            //finePayment.CnFProfileId = cnf.Id;
            //finePayment.Amount = 500;
            //finePayment.PaymentMethodId = 4;
            //finePayment.PaymentTypeId = 3;
            //finePayment.Date = DateTime.Now; ;
            //_context.Add(finePayment);
            //await _context.SaveChangesAsync();
            //cnf.Balance -= 500;
            _context.Update(cnf);
            await _context.SaveChangesAsync();

            if (truckEntity == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(GateTruckIndex));
        }

        [Authorize(Roles = "GateAdmin, HarbourAndMarine, Mechanical, TMOffice, SuperAdmin")]
        public async Task<IActionResult> AllGateTrucksOfAllTimeIndex()
        {
            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing

            TimeSpan time = DateTime.Now.TimeOfDay;
            int hour = time.Hours;
            int mins = time.Minutes;
            int totalTime = (hour * 100) + mins;

            var tempTruckEntities = await _context.TruckEntities
                .Include(a => a.AssignmentItem)
                .Include(a => a.AssignmentItem.Assignment)
                .Include(a => a.AssignmentItem.CnFProfile)
                .Include(a => a.AssignmentItem.Yard)
                .Include(a => a.AssignmentItem.Yard.Gate)
                .Where(a => (a.AssignmentItem.Status == 3 || a.AssignmentItem.Status == 4))
                .ToListAsync();

            var truckEntities = tempTruckEntities.OrderBy(m => m.AssignmentItemId).ToList();
            ViewData["TruckEntities"] = truckEntities;

            return View();
        }

        [Authorize(Roles = "Cnf, SuperAdmin")]
        public IActionResult AttachCnfFiles(int id)
        {
            var assignmentItem = _context.AssignmentItems
              .Where(a => a.Id == id).FirstOrDefault();

            ViewData["AssignmentItem"] = assignmentItem;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttachCnfFiles(AssignmentViewModel assignmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var tempAssignmentItem = await _context.AssignmentItems.Where(m => m.Id == assignmentViewModel.AssignmenItemtId).FirstOrDefaultAsync();
                string file1 = "No File";
                string file2 = "No File";
                string file3 = "No File";
                string file4 = "No File";
                string file5 = "No File";
                string file6 = "No File";
                if (tempAssignmentItem.ReleaseOrder != "" || tempAssignmentItem.ReleaseOrder != "No File")
                {
                    file1 = tempAssignmentItem.ReleaseOrder;
                }
                if (tempAssignmentItem.CartTicket != "" || tempAssignmentItem.CartTicket != "No File")
                {
                    file2 = tempAssignmentItem.CartTicket;
                }
                if (tempAssignmentItem.BillOfEntity != "" || tempAssignmentItem.BillOfEntity != "No File")
                {
                    file3 = tempAssignmentItem.BillOfEntity;
                }
                if (tempAssignmentItem.AssesmentNotice != "" || tempAssignmentItem.AssesmentNotice != "No File")
                {
                    file4 = tempAssignmentItem.AssesmentNotice;
                }
                if (tempAssignmentItem.OneStopBill != "" || tempAssignmentItem.OneStopBill != "No File")
                {
                    file5 = tempAssignmentItem.OneStopBill;
                }
                if (tempAssignmentItem.ContainerScanCopy != "" || tempAssignmentItem.ContainerScanCopy != "No File")
                {
                    file6 = tempAssignmentItem.ContainerScanCopy;
                }

                if (assignmentViewModel.ReleaseOrder != null)
                {
                    string uniqueFileName = null;
                    string stringCutted = assignmentViewModel.ReleaseOrder.FileName.Split('.').Last();
                    uniqueFileName = Guid.NewGuid().ToString() + "." + stringCutted;
                    file1 = uniqueFileName;
                    InputFile fileUpload = new InputFile(hostingEnvironment);
                    fileUpload.Uploadfile("files/cnfFiles", assignmentViewModel.ReleaseOrder, uniqueFileName);

                }
                if (assignmentViewModel.CartTicket != null)
                {
                    string uniqueFileName = null;
                    string stringCutted = assignmentViewModel.CartTicket.FileName.Split('.').Last();
                    uniqueFileName = Guid.NewGuid().ToString() + "." + stringCutted;
                    file2 = uniqueFileName;
                    InputFile fileUpload = new InputFile(hostingEnvironment);
                    fileUpload.Uploadfile("files/cnfFiles", assignmentViewModel.CartTicket, uniqueFileName);

                }
                if (assignmentViewModel.BillOfEntity != null)
                {
                    string uniqueFileName = null;
                    string stringCutted = assignmentViewModel.BillOfEntity.FileName.Split('.').Last();
                    uniqueFileName = Guid.NewGuid().ToString() + "." + stringCutted;
                    file3 = uniqueFileName;
                    InputFile fileUpload = new InputFile(hostingEnvironment);
                    fileUpload.Uploadfile("files/cnfFiles", assignmentViewModel.BillOfEntity, uniqueFileName);

                }
                if (assignmentViewModel.AssesmentNotice != null)
                {
                    string uniqueFileName = null;
                    string stringCutted = assignmentViewModel.AssesmentNotice.FileName.Split('.').Last();
                    uniqueFileName = Guid.NewGuid().ToString() + "." + stringCutted;
                    file4 = uniqueFileName;
                    InputFile fileUpload = new InputFile(hostingEnvironment);
                    fileUpload.Uploadfile("files/cnfFiles", assignmentViewModel.AssesmentNotice, uniqueFileName);

                }
                if (assignmentViewModel.OneStopBill != null)
                {
                    string uniqueFileName = null;
                    string stringCutted = assignmentViewModel.OneStopBill.FileName.Split('.').Last();
                    uniqueFileName = Guid.NewGuid().ToString() + "." + stringCutted;
                    file5 = uniqueFileName;
                    InputFile fileUpload = new InputFile(hostingEnvironment);
                    fileUpload.Uploadfile("files/cnfFiles", assignmentViewModel.OneStopBill, uniqueFileName);

                }
                if (assignmentViewModel.ContainerScanCopy != null)
                {
                    string uniqueFileName = null;
                    string stringCutted = assignmentViewModel.ContainerScanCopy.FileName.Split('.').Last();
                    uniqueFileName = Guid.NewGuid().ToString() + "." + stringCutted;
                    file6 = uniqueFileName;
                    InputFile fileUpload = new InputFile(hostingEnvironment);
                    fileUpload.Uploadfile("files/cnfFiles", assignmentViewModel.ContainerScanCopy, uniqueFileName);

                }
                var assignmentItem = await _context.AssignmentItems.Where(m => m.Id == assignmentViewModel.AssignmenItemtId).FirstOrDefaultAsync();
                assignmentItem.ReleaseOrder = file1;
                assignmentItem.CartTicket = file2;
                assignmentItem.BillOfEntity = file3;
                assignmentItem.AssesmentNotice = file4;
                assignmentItem.OneStopBill = file5;
                assignmentItem.ContainerScanCopy = file6;

                _context.Update(assignmentItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("CnfAssignments", "CnFProfiles");
            }

            return View(assignmentViewModel);
        }

        //GET: Assignments
        [Authorize(Roles = "GateAdmin, HarbourAndMarine, Mechanical, TMOffice, OneStop, SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = "OneStop, SuperAdmin")]
        //public IActionResult Index()
        //{
        //    if (fromDate == default(DateTime))
        //    {
        //        fromDate = DateTime.Now.Date;
        //    }
        //    if (toDate == default(DateTime))
        //    {
        //        toDate = DateTime.Now.Date;
        //    }
        //    var applicationDbContext = _context.Assignments.Include(a => a.AssignmentSlot).Where(m => (m.Date >= fromDate && m.Date <= toDate));

        //    ViewData["FromDate"] = fromDate.Date;
        //    ViewData["ToDate"] = toDate.Date;
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Index(FromDateToDateViewModel datesVM)
        //{
        //    return RedirectToAction("Index", new { fromDate = datesVM.FromDate, toDate = datesVM.ToDate });
        //}
    }
}
