using Chep.DTO;

namespace Chep.Service.Interface
{
    public interface ITeminatService
    {
        ResponseDTO List();
        ResponseDTO Add(TeminatDTO obj);
        ResponseDTO Edit(TeminatDTO obj);
        ResponseDTO Get(int id);
    }
}
