using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Interfaces
{
    public interface IBookRepository
    {
        List<Book> GetBooks();
        Book GetBook(Guid id);
        void AddBook(Book book);
        void UpdateBook(Guid id, string Title, string Description, int BorrowPrice, int Quantity, List<Guid> bookAuthorsIds);
        void DeleteBook(Guid id);

        
    }
}
