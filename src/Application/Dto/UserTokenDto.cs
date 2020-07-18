namespace Application.Dto
{
    public class UserTokenDto
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public TokenDto Token { get; set; }
    }
}
