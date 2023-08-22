using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLibrarian { get; set; }

        public User(string username, string password, bool isLibrarian)
        {
            Username = username;
            Password = password;
            IsLibrarian = isLibrarian;
        }
    }
}
