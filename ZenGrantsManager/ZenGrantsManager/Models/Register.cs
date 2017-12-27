using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZenGrantsManager.Models
{
    public class Register
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string PhoneNumber { get; set; }
       
    }
}