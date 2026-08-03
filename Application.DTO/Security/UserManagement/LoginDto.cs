using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.UserManagement
{
    public class LoginDto
    {
        public string userName { get; set; }=string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
