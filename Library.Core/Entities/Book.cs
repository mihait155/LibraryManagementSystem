using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class Book
    {
        // Properties with underlying private fields for EFCore to create db from
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int BorrowPrice { get; set; }
        public List<BookAuthor> BookAuthors { get; set; }
        public Book() { }
        public Book(string title, string description, int quantity, int borrowPrice, List<BookAuthor> bookAuthors)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Quantity = quantity;
            BorrowPrice = borrowPrice;
            BookAuthors = bookAuthors ?? new List<BookAuthor>();
        }

        public override string ToString()
        {
            var authorNames = BookAuthors?.Select(ba => ba.Author.Name).ToList();
            string authors = authorNames != null && authorNames.Any() ? string.Join(", ", authorNames) : "Unknown";
            return "Book: Title: " + Title + "; Authors: " + authors + "; Quantity: " + Quantity.ToString()
                + "; Description: " + Description + "; Borrow Price: " + BorrowPrice.ToString() + "; Id: " + Id.ToString();
        }
    }
}
