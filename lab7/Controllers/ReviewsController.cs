using lab7.Data;
using lab7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace lab7.Controllers
{
    public class ReviewsController : Controller
    {
        BookDbContext _context;

        public ReviewsController(BookDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public IActionResult Index()
        {
            return View(_context.Reviews.Include(Review=>Review.Book));
        }

        // GET: Reviews/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }
            var review = _context.Reviews.Include(Review => Review.Book)
                                .FirstOrDefault(m => m.Id == id);

            if (review==null)
            {
                return NotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewBag.Books = new SelectList(_context.Books, "Id", "Title");
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {if(ModelState.IsValid) {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            

            
                return View(review);
            
        }

        // GET: Reviews/Delete/5
        public IActionResult Delete(int id)
        {
            var review =  _context.Reviews
                .FirstOrDefault(m => m.Id== id);
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfiremed(int id)
        {

            if (_context.Reviews==null)
            {
                return NotFound();
            }
            var review= _context.Reviews.FirstOrDefault(m => m.Id== id);

            if (review !=null)
            {
                _context.Reviews.Remove(review);

            }
                _context.SaveChanges();
            return RedirectToAction("Index");
            
        }
    }
}
