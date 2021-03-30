using ProjectC.DTO;
using System;

namespace ProjectC.Service.Interface
{
    public interface IWorkordermasterService
    {
        ResponseDTO List();
        ResponseDTO Add(WorkOrderMasterDTO obj);
        ResponseDTO Edit(WorkOrderMasterDTO obj);
        ResponseDTO Get(Guid id);
    }
}
