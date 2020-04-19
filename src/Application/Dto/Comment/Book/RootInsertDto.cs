namespace Application.Dto.Comment.Book
{
    public class RootInsertDto
    {
        public string Text { get; set; }
        public int BookId { get; set; }
        public int CommentOwnerId { get; set; }
    }
}
