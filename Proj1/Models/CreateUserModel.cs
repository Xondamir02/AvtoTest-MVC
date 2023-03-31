namespace Proj1.Models
{
    public class CreateUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
    }
}
 
