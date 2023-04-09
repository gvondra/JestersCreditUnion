using System;

namespace JestersCreditUnion.Interface.Models
{
    public class WorkTaskContext
    {
        public Guid? WorkTaskContextId { get; set; }
        public Guid? WorkTaskId { get; set; }
        public short? ReferenceType { get; set; }
        public string ReferenceValue { get; set; }
        public DateTime? CreateTimestamp { get; set; }
    }
}
