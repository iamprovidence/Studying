namespace Domain.DataTransferObjects.Book
{
    // client -> server
    public class CreateBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
