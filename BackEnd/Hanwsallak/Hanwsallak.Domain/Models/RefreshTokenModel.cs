using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanwsallak.Domain.Models
{
    internal class RefreshTokenModel
    {
        public record RefreshTokenModel(string UserID, string RefreshToken);
    
    }
}
