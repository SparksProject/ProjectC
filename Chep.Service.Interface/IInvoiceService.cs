using Chep.DTO;
using System;

namespace Chep.Service.Interface
{
    public interface IInvoiceService
    {
        ResponseDTO List(Guid id);
        ResponseDTO Add(InvoiceDTO obj);
        ResponseDTO Edit(InvoiceDTO obj);
        ResponseDTO Get(Guid id);
    }
}
