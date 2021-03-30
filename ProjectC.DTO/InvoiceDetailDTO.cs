using System;

namespace ProjectC.DTO
{
    public class InvoiceDetailDTO
    {
        public Guid InvoiceDetailId { get; set; }
        public Guid InvoiceId { get; set; }
        public string HsCode { get; set; }
        public string DescGoods { get; set; }
        public string ProductNo { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Uom { get; set; }
        public double ActualQuantity { get; set; }
        public double InvoiceQuantity { get; set; }
        public double? GrossWeight { get; set; }
        public double NetWeight { get; set; }
        public string IntrnlAgmt { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double InvoiceAmount { get; set; }
        public string PkgType { get; set; }
        public string CommclDesc { get; set; }
        public int NumberOfPackages { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
