
namespace Shared
{
    public record UserRegisterDTO
    {
        [Required(ErrorMessage = "FullName Is Required")]
        public string FullName { get; init; }

        [EmailAddress]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; init; }

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; init; }
    }
}
