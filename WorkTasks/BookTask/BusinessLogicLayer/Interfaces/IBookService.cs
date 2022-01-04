using Domain.DataTransferObjects.Book;

using System.Collections.Generic;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBookService
    {
        IEnumerable<BookListDTO> Get();
        BookDTO Get(int id);
        int Create(CreateBookDTO createBookDTO);
        bool Update(int id, UpdateBookDTO updateBookDTO);
        bool Delete(int id);
    }
}
