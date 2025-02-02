﻿using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Province
    {
        public Province()
        {
            Districts = new HashSet<District>();
            Hotels = new HashSet<Hotel>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Type { get; set; }

        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
