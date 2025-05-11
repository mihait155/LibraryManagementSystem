using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class BookLoan
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public string PersonName { get; set; }
        
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BookLoan() { }
        public BookLoan(Book book, string personName, DateTime borrowDate, DateTime returnDate)
        {
            Book = book;
            PersonName = personName;
            BorrowDate = borrowDate;
            ReturnDate = returnDate;
        }
        public override string ToString()
        {
            return "Loan: Person: " + PersonName + "; Book: " + Book.ToString() + "; Borrow Date: " + BorrowDate.ToString()
                + "; Return Date: " + ReturnDate.ToString() + "; Loan ID: " + Id.ToString();
        }
    }
}
