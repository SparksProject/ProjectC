using ProjectC.DTO;
using System;

namespace ProjectC.Service.Interface
{
    public interface IInvoiceService
    {
        ResponseDTO List(Guid id);
        ResponseDTO Add(InvoiceDTO obj);
        ResponseDTO Edit(InvoiceDTO obj);
        ResponseDTO Get(Guid id);
    }
}
