using MyFoodDoc.CMS.Models;
using System;

namespace MyFoodDoc.CMS.Auth.Implementation
{
    public class DebugAuthenticationService : ICustomAuthenticationService
    {
        public AppUser Login(string username, string password)
        {
            if (username == "admin")
            {
                return new AppUser
                {
                    DisplayName = "admin",
                    Username = "admin",
                    Roles = new string[] { "Admin", "Editor", "Approver" }
                };
            }
            else if (username == "editor")
            {
                return new AppUser
                {
                    DisplayName = "editor",
                    Username = "editor",
                    Roles = new string[] { "Editor" }
                };
            }
            else if (username == "editor2")
            {
                return new AppUser
                {
                    DisplayName = "editor2",
                    Username = "editor2",
                    Roles = new string[] { "Editor" }
                };
            }
            else if (username == "approver")
            {
                return new AppUser
                {
                    DisplayName = "approver",
                    Username = "admapproverin",
                    Roles = new string[] { "Approver" }
                };
            }

            throw new Exception("Login failed.");
        }
    }
}
