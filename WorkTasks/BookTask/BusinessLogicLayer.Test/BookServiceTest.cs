using AutoMapper;

using BusinessLogicLayer.Services;

using DataAccessLayer.Interfaces;

using Moq;

using Domain.Entities;
using Domain.DataTransferObjects.Book;

using NUnit.Framework;

using System.Linq;
using System.Collections.Generic;

namespace BusinessLogicLayer.Test
{
    public class BookServiceTest
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Create_When_Correct_Then_CreateCalled(int createdBookIdData)
        {
            // Arrange
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<Book>(It.IsAny<CreateBookDTO>()))
                .Returns(new Book());


            int expectedReturnId = createdBookIdData;
            Mock<IBookRepository> repositoryMock = new Mock<IBookRepository>();
            repositoryMock
                .Setup(r => r.Create(It.IsAny<Book>()))
                .Returns(expectedReturnId);
            
            BookService bookService = new BookService(repositoryMock.Object, mapperMock.Object);

            // Act
            int actualReturnedId = bookService.Create(new CreateBookDTO());

            // Assert
            Assert.AreEqual(expectedReturnId, actualReturnedId);

            mapperMock.Verify(m => m.Map<Book>(It.IsAny<CreateBookDTO>()), Times.Once);
            repositoryMock.Verify(r => r.Create(It.IsAny<Book>()), Times.Once);
        }

        [Test]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(2, false)]
        public void Delete_When_Correct_Then_DeleteCalled(int deleteBookIdData, bool expectedResultData)
        {
            // Arrange
            int deleteBookId = deleteBookIdData;
            bool expectedResult = expectedResultData;

            Mock<IBookRepository> repositoryMock = new Mock<IBookRepository>();
            repositoryMock
                .Setup(r => r.Delete(It.IsAny<int>()))
                .Returns(expectedResult);

            BookService bookService = new BookService(repositoryMock.Object, null);

            // Act
            bool actualDeleteResult = bookService.Delete(deleteBookId);

            // Assert
            Assert.AreEqual(expectedResult, actualDeleteResult);
            
            repositoryMock.Verify(r => r.Delete(It.IsAny<int>()), Times.Once);
        }

        #region GET
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetById_When_MapCorrecttly_Then_GetData(int bookIdData)
        {
            // Arrange
            int bookId = bookIdData;

            BookDTO expectedBook = new BookDTO();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<BookDTO>(It.IsAny<Book>()))
                .Returns(expectedBook);
            
            Mock<IBookRepository> repositoryMock = new Mock<IBookRepository>();
            repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns(new Book());

            BookService bookService = new BookService(repositoryMock.Object, mapperMock.Object);

            // Act
            BookDTO actualResult = bookService.Get(bookId);

            // Assert
            Assert.AreEqual(expectedBook, actualResult);

            mapperMock.Verify(m => m.Map<BookDTO>(It.IsAny<Book>()), Times.Once);
            repositoryMock.Verify(r => r.Get(It.IsAny<int>()), Times.Once);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetById_When_NoDataFromRepository_Then_NullResult(int bookIdData)
        {
            // Arrange
            int bookId = bookIdData;

            BookDTO expectedBook = null;
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<BookDTO>(It.IsAny<Book>()))
                .Returns(expectedBook);

            Mock<IBookRepository> repositoryMock = new Mock<IBookRepository>();
            repositoryMock
                .Setup(r => r.Get(It.IsAny<int>()))
                .Returns(null as Book);

            BookService bookService = new BookService(repositoryMock.Object, mapperMock.Object);

            // Act
            BookDTO actualResult = bookService.Get(bookId);

            // Assert
            Assert.IsNull(actualResult);
            Assert.AreEqual(expectedBook, actualResult);

            mapperMock.Verify(m => m.Map<BookDTO>(It.IsAny<Book>()), Times.Never);
            repositoryMock.Verify(r => r.Get(It.IsAny<int>()), Times.Once);
        }

        [Test]
        [TestCase(new int[] { 0, 1, 2 })]
        [TestCase(new int[] { })]
        public void GetAll_When_DifferentDataLength_Then_SameDataResultResult(int[] booksIdData)
        {
            // Arrange
            IEnumerable<Book> repositoryResult = booksIdData.Select(id => new Book { Id = id });

            Mock<IBookRepository> repositoryMock = new Mock<IBookRepository>();
            repositoryMock
                .Setup(r => r.Get())
                .Returns(repositoryResult);

            IEnumerable<BookListDTO> expectedResult = repositoryResult.Select(b => new BookListDTO { Id = b.Id });
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<IEnumerable<BookListDTO>>(It.IsNotNull<IEnumerable<Book>>()))
                .Returns(expectedResult);
            
            BookService bookService = new BookService(repositoryMock.Object, mapperMock.Object);

            // Act
            IEnumerable<BookListDTO> actualResult = bookService.Get();

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult, actualResult);

            mapperMock.Verify(m => m.Map<IEnumerable<BookListDTO>>(It.IsNotNull<IEnumerable<Book>>()), Times.Once);
            repositoryMock.Verify(r => r.Get(), Times.Once);
        }
        #endregion
        
        [Test]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, false)]
        public void Update_When_CorrectData_Then_UpdateResult(int bookIdData, bool updateResultData)
        {
            // Arrange
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<Book>(It.IsNotNull<UpdateBookDTO>()))
                .Returns(new Book());


            int bookId = bookIdData;
            bool expectedUpdateResult = updateResultData;
            Mock<IBookRepository> repositoryMock = new Mock<IBookRepository>();
            repositoryMock
                .Setup(r => r.Update(bookId, It.IsNotNull<Book>()))
                .Returns(expectedUpdateResult);

            UpdateBookDTO updateBookDTO = new UpdateBookDTO();
            BookService bookService = new BookService(repositoryMock.Object, mapperMock.Object);

            // Act
            bool actualResult = bookService.Update(bookId, updateBookDTO);

            // Assert
            Assert.AreEqual(expectedUpdateResult, actualResult);

            mapperMock.Verify(m => m.Map<Book>(It.IsNotNull<UpdateBookDTO>()), Times.Once);
            repositoryMock.Verify(r => r.Update(bookId, It.IsNotNull<Book>()), Times.Once);
        }
        
    }
}