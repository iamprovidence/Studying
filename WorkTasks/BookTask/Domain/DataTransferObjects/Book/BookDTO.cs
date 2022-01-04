namespace Domain.DataTransferObjects.Book
{
    // server -> client
    // client -> server
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
