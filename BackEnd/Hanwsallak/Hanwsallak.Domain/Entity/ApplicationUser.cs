using Microsoft.AspNetCore.Identity;

namespace Hanwsallak.Domain.Entity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }

    }
}
