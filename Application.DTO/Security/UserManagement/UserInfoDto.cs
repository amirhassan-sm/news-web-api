using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.UserManagement
{
    public class UserInfoDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}
