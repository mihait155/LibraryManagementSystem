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
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryDBContext _context;

        public LoanRepository(LibraryDBContext dbContext)
        {
            _context = dbContext;
        }

        public void AddBookLoan(BookLoan bookLoan)
        {
            _context.BookLends.Add(bookLoan);
            _context.SaveChanges();
        }

        public void DeleteBookLoan(Guid id)
        {
            var bookLoan = _context.BookLends.Find(id);
            if (bookLoan != null)
            {
                _context.BookLends.Remove(bookLoan);
                _context.SaveChanges();
            }
        }

        public BookLoan GetBookLoan(Guid id)
        {
            return _context.BookLends.Include(bl => bl.Book).FirstOrDefault(bl => bl.Id == id);
        }

        public List<BookLoan> GetBookLoans()
        {
            return _context.BookLends.Include(bl => bl.Book).ToList();
        }

        public void UpdateBookLoan(Guid id, BookLoan updatedBookLoan)
        {
            throw new NotImplementedException();
        }
    }
}
