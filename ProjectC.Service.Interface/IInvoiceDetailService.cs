using ProjectC.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectC.Service.Interface
{
    public interface IInvoiceDetailService
    {
        ResponseDTO List(Guid id);
        ResponseDTO Add(InvoiceDTO obj);
        ResponseDTO Edit(InvoiceDTO obj);
        ResponseDTO Get(Guid id);
    }
}
