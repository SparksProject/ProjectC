using System;

namespace Chep.DTO
{
    public class WorkOrderMasterDTO
    {
        public Guid WorkOrderMasterId { get; set; }
        public string WorkOrderNo { get; set; }
        public string DeclarationType { get; set; }
        public int Status { get; set; }
        public Guid? MasterId { get; set; }
        public Guid CustomerId { get; set; }
        public string Message { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public string CustomerName { get; set; }
        public string RecordStatusName { get; set; }
        public string CreatedDateDisplay { get { return CreatedDate.ToString("dd/MM/yyyy HH:mm"); } }
        public string CreatedByName { get; set; }
        public string ModifiedDateDisplay { get { return ModifiedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string ModifiedByName { get; set; }
        public string DeletedDateDisplay { get { return DeletedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string DeletedByName { get; set; }
    }
}
