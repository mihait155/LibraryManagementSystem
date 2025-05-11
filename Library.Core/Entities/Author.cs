using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }
        public Author() { }
        public Author(string name, List<BookAuthor> bookAuthors)
        {
            Id = Guid.NewGuid();
            Name = name;
            BookAuthors = bookAuthors ?? new List<BookAuthor>();
        }
        public override string ToString()
        {
            var bookNames = BookAuthors?.Select(ba => ba.Book.Title).ToList();
            string books = bookNames != null && bookNames.Any() ? string.Join(", ", bookNames) : "Unknown";
            return "Author: Name: " + Name + "; Books: " + books + "; Id: " + Id;
        }
    }
}
