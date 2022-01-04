using System.Linq;
using System.Collections.Generic;

using Domain.Entities;

namespace DataAccessLayer.Repositories
{
    public class BookRepository : Interfaces.IBookRepository
    {
        // FIELDS
        private static int lastBookId;

        private readonly ICollection<Book> books;

        // CONSTRUCTOR
        public BookRepository()
        {
            books = new List<Book>();
        }
        static BookRepository()
        {
            lastBookId = 0;
        }

        // METHODS
        public int Create(Book createBook)
        {
            if (createBook == null) throw new System.ArgumentNullException(nameof(createBook));

            createBook.Id = lastBookId;
            books.Add(createBook);

            return lastBookId++;
        }

        public IEnumerable<Book> Get()
        {
            return books;
        }

        public Book Get(int id)
        {
            return books.FirstOrDefault(b => b.Id == id);
        }

        public bool Update(int id, Book updateBook)
        {
            if (updateBook == null) throw new System.ArgumentNullException(nameof(updateBook));

            Book bookToUpdate = books.FirstOrDefault(b => b.Id == id);

            if (bookToUpdate != null)
            {
                bookToUpdate.Title = updateBook.Title;
                bookToUpdate.Author = updateBook.Author;

                return true;
            }
            else return false;
        }

        public bool Delete(int id)
        {
            Book bookToDelete = books.FirstOrDefault(b => b.Id == id);

            return books.Remove(bookToDelete);
        }

        public void Clear()
        {
            this.books.Clear();
            lastBookId = 0;
        }
    }
}
