using x_nova_template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace x_nova_template.ViewModel
{
    public class UserViewModel
    {
        public int Id
        {
            get;
            set;
        }
        public string UserName { get; set; }
        public string Email { get; set; }

        public List<ApplicationUser> AllUsers { get; set; }
    }
}