using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.UserManagement
{
    public class UserProfileListIteam
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
