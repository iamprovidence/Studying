using Domain.Entities;

using Domain.DataTransferObjects.Book;

namespace Domain.MappingProfiles
{
    public class BookProfile: AutoMapper.Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, BookListDTO>();
            CreateMap<CreateBookDTO, Book>();
            CreateMap<UpdateBookDTO, Book>();
        }
    }
}
