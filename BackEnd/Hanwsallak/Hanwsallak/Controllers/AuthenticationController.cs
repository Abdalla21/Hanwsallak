using Hanwsallak.Domain.DTO.Authentication;
using Hanwsallak.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hanwsallak.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class AuthenticationController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> _signInManager,
        IConfiguration _config) : Controller
    {
        // Ali Makled
        [HttpPost(Name = "Register")]
        public IActionResult Register(RegisterDto register)
        {
            // Return bad request if the registeration failed
            // Create email and send it to the user to confirm his email

            return Ok();
        }

        // Ali Makled
        [HttpPost(Name = "ConfirmEmail")]
        public IActionResult ConfirmEmail(ConfirmEmailDto confirmEmail)
        {
            // Validate using the dto and confirm the email
            return Ok();
        }

        // Ahmed Rashedy
        [HttpPost(Name = "GoogleLogin")]
        public IActionResult GoogleLogin(string token)
        {
            // Login with google using the token
            return Ok();
        }

        // Ahmed Rashedy
        [HttpPost(Name = "Login")]
        public IActionResult Login(LoginDto login)
        {
            // Login using email and password
            // if they are valid, create JWT token and return it to the user
            return Ok();
        }


        //[HttpPost(Name = "RefreshToken")]
        //public IActionResult RefreshToken(RefreshTokenDto RefreshToken)
        //{
        //    return Ok();
        //}

        // Ali Makled
        [HttpPost(Name = "ForgotPassword")]
        public IActionResult ForgotPassword(string Email)
        {
            // Generate a reset token and send it to the user's email
            return Ok();
        }

        // Ali Makled
        [HttpPost(Name = "ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordDto resetPassword)
        {
            // Validate the reset token and reset the password
            return Ok();
        }

        // Ali Makled
        [HttpPost(Name = "ResendConfirmationEmail")]
        public IActionResult ResendConfirmationEmail(string Email)
        {
            return Ok();
        }
    }
}
