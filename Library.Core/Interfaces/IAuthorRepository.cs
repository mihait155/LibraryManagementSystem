using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Interfaces
{
    public interface IAuthorRepository
    {
        List<Author> GetAuthors();
        Author GetAuthor(Guid id);
        void AddAuthor(Author author);
        void UpdateAuthor(Guid id, string Name, List<Guid> newBookIds);
        void DeleteAuthor(Guid id);

    }
}
