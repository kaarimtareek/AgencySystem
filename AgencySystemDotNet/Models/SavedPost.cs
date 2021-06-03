﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.Models
{
    public class SavedPost : BaseEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PostId { get; set; }
        public virtual User Customer { get; set; }
        public virtual Post Post { get; set; }
    }
}
