using System.Collections.Generic;

using AutoMapper;

using DataAccessLayer.Interfaces;

using Domain.DataTransferObjects.Book;
using Domain.Entities;

namespace BusinessLogicLayer.Services
{
    public class BookService : Interfaces.IBookService
    {
        // FIELDS
        private readonly IBookRepository bookRepository;
        private readonly IMapper mapper;

        // CONSTRUCTORS
        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            this.bookRepository = bookRepository;
            this.mapper = mapper;
        }
        
        // METHODS
        public int Create(CreateBookDTO createBookDTO)
        {
            Book createBook = mapper.Map<Book>(createBookDTO);

            return bookRepository.Create(createBook);
        }

        public bool Delete(int id)
        {
            return bookRepository.Delete(id);
        }

        public IEnumerable<BookListDTO> Get()
        {
            IEnumerable<Book> books = this.bookRepository.Get();

            return mapper.Map<IEnumerable<BookListDTO>>(books);
        }

        public BookDTO Get(int id)
        {
            Book book = this.bookRepository.Get(id);

            return book != null ? mapper.Map<BookDTO>(book) : null;
        }

        public bool Update(int id, UpdateBookDTO updateBookDTO)
        {
            Book updateBook = this.mapper.Map<Book>(updateBookDTO);

            return this.bookRepository.Update(id, updateBook);
        }
    }
}
