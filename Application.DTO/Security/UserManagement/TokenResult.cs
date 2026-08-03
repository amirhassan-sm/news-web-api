using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.UserManagement
{
    public class TokenResult
    {
        public string token { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
    }
}
