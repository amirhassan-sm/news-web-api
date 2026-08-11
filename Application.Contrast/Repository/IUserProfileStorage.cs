using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Application.Contrast.Repository
{
    public interface IUserProfileStorage
    {
        Task<string> SaveProfileAsync(Stream fileStream , string fileName);
        Task DeleteProfileAsync(string url);
        //Task<bool> IsImageValid(string url);
        
    }
}
