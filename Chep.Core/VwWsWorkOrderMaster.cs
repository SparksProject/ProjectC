using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class VwWsWorkOrderMaster
    {
        public int StokCikisId { get; set; }
        public Guid? WorkOrderMasterId { get; set; }
        public int WorkOrderNo { get; set; }
        public string DeclarationTypei { get; set; }
        public string UserNameWs { get; set; }
        public string PasswordWs { get; set; }
    }
}
