using Hanwsallak.API.Services;
using Hanwsallak.Domain.DTO.Authentication;
using Hanwsallak.Domain.Entity;
using Hanwsallak.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hanwsallak.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly JwtService _jwtService;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _jwtService = jwtService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (register.Password != register.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match" });

            var user = new ApplicationUser
            {
                UserName = register.Email,
                Email = register.Email,
                FullName = register.FullName,
                PhoneNumber = register.PhoneNumber,
                EmailConfirmed = false // Will be confirmed via email
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { 
                    message = "Registration failed", 
                    errors = result.Errors.Select(e => e.Description) 
                });
            }

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            // TODO: Send confirmation email with token
            // For now, return success with token (remove in production)
            return Ok(new { 
                message = "Registration successful. Please confirm your email.",
                userId = user.Id,
                confirmationToken = token // Remove this in production - send via email instead
            });
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmail)
        {
            var user = await _userManager.FindByIdAsync(confirmEmail.UserID.ToString());
            if (user == null)
                return NotFound(new { message = "User not found" });

            var result = await _userManager.ConfirmEmailAsync(user, confirmEmail.ConfirmationToken);

            if (!result.Succeeded)
                return BadRequest(new { 
                    message = "Email confirmation failed", 
                    errors = result.Errors.Select(e => e.Description) 
                });

            return Ok(new { message = "Email confirmed successfully" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return Unauthorized(new { message = "Account is locked out" });
                if (result.IsNotAllowed)
                    return Unauthorized(new { message = "Email not confirmed" });
                
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var tokenModel = _jwtService.GenerateToken(user, roles);

            return Ok(new
            {
                token = tokenModel.Token,
                refreshToken = tokenModel.RefreshToken,
                tokenType = tokenModel.TokenType,
                expiresIn = tokenModel.TokenExpiryHours * 3600, // in seconds
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    roles = roles
                }
            });
        }

        [HttpPost("GoogleLogin")]
        public Task<IActionResult> GoogleLogin([FromBody] string token)
        {
            // TODO: Implement Google OAuth token validation
            // For now, return not implemented
            return Task.FromResult<IActionResult>(StatusCode(501, new { message = "Google login not yet implemented" }));
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal if user exists or not for security
                return Ok(new { message = "If the email exists, a password reset link has been sent" });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // TODO: Send password reset email with token
            // For now, return success with token (remove in production)
            return Ok(new { 
                message = "If the email exists, a password reset link has been sent",
                resetToken = token // Remove this in production - send via email instead
            });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.ResetToken, resetPassword.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new { 
                    message = "Password reset failed", 
                    errors = result.Errors.Select(e => e.Description) 
                });

            return Ok(new { message = "Password reset successfully" });
        }

        [HttpPost("ResendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal if user exists or not for security
                return Ok(new { message = "If the email exists and is not confirmed, a confirmation email has been sent" });
            }

            if (user.EmailConfirmed)
                return BadRequest(new { message = "Email is already confirmed" });

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            // TODO: Send confirmation email with token
            // For now, return success with token (remove in production)
            return Ok(new { 
                message = "If the email exists and is not confirmed, a confirmation email has been sent",
                confirmationToken = token // Remove this in production - send via email instead
            });
        }
    }
}
