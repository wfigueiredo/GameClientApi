﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameClientApi.Domain.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}