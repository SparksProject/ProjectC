using SparksX.DTO;
using System;

namespace SparksX.Service.Interface
{
    public interface ICompanyService
    {
        ResponseDTO Edit(CompanyDTO obj);
        ResponseDTO Get(Guid id);
        ResponseDTO GetArchivePath();
    }
}
