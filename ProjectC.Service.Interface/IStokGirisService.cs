using ProjectC.DTO;

namespace ProjectC.Service.Interface
{
    public interface IStokGirisService
    {
        ResponseDTO List();
        ResponseDTO Add(ChepStokGirisDTO obj);
        ResponseDTO Edit(ChepStokGirisDTO obj);
        ResponseDTO Get(int id);

        ResponseDTO ListDetails();
    }
}