using Library.Core.Entities;
using Library.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ConsoleApp.Service
{
    public class Service
    {
        private BookRepository bookRepository;
        private AuthorRepository authorRepository;
        private LoanRepository loanRepository;
        public Service(BookRepository bookRepo, AuthorRepository authorRepo, LoanRepository loanRepo) {
            bookRepository = bookRepo; 
            authorRepository = authorRepo;
            loanRepository = loanRepo;
        }

        public void PrintBooks()
        {
            var books = bookRepository.GetBooks();
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        public void AddBook(string title, int quantity, string description, int borrowPrice, List<string> authorNames)
        {
            Book book = new Book();
            book.Id = Guid.NewGuid();
            book.Title = title;
            book.Quantity = quantity;
            book.Description = description;
            book.BorrowPrice = borrowPrice;
            List<BookAuthor> bookAuthors = new List<BookAuthor>();

            // Add Authors, Then Add Books
            foreach(var authorname in authorNames)
            {
                var author_entity = authorRepository.GetAuthorByName(authorname);
                if(author_entity == null) 
                {
                    Author author = new Author();
                    author.Id = Guid.NewGuid();
                    author.Name = authorname;
                    authorRepository.AddAuthor(author);
                    BookAuthor ba = new BookAuthor();
                    ba.Author = author;
                    ba.Book = book;
                    ba.AuthorId = author.Id;
                    ba.BookId = book.Id;
                    bookAuthors.Add(ba);
                }
                else 
                {
                    BookAuthor ba = new BookAuthor();
                    ba.Author = author_entity;
                    ba.AuthorId = author_entity.Id;
                    ba.BookId = book.Id;
                    ba.Book = book;
                    bookAuthors.Add(ba);
                }
            }
            book.BookAuthors = bookAuthors;
            bookRepository.AddBook(book);
        }
        
        public List<string> GetAllBooks()
        {
            List<string> strings = new List<string>();
            List<Book> books = bookRepository.GetBooks();
            foreach(Book book in books)
            {
                string str = book.ToString();
                strings.Add(str);
            }
            return strings;
        }

        public List<string> GetAllAuthors()
        {
            List<string> strings = new List<string>();
            List<Author> authors = authorRepository.GetAuthors();
            foreach(Author author in authors)
            {
                string str = author.ToString();
                strings.Add(str);
            }
            return strings;

        }

        public List<string> SearchBookByTitle(string title)
        {
            List<Book> books = bookRepository.GetBooks();
            List<string> found = new List<string>();
            foreach(Book book in books)
            {
                if(book.Title == title || book.Title.Contains(title))
                {
                    found.Add(book.ToString());
                }
            }
            return found;
        }

        public List<string> SearchBookByAuthor(string author_name)
        {
            List<Book> books = bookRepository.GetBooks();
            List<string> found = new List<string>();
            foreach (Book book in books)
            {
                foreach(BookAuthor ba in book.BookAuthors)
                {
                    if(ba.Author.Name == author_name || ba.Author.Name.Contains(author_name) && !found.Contains(ba.Author.ToString()))
                    {
                        found.Add(ba.Author.ToString());
                    }
                }
            }
            return found;
        }
        public void DeleteBook(Guid id)
        {
            bookRepository.DeleteBook(id);
        }

        public void UpdateBook(Guid id, string Title, string Description, int BorrowPrice, int Quantity, List<Guid> authorIds)
        {
            bookRepository.UpdateBook(id, Title, Description, BorrowPrice, Quantity, authorIds);
        }

        public void AddBookLoan(Guid bookId, string personName, DateTime borrowDate, DateTime returnDate)
        {
            Book book = bookRepository.GetBook(bookId);
            if(book.Quantity >= 1)
            {
                BookLoan bookLoan = new BookLoan();
                bookLoan.Id = Guid.NewGuid();
                bookLoan.BookId = bookId;
                bookLoan.Book = bookRepository.GetBook(bookId);
                bookLoan.PersonName = personName;
                bookLoan.BorrowDate = borrowDate;
                bookLoan.ReturnDate = returnDate;
                loanRepository.AddBookLoan(bookLoan);

                UpdateBook(bookId, book.Title, book.Description, book.BorrowPrice, book.Quantity - 1, null);
            }

        }

        public List<string> GetAllLoans()
        {
            List<string> strings = new List<string>();
            List<BookLoan> loans = loanRepository.GetBookLoans();
            foreach(BookLoan bookLoan in loans)
            {
                strings.Add(bookLoan.ToString());
            }
            return strings;
        }

        public void CancelLoan(Guid loanId)
        {
            BookLoan loan = loanRepository.GetBookLoan(loanId);
            Book book = loan.Book;
            book.Quantity += 1;
            loanRepository.DeleteBookLoan(loanId);
        }

        public List<string> GetMostRequestedBooks()
        {
            List<BookLoan> bookLoans = loanRepository.GetBookLoans();
            Dictionary<Book, int> requests = new Dictionary<Book, int>();
            List<string> res = new List<string>();
            foreach(BookLoan bookLoan in bookLoans)
            {
                if(!requests.ContainsKey(bookLoan.Book))
                {
                    requests.Add(bookLoan.Book, 1);
                }
                else
                {
                    requests[bookLoan.Book]++;
                }
            }
            int max_request = 0;
            foreach(var  request in requests)
            {
                if(request.Value > max_request) { max_request = request.Value; }
            }
            res.Add("Most Loans: " + max_request.ToString());
            res.Add("Books with " + max_request.ToString() + " active loans");
            foreach (var request in requests)
            {
                if (request.Value == max_request) res.Add(request.Key.ToString());
            }
            return res;
        }

        public List<string> GetLeastRequestedBooks()
        {
            List<BookLoan> bookLoans = loanRepository.GetBookLoans();
            Dictionary<Book, int> requests = new Dictionary<Book, int>();
            List<string> res = new List<string>();
            foreach (BookLoan bookLoan in bookLoans)
            {
                if (!requests.ContainsKey(bookLoan.Book))
                {
                    requests.Add(bookLoan.Book, 1);
                }
                else
                {
                    requests[bookLoan.Book]++;
                }
            }
            int min_request = int.MaxValue;
            foreach (var request in requests)
            {
                if (request.Value < min_request) { min_request = request.Value; }
            }
            if(min_request == int.MaxValue)
            {
                res.Add("There are no such books");
                return res;
            }
            res.Add("Least Loans: " + min_request.ToString());
            res.Add("Books with " + min_request.ToString() + " active loans");
            foreach (var request in requests)
            {
                if (request.Value == min_request) res.Add(request.Key.ToString());
            }
            return res;
        }
    }
}
