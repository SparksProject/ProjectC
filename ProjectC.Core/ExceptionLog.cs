using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class ExceptionLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string SpecialMessage { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public DateTime ExceptionDate { get; set; }
    }
}
