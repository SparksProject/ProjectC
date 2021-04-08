using Chep.DTO;
using System;

namespace Chep.Service.Interface
{
    public interface ICompanyService
    {
        ResponseDTO Edit(CompanyDTO obj);
        ResponseDTO Get(Guid id);
        ResponseDTO GetArchivePath();
    }
}
