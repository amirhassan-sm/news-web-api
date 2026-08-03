using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Security.UserManagement
{
        public class UserProfileComplexResult:PageModel
        {

            public List<UserProfileListIteam> userProfiles { get; set; } = new();
        }
}
