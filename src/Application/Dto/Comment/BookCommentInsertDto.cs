namespace Application.Dto.Comment
{
    public class BookCommentInsertDto
    {
        public string Text { get; set; }
        public int BookId { get; set; }
        public int CommentOwnerId { get; set; }
    }
}
