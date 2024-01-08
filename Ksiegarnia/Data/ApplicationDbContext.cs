using Ksiegarnia.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Ksiegarnia.Data;

namespace Ksiegarnia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> User { get; set; }
        public new DbSet<Role> Roles { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int bookId);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int bookId);
        IEnumerable<Book> GetFavoriteBooks(int userId);
        void AddToFavorite(int userId, int bookId);
        void RemoveFromFavorite(int userId, int bookId);
    }
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.FirstOrDefault(b => b.BookId == bookId);
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void UpdateBook(Book book)
        {
            // Implementacja aktualizacji
            _context.SaveChanges();
        }

        public void DeleteBook(int bookId)
        {
            var bookToDelete = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (bookToDelete != null)
            {
                _context.Books.Remove(bookToDelete);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Book> GetFavoriteBooks(int userId)
        {
            return _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Book)
                .ToList();
        }

        public void AddToFavorite(int userId, int bookId)
        {
            var favorite = new Favorite { UserId = userId, BookId = bookId };
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
        }

        public void RemoveFromFavorite(int userId, int bookId)
        {
            var favorite = _context.Favorites
                .SingleOrDefault(f => f.UserId == userId && f.BookId == bookId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }
        }
    }

}