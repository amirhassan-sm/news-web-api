using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.RoleManagement
{
    public class EditRoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
       
    }
}
