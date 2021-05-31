using System;

namespace Chep.DTO
{
    public class ProductDTO
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
        public decimal? UnitPrice { get; set; }
        public string CurrencyType { get; set; }

        public string CustomerName { get; set; }
        public string RecordStatusName { get; set; }
        public string CreatedDateDisplay { get { return CreatedDate.ToString("dd/MM/yyyy HH:mm"); } }
        public string CreatedByName { get; set; }
        public string ModifiedDateDisplay { get { return ModifiedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string ModifiedByName { get; set; }
        public string DeletedDateDisplay { get { return DeletedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string DeletedByName { get; set; }
        //public int[] File { get; set; }
    }
}
