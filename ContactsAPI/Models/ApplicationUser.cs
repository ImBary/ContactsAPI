﻿using Microsoft.AspNetCore.Identity;

namespace ContactsAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name {  get; set; }
    }
}
