namespace Application.Dto.Comment.Book
{
    public class RootUpdateDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int OwnerId { get; set; }
        public int Rating { get; set; }
    }
}
