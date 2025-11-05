using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanwsallak.Domain.Models
{
    internal class JwtTokenWithoutRefreshTokenModel
    {
        public string Token { get; set; }
        public int ExpiryHours { get; set; }
        public DateTime Expiration { get; set; }
    }
}
