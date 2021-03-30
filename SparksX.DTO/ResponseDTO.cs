using System;

namespace SparksX.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccesful { get; set; }
        public Enums.ResponseMessage ResultMessage { get; set; }
        public Exception Exception { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
    }
}