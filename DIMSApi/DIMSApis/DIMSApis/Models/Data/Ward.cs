﻿using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Ward
    {
        public Ward()
        {
            Hotels = new HashSet<Hotel>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? DistrictId { get; set; }

        public virtual District? District { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
