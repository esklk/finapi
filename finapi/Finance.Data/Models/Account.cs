﻿using System.Collections.Generic;

namespace Finance.Data.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
