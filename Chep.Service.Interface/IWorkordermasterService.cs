using Chep.DTO;
using System;

namespace Chep.Service.Interface
{
    public interface IWorkordermasterService
    {
        ResponseDTO List();
        ResponseDTO Add(WorkOrderMasterDTO obj);
        ResponseDTO Edit(WorkOrderMasterDTO obj);
        ResponseDTO Get(Guid id);
    }
}
