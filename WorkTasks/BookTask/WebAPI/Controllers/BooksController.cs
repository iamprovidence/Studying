using System.Collections.Generic;

using BusinessLogicLayer.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Domain.DataTransferObjects.Book;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // FIELDS
        private readonly IBookService bookService;

        // CONSTRUCTORS
        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<BookListDTO>> Get()
        {
            return Ok(bookService.Get());
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public ActionResult<BookDTO> Get(int id)
        {
            BookDTO bookDTO = bookService.Get(id);

            int statusCodeResponse = bookDTO != null ? 200 : 404;

            return StatusCode(statusCodeResponse, bookDTO);
        }

        // POST api/books
        [HttpPost]
        public ActionResult Post([FromBody] CreateBookDTO value)
        {
            int createdBookId = bookService.Create(value);

            int statusCodeResponse = createdBookId != -1 ? 200 : 409;

            return StatusCode(statusCodeResponse);
        }

        // PUT api/books/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] UpdateBookDTO value)
        {
            bool updateResult = bookService.Update(id, value);

            int statusCodeResponse = updateResult ? 200 : 409;

            return StatusCode(statusCodeResponse);
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool deleteResult = bookService.Delete(id);

            int statusCodeResponse = deleteResult ? 200 : 409;

            return StatusCode(statusCodeResponse);
        }
    }
}
