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
        public async Task<ActionResult<ApplicationResult<JwtTokenModel>>> GoogleLogin(string token)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);

            var user = await userManager.FindByEmailAsync(payload.Email);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    FullName = payload.Name,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user);
            }

            UserLoginInfo info = new UserLoginInfo("Google", payload.Subject, "Google");
            var userLogins = await userManager.GetLoginsAsync(user);
            if (!userLogins.Any(l => l.LoginProvider == "Google"))
                await userManager.AddLoginAsync(user, info);

            await _signInManager.SignInAsync(user, isPersistent: false);
            JwtTokenModel tokenModel = new JwtTokenModel
            {
                Token = await GenerateJwtToken(user, true),
                TokenExpiryHours = 2,
                TokenExpiration = DateTime.Now.AddHours(2),
                RefreshToken = await GenerateJwtToken(user, false),
                RefreshTokenExpiryHours = 24,
                RefreshTokenExpiration = DateTime.Now.AddDays(1)
            };
            return ApiResponseStatus.Ok<JwtTokenModel>(tokenModel);
        }


        // Ahmed Rashedy
        [HttpPost(Name = "Login")]
        public Task<ActionResult<ApplicationResult<JwtTokenModel>>> Login(LoginDto loginDto)
        {
        var user = await userManager.FindByEmailAsync(loginDto.Email.ToLower());
            if (user is null)
                return ApiResponseStatus.Unauthorized<JwtTokenModel>(Errors.Unauthorized(ErrorConstants.INVALIDUSER, ErrorConstants.INVALIDUSERCODE));

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
                return ApiResponseStatus.Unauthorized<JwtTokenModel>(Errors.Unauthorized(ErrorConstants.INVALIDUSER, ErrorConstants.INVALIDUSERCODE));

            await _signInManager.SignInAsync(user, isPersistent: false);
            JwtTokenModel tokenModel = new JwtTokenModel
            {
                Token = await GenerateJwtToken(user, true),
                TokenExpiryHours = 2,
                TokenExpiration = DateTime.Now.AddHours(2),
                RefreshToken = await GenerateJwtToken(user, false),
                RefreshTokenExpiryHours = 24,
                RefreshTokenExpiration = DateTime.Now.AddDays(1)
            };
            user.RefreshToken = tokenModel.RefreshToken;
            user.RefreshTokenExpiryTime = tokenModel.RefreshTokenExpiration;
            await userManager.UpdateAsync(user);

            return ApiResponseStatus.Ok<JwtTokenModel>(tokenModel);
        
        }


        [HttpPost(Name = "RefreshToken")]
        public async Task<ActionResult<ApplicationResult<JwtTokenWithoutRefreshTokenModel>>> RefreshToken(RefreshTokenModel RefreshTokenModel)
        {
            var user = await userManager.FindByIdAsync(RefreshTokenModel.UserID);
            if (user is null || user.RefreshToken != RefreshTokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return ApiResponseStatus.Unauthorized<JwtTokenWithoutRefreshTokenModel>(Errors.Unauthorized(ErrorConstants.INVALIDREFRESHTOKEN, ErrorConstants.INVALIDREFRESHTOKENCODE));

            JwtTokenWithoutRefreshTokenModel tokenModel = new JwtTokenWithoutRefreshTokenModel
            {
                Token = await GenerateJwtToken(user, true),
                ExpiryHours = 2,
                Expiration = DateTime.Now.AddHours(2)
            };
            return ApiResponseStatus.Ok<JwtTokenWithoutRefreshTokenModel>(tokenModel);
        }

        
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
