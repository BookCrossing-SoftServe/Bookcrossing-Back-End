namespace Application.Dto.Comment.Book
{
    public class RootInsertDto
    {
        public string Text { get; set; }
        public int BookId { get; set; }
        public int OwnerId { get; set; }
    }
}
