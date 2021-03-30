namespace ProjectC.DTO
{
    public class MailReportUserDTO
    {
        public int MailReportUserId { get; set; }
        public int MailReportId { get; set; }
        public int MailDefinitionId { get; set; }
        public byte ReceiverTypeId { get; set; }

        public string EmailAddress { get; set; }
        public string RecipientName { get; set; }
        public string ReceiverTypeName { get; set; }
    }
}
