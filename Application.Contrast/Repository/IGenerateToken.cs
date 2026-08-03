
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contrast.Repository
{
    public interface IGenerateToken
    {
        Task<string> GenerateAcsessToken(int userId,string userName , string firstName , string lastName);

        string GenerateRefreshToken();

    }
}
