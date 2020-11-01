using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPATCMSApp.Data;
using CPATCMSApp.Models.Currency;
using CPATCMSApp.Models.Common.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using CPATCMSApp.Models.ViewModels;
using jQueryDatatableServerSideNetCore22.Models.AuxiliaryModels;
using jQueryDatatableServerSideNetCore22.Extensions;

namespace CPATCMSApp.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PaymentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Payments
        [Authorize(Roles = "Admin, HarbourAndMarine , TMOffice, SuperAdmin")]
        public IActionResult RechargeIndex()
        {
            return View();
        }

        [Authorize(Roles = "Admin, HarbourAndMarine , TMOffice, SuperAdmin")]
        public async Task<IActionResult> RevenueIndex(DateTime fromDate, DateTime toDate)
        {

            if (fromDate == default(DateTime))
            {
                fromDate = DateTime.Now.Date;
            }
            if (toDate == default(DateTime))
            {
                toDate = DateTime.Now.Date;
            }
            var applicationDbContext = _context.Payments.Include(p => p.CnFProfile).Include(p => p.PaymentMethod).Include(p => p.PaymentType)
                .Where(m => m.PaymentTypeId == 2 && m.Date >= fromDate && m.Date <= toDate);
            ViewData["Revenues"] = await applicationDbContext.ToListAsync();
            ViewData["FromDate"] = fromDate.Date;
            ViewData["ToDate"] = toDate.Date;
            return View();
        }

        [HttpPost]
        public IActionResult RevenueIndex(FromDateToDateViewModel datesVM)
        {
            return RedirectToAction("RevenueIndex", new { id = datesVM.Id, fromDate = datesVM.FromDate, toDate = datesVM.ToDate });
        }


        [Authorize(Roles = "Admin, HarbourAndMarine , TMOffice, SuperAdmin")]
        public IActionResult FineIndex()
        {
            return View();
        }



        [Authorize(Roles = "Admin, HarbourAndMarine , TMOffice, SuperAdmin")]
        public IActionResult PaymentHistory()
        {
            return View();
        }
        [Authorize(Roles = "Cnf, SuperAdmin")]
        public async Task<IActionResult> UserPaymentHistory()
        {
            var user = (await _userManager.FindByNameAsync(HttpContext.User.Identity.Name)); //same thing
            var cnf = await _context.CnFProfiles.Where(m => m.ApplicationUserId == user.Id).FirstOrDefaultAsync();
            var applicationDbContext = _context.Payments.Include(p => p.CnFProfile).Include(p => p.PaymentMethod).Include(p => p.PaymentType)
                .Where(m => m.CnFProfileId==cnf.Id);
            ViewData["Cnf"] = cnf;
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["CnFProfileId"] = new SelectList(_context.CnFProfiles, "Id", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethod.Where(m=>m.Id!=4), "Id", "Name");
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            payment.PaymentTypeId = 1;
            payment.Date = DateTime.Now;

            var cnf = await _context.CnFProfiles.Where(m => m.Id == payment.CnFProfileId).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();

                cnf.Balance += payment.Amount;
                _context.Update(cnf);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PaymentHistory));
            }
            ViewData["CnFProfileId"] = new SelectList(_context.CnFProfiles, "Id", "Name", payment.CnFProfileId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethod.Where(m => m.Id != 4), "Id", "Name", payment.PaymentMethodId);
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name", payment.PaymentTypeId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["CnFProfileId"] = new SelectList(_context.CnFProfiles, "Id", "Name", payment.CnFProfileId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethod.Where(m => m.Id != 4), "Id", "Name", payment.PaymentMethodId);
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name", payment.PaymentTypeId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(PaymentHistory));
            }
            ViewData["CnFProfileId"] = new SelectList(_context.CnFProfiles, "Id", "Name", payment.CnFProfileId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethod.Where(m => m.Id != 4), "Id", "Name", payment.PaymentMethodId);
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name", payment.PaymentTypeId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.CnFProfile)
                .Include(p => p.PaymentMethod)
                .Include(p => p.PaymentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PaymentHistory));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody]DTParameters dtParameters)
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

            var result = await _context.Payments.Include(p => p.CnFProfile).Include(p => p.PaymentMethod).Include(p => p.PaymentType)
                .Where(m => m.PaymentTypeId == 1).ToListAsync();

            List<Payment> payments = new List<Payment>();

            foreach (var item in result)
            {
                Payment tempPayment = new Payment
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    Date = item.Date,
                    VerificationCode = item.VerificationCode,
                    ReferenceCode = item.ReferenceCode,
                    CnFProfileName = item.CnFProfile.Name,
                    PaymentMethodName = item.PaymentMethod.Name,
                    PaymentMethodId = item.PaymentMethodId,
                };
                payments.Add(tempPayment);
            }



            if (!string.IsNullOrEmpty(searchBy))
            {
                payments = payments.Where(r => 
                r.CnFProfileName != null && r.CnFProfileName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.PaymentMethodName != null && r.PaymentMethodName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Date != null && r.Date.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.Amount.ToString().Contains(searchBy.ToUpper()) ||
                r.VerificationCode != null && r.VerificationCode.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ReferenceCode != null && r.ReferenceCode.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            payments = orderAscendingDirection ? payments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : payments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.Payments.Include(p => p.CnFProfile).Include(p => p.PaymentMethod).Include(p => p.PaymentType)
                .Where(m => m.PaymentTypeId == 1).CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = payments
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
                    data = payments
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

            var result = await _context.Payments.Include(p => p.CnFProfile).Include(p => p.PaymentMethod).Include(p => p.PaymentType)
                .Where(m => m.PaymentTypeId == 3).ToListAsync();

            List<Payment> payments = new List<Payment>();

            foreach (var item in result)
            {
                Payment tempPayment = new Payment
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    Date = item.Date,
                    VerificationCode = item.VerificationCode,
                    ReferenceCode = item.ReferenceCode,
                    CnFProfileName = item.CnFProfile.Name,
                    PaymentMethodName = item.PaymentMethod.Name,
                    PaymentMethodId = item.PaymentMethodId,
                };
                payments.Add(tempPayment);
            }



            if (!string.IsNullOrEmpty(searchBy))
            {
                payments = payments.Where(r =>
                r.CnFProfileName != null && r.CnFProfileName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.PaymentMethodName != null && r.PaymentMethodName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Date != null && r.Date.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.Amount.ToString().Contains(searchBy.ToUpper()) ||
                r.VerificationCode != null && r.VerificationCode.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ReferenceCode != null && r.ReferenceCode.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            payments = orderAscendingDirection ? payments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : payments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.Payments.Include(p => p.CnFProfile).Include(p => p.PaymentMethod).Include(p => p.PaymentType)
                .Where(m => m.PaymentTypeId == 3).CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = payments
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
                    data = payments
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable3([FromBody]DTParameters dtParameters)
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

            var result = await _context.Payments.Include(p => p.CnFProfile).Include(p => p.PaymentMethod).Include(p => p.PaymentType).ToListAsync();

            List<Payment> payments = new List<Payment>();

            foreach (var item in result)
            {
                Payment tempPayment = new Payment
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    Date = item.Date,
                    VerificationCode = item.VerificationCode,
                    ReferenceCode = item.ReferenceCode,
                    CnFProfileName = item.CnFProfile.Name,
                    PaymentMethodName = item.PaymentMethod.Name,
                    PaymentMethodId = item.PaymentMethodId,
                    PaymentTypeName = item.PaymentType.Name,
                };
                payments.Add(tempPayment);
            }



            if (!string.IsNullOrEmpty(searchBy))
            {
                payments = payments.Where(r =>
                r.CnFProfileName != null && r.CnFProfileName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.PaymentMethodName != null && r.PaymentMethodName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Date != null && r.Date.ToString("dd-MM-yyyy").Contains(searchBy.ToString()) ||
                r.Amount.ToString().Contains(searchBy.ToUpper()) ||
                r.VerificationCode != null && r.VerificationCode.ToUpper().Contains(searchBy.ToUpper()) ||
                r.PaymentTypeName != null && r.PaymentTypeName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.ReferenceCode != null && r.ReferenceCode.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            payments = orderAscendingDirection ? payments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : payments.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.Payments.Include(p => p.CnFProfile).Include(p => p.PaymentMethod).Include(p => p.PaymentType).CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = payments
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
                    data = payments
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }
    }
}
