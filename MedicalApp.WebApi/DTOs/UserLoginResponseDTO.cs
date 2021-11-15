﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjemploApiRest.WebApi.DTOs
{
    public class UserLoginResponseDTO
    {
        public string Token { get; set; }
        public bool Login { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Errors { get; set; }
    }
}
