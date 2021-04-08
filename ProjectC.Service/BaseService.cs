using Chep.DTO;
using System;

namespace Chep.Service
{
    public class BaseService : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public ResponseDTO Success(object obj, string message = null)
        {
            return new ResponseDTO
            {
                IsSuccesful = true,
                Result = obj,
                ResultMessage = Enums.ResponseMessage.OK,
                Message = message,
            };
        }

        public ResponseDTO Error(Exception ex)
        {
            return new ResponseDTO
            {
                IsSuccesful = false,
                Result = null,
                ResultMessage = Enums.ResponseMessage.ERROR,
                Exception = ex
            };
        }

        public ResponseDTO NotFound()
        {
            return new ResponseDTO
            {
                IsSuccesful = false,
                Result = null,
                ResultMessage = Enums.ResponseMessage.NOTFOUND
            };
        }
        public ResponseDTO Warning(string message)
        {
            return new ResponseDTO
            {
                ResultMessage = Enums.ResponseMessage.WARNING,
                Message = message,
            };
        }

        public ResponseDTO Unauthorized()
        {
            return new ResponseDTO
            {
                IsSuccesful = false,
                Result = null,
                ResultMessage = Enums.ResponseMessage.UNAUTHORIZED
            };
        }
    }
}