using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class Customs
    {
        public int CustomsId { get; set; }
        public string EdiCode { get; set; }
        public string CustomsName { get; set; }
        public bool Status { get; set; }
    }
}
