namespace Hive.Identity.Models
{
    public class RegisterResponseViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public RegisterResponseViewModel(ApplicationUser user)
        {
            Id = user.Id;
            Email = user.Email;
        }
    }
}