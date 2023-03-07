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
    public class BooksController : Controller
    {
        private readonly BookDbContext _context;

        public BooksController(BookDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {

              return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {


            if (id == null || _context.Books == null)
            {
                return NotFound();
            }
            var testBook = _context.PriceOffers.Where(x => x.BookId == id).ToArray();
            
            if (testBook.Length==0)
            {
                var book = await _context.Books
                   .Include(book => book.Reviews).Include(book => book.Tags).Include(book => book.Authors).ThenInclude(book => book.Author)
                   .FirstOrDefaultAsync(m => m.Id == id);
                PriceOffer nullOffer = new PriceOffer() { NewPrice = book.Price };
                book.Offer = nullOffer;

                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
            else
            {
                var book = await _context.Books
                .Include(book => book.Reviews).Include(book => book.Tags).Include(book => book.Offer).Include(book => book.Authors).ThenInclude(book => book.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

                if (book == null)
                {
                    return NotFound();
                }

                return View(book);
            }
            #region old version 

            //if (testBook.Length > 0)
            //{


            //    if (id == null || _context.Books == null)
            //    {
            //        return NotFound();
            //    }
            //    var book = await _context.Books
            //        .Include(book => book.Reviews).Include(book => book.Tags).Include(book => book.Offer).Include(book=>book.Authors).ThenInclude(book=>book.Author)
            //        .FirstOrDefaultAsync(m => m.Id == id);

            //    #region authors
            //    //var authors = _context.Book_Authors.Where(x => x.Book_id == id).OrderBy(x => x.Order).ToArray();
            //    //var authorsNames = new List<Author>();
            //    //foreach (var item in authors)
            //    //{
            //    //    var name = _context.Authors.First(x => x.Id == item.Author_id);
            //    //    authorsNames.Add(name);

            //    //}
            //    //ViewBag.authors = authorsNames;
            //    #endregion

            //    if (book == null)
            //    {
            //        return NotFound();
            //    }

            //    return View(book);
            //}
            //else
            //{


            //    if (id == null || _context.Books == null)
            //    {
            //        return NotFound();
            //    }
            //    var book = await _context.Books
            //        .Include(book => book.Reviews).Include(book => book.Tags)
            //        .FirstOrDefaultAsync(m => m.Id == id);
            //    PriceOffer nullOffer = new PriceOffer() { NewPrice = book.Price };
            //    book.Offer = nullOffer;

            //    #region authors
            //    var authors = _context.Book_Authors.Where(x => x.Book_id == id).OrderBy(x => x.Order).ToArray();
            //    var authorsNames = new List<Author>();
            //    foreach (var item in authors)
            //    {
            //        var name = _context.Authors.First(x => x.Id == item.Author_id);
            //        authorsNames.Add(name);

            //    }
            //    ViewBag.authors = authorsNames;
            //    #endregion

            //    if (book == null)
            //    {
            //        return NotFound();
            //    }

            //    return View(book);
            //}
            #endregion
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["Tages"] = new SelectList(_context.Tags, "ID", "Name");
            ViewBag.Authors = new SelectList(_context.Authors, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Publish_on,Publisher,Price,Authors,ImageURL,Tages")] Book book, int[] Authors, int[] Tags,IFormFile bookimg)
        {
            List<Tag> booktags=new List<Tag>();

            List<Book_Authors> bookauthors = new List<Book_Authors>();

            if (ModelState.IsValid)
            {
                string imageName = book.Title+ "." + (bookimg.FileName.Split(".").Last());
                using(var fs=System.IO.File.Create("wwwroot/images/"+imageName.ToString()))
                {
                    book.ImageURL = imageName;
                    bookimg.CopyTo(fs);
                }

                for (int item = 0; item < Authors.Length; item++)
                {
                    var checkAuthors = _context.Authors.FirstOrDefault(x => x.Id == Authors[item]);
                    Book_Authors book_Authors = new Book_Authors();
                    book_Authors.Author_id = checkAuthors.Id;
                    book_Authors.Book_id = book.Id;
                    book_Authors.Order = item + 1;
                    bookauthors.Add(book_Authors);

                }
                book.Authors = bookauthors;
                foreach (var item in Tags)
                {
                    booktags.Add(_context.Tags.First(x => x.ID == item));
                }
                book.Tags = booktags;

                _context.Add(book);
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,string Title,string Description,DateTime Publish_on,string Publisher,int Price)
        {
            Book book=new Book() { Id=id,Title=Title,Description=Description, Publisher=Publisher, Price=Price};

            if (id != book.Id)
            {
                return NotFound();
            }
            var originalBook = await _context.Books
                   .Include(book => book.Reviews).Include(book => book.Tags).Include(book => book.Authors).ThenInclude(book => book.Author)
                   .FirstOrDefaultAsync(m => m.Id == id);
            book.Authors = originalBook.Authors;
            book.Tags = originalBook.Tags;
            book.ImageURL = originalBook.ImageURL;
            

            if (ModelState.IsValid)
            {
                try
                {
                    
                    originalBook.Title = book.Title;
                    originalBook.Description = book.Description;
                    originalBook.Publish_on = book.Publish_on;
                    originalBook.Publisher = book.Publisher;
                    originalBook.Price=book.Price;
                    //_context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return _context.Books.Any(e => e.Id == id);
        }
    }
}
