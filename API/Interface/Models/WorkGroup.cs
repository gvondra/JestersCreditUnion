﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Models
{
    public class WorkGroup
    {
        public Guid? WorkGroupId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public List<string> MemberUserIds { get; set; }
        public List<Guid> WorkTaskTypeIds { get; set; }
    }
}
