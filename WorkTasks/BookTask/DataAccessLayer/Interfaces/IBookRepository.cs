using Domain.Entities;

using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IBookRepository
    {
        IEnumerable<Book> Get();
        Book Get(int id);

        int Create(Book createBook);

        bool Update(int id, Book updateBook);

        bool Delete(int id);
        void Clear();
    }
}
