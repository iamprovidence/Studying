namespace Domain.DataTransferObjects.Book
{
    // server -> client
    public class BookListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
