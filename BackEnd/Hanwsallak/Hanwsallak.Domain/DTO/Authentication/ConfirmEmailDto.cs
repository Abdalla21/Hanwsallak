namespace Hanwsallak.Domain.DTO.Authentication
{
    public class ConfirmEmailDto
    {
        public Guid UserID { get; set; }
        public string ConfirmationToken { get; set; } = string.Empty;
    }
}
