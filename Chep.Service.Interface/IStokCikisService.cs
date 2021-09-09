using Chep.DTO;
using System;
using System.Collections.Generic;

namespace Chep.Service.Interface
{
    public interface IStokCikisService
    {
        ResponseDTO List(int? referansNo, string beyannameNo, string tpsNo);
        ResponseDTO Add(ChepStokCikisDTO obj);
        ResponseDTO Edit(ChepStokCikisDTO obj);
        ResponseDTO WorkOrderStatusEdit(ChepStokCikisDTO obj);
        ResponseDTO Get(int id);
        ResponseDTO GetByUrunKod(string id);
        ResponseDTO Delete(int id);
        ResponseDTO AddDetail(int stokCikisId, List<ViewStokDusumListeDto> details);
        ResponseDTO GetStokDusumListe(string itemNo, int cikisAdet, Guid customerId);
        ResponseDTO StokDusumListeAdd(string itemNo, int cikisAdet, List<ChepStokCikisDetayDTO> details);
    }
}