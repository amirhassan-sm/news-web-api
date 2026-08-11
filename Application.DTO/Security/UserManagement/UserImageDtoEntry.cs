using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.UserManagement
{
    public class UserImageDtoEntry
    {
        public IFormFile ProfileImage { get; set; } = default!;
    }
}
