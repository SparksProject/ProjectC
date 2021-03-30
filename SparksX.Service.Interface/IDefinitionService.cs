using SparksX.DTO;
using System;

namespace SparksX.Service.Interface
{
    public interface IDefinitionService
    {
        ResponseDTO GetPeriodTypes();
        ResponseDTO GetRecordStatuses();
        ResponseDTO GetUsers();
        ResponseDTO GetCustomers();
        ResponseDTO GetMailDefinitions();
        ResponseDTO GetUserTypes();
        ResponseDTO GetParameterTypes();
    }
}