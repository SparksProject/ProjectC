using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class Product
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public string ProductNo { get; set; }
        public string ProductNameTr { get; set; }
        public string ProductNameEng { get; set; }
        public string ProductNameOrg { get; set; }
        public string HsCode { get; set; }
        public string Uom { get; set; }
        public double? GrossWeight { get; set; }
        public double? NetWeight { get; set; }
        public string SapCode { get; set; }
        public string CountryOfOrigin { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
    }
}
