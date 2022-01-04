using DataAccessLayer.Repositories;
using DataAccessLayer.Test.TestData;

using Domain.Entities;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Test.Repositories
{
    [TestFixture]
    public class BookRepositoryTest
    {
        // FIELDS
        BookInitializer initializer;

        BookRepository bookRepository;

        // CONSTRUCTORS
        public BookRepositoryTest()
        {
            initializer = new BookInitializer(recordsAmount: 10);
        }

        [SetUp]
        public void Init()
        {
            bookRepository = new BookRepository();
            initializer.Initialize(bookRepository);

        }
        [TearDown]
        public void Clear()
        {
            bookRepository.Clear();
        }

        // TEST
        #region GET
        [Test]
        public void GetAll()
        {
            // Arrange
            IEnumerable<Book> expectedBooks = initializer.GeneratedBooks;

            // Act
            IEnumerable<Book> actualBooks = bookRepository.Get();

            // Assert
            Assert.IsNotEmpty(actualBooks);
            Assert.AreEqual(expectedBooks, actualBooks);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetById_When_CorrectID_Then_CorrectRecord(int bookIdData)
        {
            // Arrange
            int bookId = bookIdData;

            Book expectedBook = initializer.GeneratedBooks[bookId];

            // Act
            Book actualBook = bookRepository.Get(bookId);

            // Assert
            Assert.IsNotNull(actualBook);
            Assert.AreEqual(expectedBook, actualBook);
        }

        [Test]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        public void GetById_When_WrongID_Then_NullResult(int bookIdData)
        {
            // Arrange
            int bookId = bookIdData;
            
            // Act
            Book actualBook = bookRepository.Get(bookId);

            // Assert
            Assert.IsNull(actualBook);
        }
        #endregion

        #region CREATE
        [Test]
        public void Create_WhenCorrect_ThenCreate()
        {
            // Arrange
            Book expectedBookCreate = new Book
            {
                Title = "Title",
                Author = "Author"
            };

            // Act
            int actualCreateBookId = bookRepository.Create(expectedBookCreate);

            Book actualCreatedBook = bookRepository.Get(actualCreateBookId);
            IEnumerable<Book> actualBookList = bookRepository.Get();

            // Assert
            Assert.AreEqual(expectedBookCreate.Id, actualCreateBookId);

            Assert.IsNotNull(actualCreatedBook);

            Assert.IsNotNull(actualBookList);
            Assert.Contains(actualCreatedBook, actualBookList.ToArray());
        }
        [Test]
        public void Create_WhenNull_ThenException()
        {
            // Arrange
            Book expectedBookCreate = null;

            // Act
            // Assert
            Assert.Throws<System.ArgumentNullException>(() => bookRepository.Create(expectedBookCreate));
        }
        #endregion

        #region DELETE
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_WhenCorrectId_ThenDelete(int bookIdData)
        {
            // Arrange
            int bookId = bookIdData;

            Book expectedBookToDelete = initializer.GeneratedBooks[bookId];

            // Act
            bool actualBookDeleteResult = bookRepository.Delete(bookId);
            IEnumerable<Book> actualBookList = bookRepository.Get();

            // Assert
            Assert.IsNotNull(expectedBookToDelete);

            Assert.IsTrue(actualBookDeleteResult);

            Assert.That(actualBookList, Has.No.Member(expectedBookToDelete));
        }
        [Test]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        public void Delete_WhenWrongId_ThenDoNotDelete(int bookIdData)
        {
            // Arrange
            int bookId = bookIdData;

            int initialRecordsAmount = initializer.InitialRecordsAmount;
            
            // Act
            bool actualBookDeleteResult = bookRepository.Delete(bookId);
            IEnumerable<Book> actualBookList = bookRepository.Get().ToArray();
            int actualBookListLength = actualBookList.Count();
            
            // Assert
            Assert.IsFalse(actualBookDeleteResult);

            Assert.That(actualBookListLength, Is.EqualTo(initialRecordsAmount));
        }

        [Test]
        public void ClearTest()
        {
            // Arrange
            IEnumerable<Book> bookListBeforeClear = bookRepository.Get().ToArray();
            
            // Act
            bookRepository.Clear();
            IEnumerable<Book> bookListAfterClear = bookRepository.Get().ToArray();

            // Assert
            Assert.IsNotEmpty(bookListBeforeClear);

            Assert.IsEmpty(bookListAfterClear);
        }
        #endregion

        #region UPDATE

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Update_WhenCorrect_ThenUpdate(int bookIdData)
        {
            // Arrange
            int bookToUpdateId = bookIdData;

            Book expectedBookUpdate = new Book
            {
                Title = "New Title",
                Author = "Author"
            };

            // Act
            bool actualUpdateResult = bookRepository.Update(bookToUpdateId, expectedBookUpdate);

            Book actualUpdateBook = bookRepository.Get(bookToUpdateId);

            // Assert
            Assert.IsTrue(actualUpdateResult);

            Assert.AreEqual(actualUpdateBook.Id, bookToUpdateId);
            Assert.AreEqual(actualUpdateBook.Author, expectedBookUpdate.Author);
            Assert.AreEqual(actualUpdateBook.Title, expectedBookUpdate.Title);
        }

        [Test]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        public void Update_WhenWrongId_ThenDoNotUpdate(int bookIdData)
        {
            // Arrange
            int bookToUpdateId = bookIdData;

            Book expectedBookUpdate = new Book
            {
                Title = "New Title",
                Author = "Author"
            };

            // Act
            bool actualUpdateResult = bookRepository.Update(bookToUpdateId, expectedBookUpdate);

            // Assert
            Assert.IsFalse(actualUpdateResult);
        }
        [Test]
        public void Update_WhenNull_ThenException()
        {
            // Arrange
            int bookId = 0;
            Book expectedBookUpdate = null;

            // Act
            // Assert
            Assert.Throws<System.ArgumentNullException>(() => bookRepository.Update(bookId, expectedBookUpdate));
        }
        #endregion
    }
}
