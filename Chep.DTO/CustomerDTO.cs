using System;

namespace Chep.DTO
{
    public class CustomerDTO
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string OtherId { get; set; }
        public string UserNameWs { get; set; }
        public string PasswordWs { get; set; }
        public string TaxNo { get; set; }
        public string TaxName { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public string RecordStatusName { get; set; }
        public string CreatedDateDisplay { get { return CreatedDate.ToString("dd/MM/yyyy HH:mm"); } }
        public string CreatedByName { get; set; }
        public string ModifiedDateDisplay { get { return ModifiedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string ModifiedByName { get; set; }
        public string DeletedDateDisplay { get { return DeletedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string DeletedByName { get; set; }
    }
}
