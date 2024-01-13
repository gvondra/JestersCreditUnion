﻿using System;
using System.Collections.Generic;

namespace JestersCreditUnion.Interface.Models
{
    public class WorkTask
    {
        public Guid? WorkTaskId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string AssignedToUserId { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public WorkTaskType WorkTaskType { get; set; }
        public WorkTaskStatus WorkTaskStatus { get; set; }
        public List<WorkTaskContext> WorkTaskContexts { get; set; }
    }
}
