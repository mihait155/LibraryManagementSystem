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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDBContext _context;

        public AuthorRepository(LibraryDBContext context)
        {
            _context = context;
        }

        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public void DeleteAuthor(Guid id)
        {
            var author = _context.Authors.Find(id);
            if(author != null)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
            }
        }

        public Author GetAuthor(Guid id)
        {
            return _context.Authors.Include(a => a.BookAuthors).ThenInclude(ba => ba.Book)
                .FirstOrDefault(a => a.Id == id);
        }

        public Author GetAuthorByName(string name)
        {
            return _context.Authors.Include(a => a.BookAuthors).ThenInclude(ba => ba.Book)
                .FirstOrDefault(a => a.Name == name);
        }

        public List<Author> GetAuthors()
        {
            return _context.Authors.Include(a => a.BookAuthors).ThenInclude(ba => ba.Book)
                .ToList();
        }
        
        // Change
        public void UpdateAuthor(Guid id, string Name, List<Guid> newBookIds)
        {
            var author = _context.Authors.Include(a => a.BookAuthors).FirstOrDefault(a => a.Id == id);
            if (author == null) return;

            author.Name = Name;

            _context.BookAuthors.RemoveRange(author.BookAuthors);

            foreach (var bId  in newBookIds)
            {
                BookAuthor ba = new BookAuthor();
                ba.AuthorId = id;
                ba.BookId = bId;
                ba.Book = _context.Books.Find(bId);
                ba.Author = author;
                author.BookAuthors.Add(ba);
            }
            _context.SaveChanges();
        }
    }
}
