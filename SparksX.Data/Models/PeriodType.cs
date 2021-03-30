using System.Collections.Generic;

namespace SparksX.Data.Models
{
    public partial class PeriodType
    {
        public PeriodType()
        {
            MailReport = new HashSet<MailReport>();
        }

        public int PeriodTypeId { get; set; }
        public string PeriodTypeName { get; set; }

        public virtual ICollection<MailReport> MailReport { get; set; }
    }
}