using Chep.DTO;

namespace Chep.Service.Interface
{
    public interface IStokGirisService
    {
        ResponseDTO List(int? referansNo, string beyannameNo, string tpsNo);
        ResponseDTO Add(ChepStokGirisDTO obj);
        ResponseDTO Edit(ChepStokGirisDTO obj);
        ResponseDTO Get(int id);

        ResponseDTO ListDetails();
    }
}