
namespace Services
{
    public class AuthenticationService(UserManager<User> userManager, IOptions<JwtOptions> options, IMapper mapper, IEmailService emailService) 
        : IAuthenticationService
    {
        public async Task<bool> CheckEmailExist(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AddressDTO> GetUserAddress(string email)
        {
            var user = await userManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new UserNotFoundException(email);

            return mapper.Map<AddressDTO>(user.Address);
        }

        public async Task<UserResultDTO> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);

            return new UserResultDTO(
                user.FullName,
                user.Email!,
                await CreateTokenAsync(user));
        }

        public async Task<UserResultDTO> LoginAsync(LoginDTO loginModel)
        {
            // Check If there is user under this Email
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user == null) throw new UnAuthorizedException("Email Doesn't Exist");

            // Check If The Password Is Correct for this Email
            var result = await userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!result) throw new UnAuthorizedException();

            // Create Token and Return Response
            return new UserResultDTO(
                user.FullName,
                user.Email!,
                await CreateTokenAsync(user));
        }

        public async Task<UserResultDTO> RegisterAsync(UserRegisterDTO registerModel)
        {
            var user = new User()
            {
                Email = registerModel.Email,
                FullName = registerModel.FullName,
                UserName = registerModel.Email,
            };

            var result = await userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }

            return new UserResultDTO(
                user.FullName,
                user.Email!,
                await CreateTokenAsync(user));
        }

        public async Task<AddressDTO> UpdateUserAddress(AddressDTO address, string email)
        {
            var user = await userManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new UserNotFoundException(email);

            if(user.Address != null)
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
                user.Address.Street = address.Street;
            }
            else
            {
                var userAddress = mapper.Map<UserAddress>(address);
                user.Address = userAddress; 
            }

            await userManager.UpdateAsync(user);
            return address;
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var jwtOptions = options.Value;

            // Private Claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // Add Roles to Claims If Exist
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                audience: jwtOptions.Audience,
                issuer: jwtOptions.Issure,
                expires: DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
                claims: authClaims,
                signingCredentials: signingCreds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;  // Email doesn't exist

            // Generate a 6-digit OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Store OTP and expiration in database
            user.PasswordResetOTP = otp;
            user.PasswordResetOTPExpiry = DateTime.UtcNow.AddMinutes(10); // OTP valid for 10 minutes
            await userManager.UpdateAsync(user);

            // Send OTP via email
            var emailBody = $@"
                            <h2>Password Reset OTP</h2>
                            <p>Use the following OTP to reset your password:</p>
                            <h3>{otp}</h3>
                            <p>This OTP will expire in 10 minutes.</p>
                            <p>If you didn't request this, ignore this email.</p>";

            return await emailService.SendEmailAsync(user.Email, "Password Reset OTP", emailBody);
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;  // Email doesn't exist

            // Check if OTP is valid
            if (user.PasswordResetOTP != dto.OTP || user.PasswordResetOTPExpiry < DateTime.UtcNow)
            {
                throw new ValidationException(new List<string> { "Invalid or expired OTP." });
            }

            // Reset Password
            var resetResult = await userManager.RemovePasswordAsync(user);
            if (!resetResult.Succeeded) return false;

            resetResult = await userManager.AddPasswordAsync(user, dto.NewPassword);
            if (!resetResult.Succeeded) return false;

            // Clear OTP after successful reset
            user.PasswordResetOTP = null;
            user.PasswordResetOTPExpiry = null;
            await userManager.UpdateAsync(user);

            return true;
        }

    }
}
