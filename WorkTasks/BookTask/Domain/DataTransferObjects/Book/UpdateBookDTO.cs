namespace Domain.DataTransferObjects.Book
{
    // client -> server
    public class UpdateBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
