﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get;  set; }
        public string Username { get;  set; }
        public string Email { get;  set; }
        public string PasswordHash { get;  set; }
    }
}
