using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPATCMSApp.Data;
using CPATCMSApp.Models.Yards;
using CPATCMSApp.Models.Currency;
using Microsoft.AspNetCore.Authorization;
using jQueryDatatableServerSideNetCore22.Models.AuxiliaryModels;
using jQueryDatatableServerSideNetCore22.Extensions;

namespace CPATCMSApp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class BaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bays
        public IActionResult Index()
        {
            return View();
        }

        // GET: Bays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bay = await _context.Bays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bay == null)
            {
                return NotFound();
            }

            return View(bay);
        }

        // GET: Bays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Bay bay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bay);
        }

        // GET: Bays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bay = await _context.Bays.FindAsync(id);
            if (bay == null)
            {
                return NotFound();
            }
            return View(bay);
        }

        // POST: Bays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Bay bay)
        {
            if (id != bay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BayExists(bay.Id))
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
            return View(bay);
        }

        // GET: Bays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bay = await _context.Bays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bay == null)
            {
                return NotFound();
            }

            return View(bay);
        }

        // POST: Bays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bay = await _context.Bays.FindAsync(id);
            _context.Bays.Remove(bay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BayExists(int id)
        {
            return _context.Bays.Any(e => e.Id == id);
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
            var result = await _context.Bays.ToListAsync();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r =>
                r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            var filteredResultsCount = result.Count();

            //here
            var totalResultsCount = await _context.Bays.CountAsync();
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