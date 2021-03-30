using System;

namespace SparksX.Data.Models
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