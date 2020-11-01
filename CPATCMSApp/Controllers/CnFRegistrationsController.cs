using System;
using System.Linq;
using CPATCMSApp.Data;
using CPATCMSApp.Models.CnFs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using jQueryDatatableServerSideNetCore22.Extensions;
using jQueryDatatableServerSideNetCore22.Models.AuxiliaryModels;

namespace CPATCMSApp.Controllers
{
    public class CnFRegistrationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CnFRegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CnFRegistrations
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: CnFRegistrations/Details/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cnFRegistration = await _context.CnFRegistrations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cnFRegistration == null)
            {
                return NotFound();
            }

            return View(cnFRegistration);
        }

        // GET: CnFRegistrations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CnFRegistrations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CnFName,CnFCode,CnFPhoneNumber,CnFAddress")] CnFRegistration cnFRegistration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cnFRegistration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Successful));
            }
            return View(cnFRegistration);
        }

        // GET: CnFRegistrations/Edit/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cnFRegistration = await _context.CnFRegistrations.FindAsync(id);
            if (cnFRegistration == null)
            {
                return NotFound();
            }
            return View(cnFRegistration);
        }

        // POST: CnFRegistrations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,CnFName,CnFCode,CnFPhoneNumber,CnFAddress")] CnFRegistration cnFRegistration)
        {
            if (id != cnFRegistration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cnFRegistration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CnFRegistrationExists(cnFRegistration.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cnFRegistration);
        }

        // GET: CnFRegistrations/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cnFRegistration = await _context.CnFRegistrations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cnFRegistration == null)
            {
                return NotFound();
            }

            return View(cnFRegistration);
        }

        // POST: CnFRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cnFRegistration = await _context.CnFRegistrations.FindAsync(id);
            _context.CnFRegistrations.Remove(cnFRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CnFRegistrationExists(string id)
        {
            return _context.CnFRegistrations.Any(e => e.Id == id);
        }

        public IActionResult Successful()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
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
            var result = await _context.CnFRegistrations.ToListAsync();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r =>
                r.CnFName != null && r.CnFName.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CnFCode != null && r.CnFCode.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CnFPhoneNumber != null && r.CnFPhoneNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                r.CnFAddress != null && r.CnFAddress.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            var filteredResultsCount = result.Count();

            //here
            var totalResultsCount = await _context.CnFRegistrations.CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = result
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
                    data = result
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }
    }
}
