using System;

namespace Chep.Core
{
    public partial class CurrencyType
    {
        public Guid CurrencyTypeId { get; set; }
        public string EdiCode { get; set; }
        public string CurrencyTypeName { get; set; }
        public bool Status { get; set; }
    }
}
