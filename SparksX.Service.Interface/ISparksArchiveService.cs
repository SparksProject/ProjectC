using SparksX.DTO;
using System;

namespace SparksX.Service.Interface
{
    public interface ISparksArchiveService
    {
        ResponseDTO List(ArchiveFiltersDTO obj);
        ResponseDTO Add(SparksArchiveDTO obj);
        ResponseDTO Edit(SparksArchiveDTO obj);
        ResponseDTO Get(Guid id);
        ResponseDTO AddRange(Guid customerId, string dosyaTipi, string dosyaNo, string belgeAdi, string dosyaYolu);
        ResponseDTO Delete(Guid id);
    }
}
