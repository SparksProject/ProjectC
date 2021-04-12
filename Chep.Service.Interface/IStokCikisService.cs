using Chep.DTO;

using System.Collections.Generic;

namespace Chep.Service.Interface
{
    public interface IStokCikisService
    {
        ResponseDTO List(int? referansNo, string beyannameNo, string tpsNo);
        ResponseDTO Add(ChepStokCikisDTO obj);
        ResponseDTO Edit(ChepStokCikisDTO obj);
        ResponseDTO Get(int id);
        ResponseDTO AddDetail(int stokCikisId, List<ViewStokDusumListeDto> details);
        ResponseDTO GetStokDusumListe(string itemNo, int cikisAdet);
    }
}