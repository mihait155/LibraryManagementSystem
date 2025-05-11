using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Interfaces
{
    public interface ILoanRepository
    {
        List<BookLoan> GetBookLoans();
        BookLoan GetBookLoan(Guid id);
        void AddBookLoan(BookLoan bookLoan);
        void UpdateBookLoan(Guid id, BookLoan updatedBookLoan);
        void DeleteBookLoan(Guid id);
    }
}
