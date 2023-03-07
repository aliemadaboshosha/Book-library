using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab7.Data;
using lab7.Models;

namespace lab7.Controllers
{
    public class PriceOffersController : Controller
    {
        private readonly BookDbContext _context;

        public PriceOffersController(BookDbContext context)
        {
            _context = context;
        }

        // GET: PriceOffers
        public IActionResult Index()
        {
            var bookDbContext = _context.PriceOffers.Include(p => p.Book);
            return View( bookDbContext.ToList());
        }

        // GET: PriceOffers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.PriceOffers == null)
            {
                return NotFound();
            }

            var priceOffer = _context.PriceOffers
                .Include(p => p.Book)
                .FirstOrDefaultAsync(m => m.PriceOfferId == id);
            if (priceOffer == null)
            {
                return NotFound();
            }

            return View(priceOffer);
        }

        // GET: PriceOffers/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            return View();
        }

        // POST: PriceOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( PriceOffer priceOffer)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == priceOffer.BookId);
            priceOffer.Book = book;
            if (ModelState.IsValid)
            {var check =_context.PriceOffers.Count(o=>o.BookId==priceOffer.BookId);
                if (check>0)
                {
                    ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", priceOffer.BookId);
                    return View(priceOffer);

                }
                

                _context.Add(priceOffer);
                 _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", priceOffer.BookId);
            return View(priceOffer);
        }

        // GET: PriceOffers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PriceOffers == null)
            {
                return NotFound();
            }

            var priceOffer = await _context.PriceOffers.FindAsync(id);
            if (priceOffer == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", priceOffer.BookId);
            return View(priceOffer);
        }

        // POST: PriceOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriceOfferId,NewPrice,OfferTxt,BookId")] PriceOffer priceOffer)
        {
            if (id != priceOffer.PriceOfferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priceOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceOfferExists(priceOffer.PriceOfferId))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", priceOffer.BookId);
            return View(priceOffer);
        }

        // GET: PriceOffers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PriceOffers == null)
            {
                return NotFound();
            }

            var priceOffer = await _context.PriceOffers
                .Include(p => p.Book)
                .FirstOrDefaultAsync(m => m.PriceOfferId == id);
            if (priceOffer == null)
            {
                return NotFound();
            }

            return View(priceOffer);
        }

        // POST: PriceOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PriceOffers == null)
            {
                return Problem("Entity set 'BookDbContext.PriceOffers'  is null.");
            }
            var priceOffer = await _context.PriceOffers.FindAsync(id);
            if (priceOffer != null)
            {
                _context.PriceOffers.Remove(priceOffer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceOfferExists(int id)
        {
          return _context.PriceOffers.Any(e => e.PriceOfferId == id);
        }
    }
}
