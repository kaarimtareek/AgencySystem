﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgencySystemDotNet.ViewModels.Admin
{
    public class AdminViewModelR : BaseUserViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phonenumber { get; set; }

        public string Photo { get; set; }
    }

    
}