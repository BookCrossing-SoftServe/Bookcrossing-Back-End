namespace Domain.RDBMS.Entities
{
    public class Aphorism : IEntityBase
    {
        public int Id { get; set; }

        public string Phrase { get; set; }

        public string PhraseAuthor { get; set; }

        public bool IsCurrent { get; set; }
    }
}
