namespace Proj1.Models
{
    public class ChangeUserModel
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public string? Name { get; set; }
        public string? LanguageJsonName { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
