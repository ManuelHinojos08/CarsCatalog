using System;
using System.Collections.Generic;

namespace CarsCatalog.Models
{
    // Modelos devueltos por las acciones de AccountController.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountType { get; set; }
        public string FacebookToken { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public int Ads { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class UserLoginModel
    {
        public string User { get; set; }

        public string Password { get; set; }

        public bool HasValidAccount { get; set; }

        public string LoginMessage { get; set; }
    }
}
