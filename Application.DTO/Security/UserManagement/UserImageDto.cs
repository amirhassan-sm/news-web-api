using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.UserManagement
{
    public class UserImageDto
    {
        public int UserId { get; set; }
        public IFormFile ProfileImage { get; set; } = default!;
        
        //public long fileSize { get; set; }
    }
}
