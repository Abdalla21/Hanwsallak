namespace Hanwsallak.Domain.DTO.Authentication
{
    public record ResetPasswordDto(string Email, string ResetToken, string NewPassword);
}
