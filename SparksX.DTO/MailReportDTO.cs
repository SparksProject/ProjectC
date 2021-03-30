using System;
using System.Collections.Generic;

namespace SparksX.DTO
{
    public class MailReportDTO
    {
        public int MailReportId { get; set; }
        public string MailReportName { get; set; }
        public string Subject { get; set; }
        public string BodyTemplate { get; set; }
        public string SqlScript { get; set; }
        public int PeriodTypeId { get; set; }
        public string PeriodValue { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public int[] ToEmails { get; set; }
        public int[] CcEmails { get; set; }
        public string PeriodTypeName { get; set; }
        public string RecordStatusName { get; set; }
        public List<MailReportUserDTO> MailReportUserList { get; set; }
        public List<MailDefinitionDto> MailDefinitionList { get; set; }
        public List<string> MailReportToList { get; set; }
        public List<string> MailReportCcList { get; set; }
        public List<string> MailReportAllList { get; set; }
        public string CreatedDateDisplay { get { return CreatedDate.ToString("dd/MM/yyyy"); } }
        public string CreatedByName { get; set; }
    }
}
