using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPATCMSApp.Data;
using CPATCMSApp.Models.Yards;
using jQueryDatatableServerSideNetCore22.Models.AuxiliaryModels;
using jQueryDatatableServerSideNetCore22.Extensions;
using CPATCMSApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CPATCMSApp.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class YardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public YardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Yards
        public IActionResult Index()
        {
            return View();
        }

        // GET: Yards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yard = await _context.Yards
                .Include(y => y.Bay)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yard == null)
            {
                return NotFound();
            }

            return View(yard);
        }

        // GET: Yards/Create
        public IActionResult Create()
        {
            ViewData["BayId"] = new SelectList(_context.Bays, "Id", "Name");
            return View();
        }

        // POST: Yards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Number,BayId")] Yard yard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BayId"] = new SelectList(_context.Bays, "Id", "Name", yard.BayId);
            return View(yard);
        }

        // GET: Yards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yard = await _context.Yards.FindAsync(id);
            if (yard == null)
            {
                return NotFound();
            }
            ViewData["BayId"] = new SelectList(_context.Bays, "Id", "Name", yard.BayId);
            return View(yard);
        }

        // POST: Yards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Number,BayId")] Yard yard)
        {
            if (id != yard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YardExists(yard.Id))
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
            ViewData["BayId"] = new SelectList(_context.Bays, "Id", "Name", yard.BayId);
            return View(yard);
        }

        // GET: Yards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yard = await _context.Yards
                .Include(y => y.Bay)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yard == null)
            {
                return NotFound();
            }

            return View(yard);
        }

        // POST: Yards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var yard = await _context.Yards.FindAsync(id);
            _context.Yards.Remove(yard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YardExists(int id)
        {
            return _context.Yards.Any(e => e.Id == id);
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

            var result = await _context.Yards.Include(m => m.Bay).ToListAsync();

            List<Yard> yards = new List<Yard>();

            foreach (var item in result)
            {
                Yard tempYard = new Yard
                {
                    Name = item.Name,
                    Number = item.Number,
                    Id = item.Id,
                    BayName = item.Bay.Name
                };
                yards.Add(tempYard);
            }



            if (!string.IsNullOrEmpty(searchBy))
            {
                yards = yards.Where(r => 
                r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                r.Number != null && r.Number.ToUpper().Contains(searchBy.ToUpper()) ||
                r.BayName != null && r.BayName.ToUpper().Contains(searchBy.ToUpper())
                ).ToList();
            }

            yards = orderAscendingDirection ? yards.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : yards.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.Yards.Include(m => m.Bay).CountAsync();
            if (dtParameters.Length == -1)
            {
                return Json(new
                {
                    draw = dtParameters.Draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = yards
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
                    data = yards
                  .Skip(dtParameters.Start)
                  .Take(dtParameters.Length)
                  .ToList()
                });
            }
        }
    }
}
