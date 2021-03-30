using ProjectC.DTO;
using System;

namespace ProjectC.Service.Interface
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