using Chep.DTO;

namespace Chep.Service.Interface
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
        ResponseDTO GetCustoms();
        ResponseDTO GetUnits();
        ResponseDTO GetCountries();
        ResponseDTO GetCurrencyTypes();
        ResponseDTO GetProducts();
        ResponseDTO GetPaymentMethods();
        ResponseDTO GetDeliveryTerms();
        ResponseDTO GetNextReferenceNumber(string stockType);
    }
}