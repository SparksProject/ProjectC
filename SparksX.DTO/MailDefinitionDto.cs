using System;
namespace SparksX.DTO
{
    public class MailDefinitionDto
    {
        public int MailDefinitionId { get; set; }
        public string RecipientName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedDateDisplay { get { return CreatedDate.ToString("dd/MM/yyyy"); } }
        public string DisplayName { get; set; }
    }
}
