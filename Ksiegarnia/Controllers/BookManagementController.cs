using Ksiegarnia.Data;
using Ksiegarnia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Ksiegarnia.Controllers
{
    public class BookManagementController : Controller
    {
        private readonly IBookRepository _bookRepository;

        public BookManagementController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [Authorize]
        public IActionResult Index()
        {
            var userEmail = User.Identity.Name;
            var books = _bookRepository.GetAllBooks();
            if (userEmail.Equals("admin@wp.pl", StringComparison.OrdinalIgnoreCase))
            {
                return View(books);
            }
            else
            {
                // Użytkownik nie ma dostępu
                return new ForbidResult(); // Możesz również użyć RedirectToPage("/AccessDenied")
            }
        }

        public IActionResult Add()
        {
            var userEmail = User.Identity.Name;
            if (userEmail.Equals("admin@wp.pl", StringComparison.OrdinalIgnoreCase))
            {
                // Użytkownik ma dostęp
                return View();
            }
            else
            {
                // Użytkownik nie ma dostępu
                return new ForbidResult(); // Możesz również użyć RedirectToPage("/AccessDenied")
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Add(Book book, IFormFile coverImage)
        {
            // Tworzenie nowej książki
            var newBook = new Book
            {
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Price = book.Price,
                CoverImage = null // Domyślna wartość dla CoverImage
            };
            if (ModelState.IsValid)
            {


                // Obsługa obrazu okładki
                if (coverImage != null)
                {
                    // Zapisz plik na serwerze i ustaw nazwę w bazie danych
                    // (pamiętaj o ścieżce zapisu, obsłudze błędów itp.)
                    string uniqueFileName = GetUniqueFileName(coverImage.FileName);
                    string filePath = Path.Combine("wwwroot", "images", uniqueFileName);

                    coverImage.CopyTo(new FileStream(filePath, FileMode.Create));
                    newBook.CoverImage = uniqueFileName;
                }


            }

            _bookRepository.AddBook(newBook);
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            var userEmail = User.Identity.Name;
            var book = _bookRepository.GetBookById(id);
            if (userEmail.Equals("admin@wp.pl", StringComparison.OrdinalIgnoreCase))
            {
                return View(book);
            }
            else
            {
                // Użytkownik nie ma dostępu
                return new ForbidResult(); // Możesz również użyć RedirectToPage("/AccessDenied")
            }
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book, IFormFile coverImage)
        {
            if (ModelState.IsValid)
            {
                // Obsługa obrazu okładki
                if (coverImage != null)
                {
                    // Zapisz plik na serwerze i ustaw nazwę w bazie danych
                    // (pamiętaj o ścieżce zapisu, obsłudze błędów itp.)
                    string uniqueFileName = GetUniqueFileName(coverImage.FileName);
                    string filePath = Path.Combine("wwwroot", "images", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        coverImage.CopyTo(stream);
                    }

                    book.CoverImage = uniqueFileName;
                    _bookRepository.UpdateBook(book);
                }

            }
            _bookRepository.UpdateBook(book);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var userEmail = User.Identity.Name;
            if (userEmail.Equals("admin@wp.pl", StringComparison.OrdinalIgnoreCase))
            {
                _bookRepository.DeleteBook(id);
                return RedirectToAction("Index");

            }
            else
            {
                // Użytkownik nie ma dostępu
                return new ForbidResult(); // Możesz również użyć RedirectToPage("/AccessDenied")
            }

        }

            private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }
    }
}
