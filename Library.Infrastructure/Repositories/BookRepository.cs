using Library.Core.Entities;
using Library.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDBContext _context;

        public BookRepository(LibraryDBContext context)
        {
            _context = context;
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void DeleteBook(Guid id)
        {
            var book = _context.Books.Find(id);
            if(book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public Book GetBook(Guid id)
        {
            return _context.Books.Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .FirstOrDefault(b => b.Id == id);
        }

        public List<Book> GetBooks()
        {
            return _context.Books.Include(b => b.BookAuthors).ThenInclude(ba => ba.Author).ToList();
        }

        public void UpdateBook(Guid id, string Title, string Description, int BorrowPrice, int Quantity, List<Guid> bookAuthorsIds)
        {
            var book = _context.Books.Include(b => b.BookAuthors).FirstOrDefault(b => b.Id == id);
            if (book == null) return;

            book.Title = Title;
            book.Description = Description;
            book.BorrowPrice = BorrowPrice;
            book.Quantity = Quantity;
            if (bookAuthorsIds == null)
            {
                _context.SaveChanges();
                return;
            }
            // delete all old authors
            _context.BookAuthors.RemoveRange(book.BookAuthors);

            foreach (var at in bookAuthorsIds)
            {
                BookAuthor newAuthor = new BookAuthor();
                newAuthor.BookId = book.Id;
                newAuthor.AuthorId = at;
                newAuthor.Book = book;
                newAuthor.Author = _context.Authors.Find(at);
                book.BookAuthors.Add(newAuthor);
            }
            _context.SaveChanges();
        }

    }
}
