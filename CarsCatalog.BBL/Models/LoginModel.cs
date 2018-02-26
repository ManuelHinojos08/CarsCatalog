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
}
