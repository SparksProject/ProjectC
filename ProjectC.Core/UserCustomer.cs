using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class UserCustomer
    {
        public int UserCustomerId { get; set; }
        public int UserId { get; set; }
        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User User { get; set; }
    }
}
