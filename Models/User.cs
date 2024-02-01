namespace Snipper_Snippet_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User(int id, string email, string password)
        {
            this.Id = id;
            this.Email = email;
            this.Password = password;
        }
    }
}
