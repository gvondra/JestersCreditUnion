using System;

namespace JestersCreditUnion.Interface.Loan.Models
{
    public class LoanApplicationDenial
    {
        public short? Reason { get; set; }
        public string ReasonDescription { get; set; }
        public DateTime? Date { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
    }
}
