using ProjectC.DTO;

namespace ProjectC.Service.Interface
{
    public interface IStokGirisService
    {
        ResponseDTO List(string referansNo, string beyannameNo, string tpsNo);
        ResponseDTO Add(ChepStokGirisDTO obj);
        ResponseDTO Edit(ChepStokGirisDTO obj);
        ResponseDTO Get(int id);

        ResponseDTO ListDetails();
    }
}