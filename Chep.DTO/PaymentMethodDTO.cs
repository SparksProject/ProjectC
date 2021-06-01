using System;
using System.Collections.Generic;
using System.Text;

namespace Chep.DTO
{
    public class PaymentMethodDTO
    {
        public int PaymentMethodId { get; set; }
        public string EdiCode { get; set; }
        public string Name { get; set; }
    }
}
