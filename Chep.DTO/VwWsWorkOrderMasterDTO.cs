using System;
using System.Collections.Generic;
using System.Text;

namespace Chep.DTO
{
    public class VwWsWorkOrderMasterDTO
    {
        public int StokCikisId { get; set; }
        public Guid? WorkOrderMasterId { get; set; }
        public int WorkOrderNo { get; set; }
        public string DeclarationTypei { get; set; }
        public string UserNameWs { get; set; }
        public string PasswordWs { get; set; }
    }
}
