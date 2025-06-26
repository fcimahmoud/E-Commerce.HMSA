
namespace Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public Address Address { get; set; } // Ref Nav Prop
        public string? PasswordResetOTP {  get; set; }
        public DateTime? PasswordResetOTPExpiry { get; set; }
    }
}
