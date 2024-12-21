using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDKCurs
{
    public class User
    {
        public string Username { get; set; }
        public string Role { get; set; }

        public User(string username, string role)
        {
            Username = username;
            Role = role;
        }

        public bool IsAdmin()
        {
            return Role.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }

        public void ChangeUsername(string newUsername)
        {
            if (!string.IsNullOrEmpty(newUsername))
            {
                Username = newUsername;
            }
        }
    }
}

