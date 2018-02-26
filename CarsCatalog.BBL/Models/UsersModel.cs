using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.BBL.Models
{
    public class UserLoginModel
    {
        public string User { get; set; }

        public string Password { get; set; }

        public bool HasValidAccount { get; set; }

        public string LoginMessage { get; set; }
    }

    public class NewUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string FacebookToken { get; set; }

        public int AccountTypeId { get; set; }
    }

    public class SaveMessagesModel
    {
        public bool Saved { get; set; }
        public string ErrorMessage { get; set; }
    }
}
