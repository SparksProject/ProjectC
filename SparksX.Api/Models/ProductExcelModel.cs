using System;

namespace SparksX.Api.Models
{
    public class ProductExcelModel
    {
        public Guid CustomerId { get; set; }
        public int CreatedBy { get; set; }
        public string File { get; set; }
    }
}