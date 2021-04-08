using Chep.DTO;
using System.Collections.Generic;

namespace Chep.Service.Interface
{
    public interface IGenericReportService
    {
        ResponseDTO List(int id);
        ResponseDTO Add(GenericReportDTO obj);
        ResponseDTO Get(int id);
        ResponseDTO Edit(GenericReportDTO obj);
        ResponseDTO GetResultSet(int id, int userId, List<GenericReportParameterDTO> parameters);
        ResponseDTO CreateExcel(int id, List<GenericReportParameterDTO> parameters);
    }
}