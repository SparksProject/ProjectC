using ProjectC.DTO;
using System;

namespace ProjectC.Service.Interface
{
    public interface ICompanyService
    {
        ResponseDTO Edit(CompanyDTO obj);
        ResponseDTO Get(Guid id);
        ResponseDTO GetArchivePath();
    }
}
