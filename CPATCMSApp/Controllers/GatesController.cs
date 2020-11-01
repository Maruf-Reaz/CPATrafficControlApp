using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPATCMSApp.Data;
using CPATCMSApp.Models.Gates;
using Microsoft.AspNetCore.Authorization;
using jQueryDatatableServerSideNetCore22.Models.AuxiliaryModels;
using jQueryDatatableServerSideNetCore22.Extensions;

namespace CPATCMSApp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class GatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gates
        public IActionResult Index()
        {
            return View();
        }

        // GET: Gates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gate = await _context.Gates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gate == null)
            {
                return NotFound();
            }

            return View(gate);
        }

        // GET: Gates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Gate gate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gate);
        }

        // GET: Gates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gate = await _context.Gates.FindAsync(id);
            if (gate == null)
            {
                return NotFound();
            }
            return View(gate);
        }

        // POST: Gates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Gate gate)
        {
            if (id != gate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GateExists(gate.Id))
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
            return View(gate);
        }

        // GET: Gates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gate = await _context.Gates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gate == null)
            {
                return NotFound();
            }

            return View(gate);
        }

        // POST: Gates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gate = await _context.Gates.FindAsync(id);
            _context.Gates.Remove(gate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GateExists(int id)
        {
            return _context.Gates.Any(e => e.Id == id);
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
            var result = await _context.Gates.ToListAsync();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r =>
                r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            var filteredResultsCount = result.Count();

            //here
            var totalResultsCount = await _context.Gates.CountAsync();
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
