using Chep.DTO;
using System;

namespace Chep.Service.Interface
{
    public interface ICustomerService
    {
        ResponseDTO List();
        ResponseDTO Add(CustomerDTO obj);
        ResponseDTO Edit(CustomerDTO obj);
        ResponseDTO Get(Guid id);
    }
}
