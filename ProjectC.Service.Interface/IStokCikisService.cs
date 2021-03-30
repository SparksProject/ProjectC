using ProjectC.DTO;

namespace ProjectC.Service.Interface
{
    public interface IStokCikisService
    {
        ResponseDTO List();
        ResponseDTO Add(ChepStokCikisDTO obj);
        ResponseDTO Edit(ChepStokCikisDTO obj);
        ResponseDTO Get(int id);
    }
}