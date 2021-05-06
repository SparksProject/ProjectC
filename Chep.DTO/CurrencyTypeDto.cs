using System;

namespace Chep.DTO
{
    public class CurrencyTypeDto
    {
        public Guid CurrencyTypeId { get; set; }
        public string EdiCode { get; set; }
        public string CurrencyTypeName { get; set; }
        public bool Status { get; set; }
    }
}
