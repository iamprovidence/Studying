using Bogus;

using DataAccessLayer.Interfaces;

using Domain.Entities;

using System.Collections.Generic;

namespace DataAccessLayer.Test.TestData
{
    public class BookInitializer
    {
        public int InitialRecordsAmount { get; private set; }
        public IList<Book> GeneratedBooks { get; private set; }

        public BookInitializer(int recordsAmount)
        {
            Faker<Book> fakeBooks = new Faker<Book>()
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
                .RuleFor(b => b.Author, f => f.Person.FullName);

            GeneratedBooks = fakeBooks.Generate(recordsAmount);

            InitialRecordsAmount = recordsAmount;
        }

        public void Initialize(IBookRepository bookRepository)
        {
            foreach(Book createdBook in GeneratedBooks)
            {
                bookRepository.Create(createdBook);
            }
        }
    }
}
