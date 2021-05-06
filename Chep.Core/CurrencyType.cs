using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class CurrencyType
    {
        public Guid CurrencyTypeId { get; set; }
        public string EdiCode { get; set; }
        public string CurrencyTypeName { get; set; }
        public bool? Status { get; set; }
    }
}
