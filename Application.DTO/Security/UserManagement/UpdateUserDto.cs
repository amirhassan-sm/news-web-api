using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.UserManagement
{
    public class UpdateUserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }=string.Empty;

        public string FirstName{ get; set; }=string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;

    }
}
