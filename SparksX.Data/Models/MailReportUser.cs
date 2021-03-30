namespace SparksX.Data.Models
{
    public partial class MailReportUser
    {
        public int MailReportUserId { get; set; }
        public int MailReportId { get; set; }
        public int MailDefinitionId { get; set; }
        public byte ReceiverTypeId { get; set; }

        public virtual MailDefinition MailDefinition { get; set; }
        public virtual MailReport MailReport { get; set; }
    }
}