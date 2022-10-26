using Library.Data;
using Library.Data.Entities;
using Library.Models.BookModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly LibraryDbContext context;

        public BooksController(LibraryDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var allBooks = await context.Books
                .Select(x => new BooksViewModel 
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    Rating = x.Rating,
                    Category = x.Category.Name
                })
                .ToListAsync();
            
            return View(allBooks);
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddBookViewModel()
            {
                Categories = await context.Categories.ToListAsync()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var book = new Book 
                {
                    Author = model.Author,
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl= model.ImageUrl,
                    CategoryId = model.CategoryId,
                    Rating = model.Rating,
                };

                await context.Books.AddAsync(book);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error while adding book");

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await context.Users
                .Where(x => x.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .ThenInclude(au => au.Book)
                .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user");
            }

            var books = user.ApplicationUsersBooks
                .Select(x => new BooksViewModel
                {
                    Id = x.BookId,
                    Title = x.Book.Title,
                    Description = x.Book.Description,
                    Author = x.Book.Author,
                    Category = x.Book.Category.Name,
                    Rating = x.Book.Rating,
                    ImageUrl = x.Book.ImageUrl,
                })
                .ToList();

            return View(books);
        }

        public async Task<IActionResult> AddToCollection(int bookId)
        { 
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user");
            }

            var book = await context.Books.FindAsync(bookId);

            if (book == null)
            {
                throw new ArgumentException("Invalid book");
            }

            if (!user.ApplicationUsersBooks.Any(b => b.BookId == bookId))
            {
                user.ApplicationUsersBooks.Add(new ApplicationUserBook
                {
                    Book = book,
                    BookId = bookId,
                    ApplicationUser = user,
                    ApplicationUserId = user.Id
                });

                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user");
            }

            var book = user.ApplicationUsersBooks.FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                user.ApplicationUsersBooks.Remove(book);

                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Mine));
        }
    }
}
