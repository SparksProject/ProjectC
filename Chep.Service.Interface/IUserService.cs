using Chep.DTO;

namespace Chep.Service.Interface
{
    public interface IUserService
    {
        ResponseDTO List();
        ResponseDTO Add(UserDTO obj);
        ResponseDTO Edit(UserDTO obj);
        ResponseDTO GetUser(int id);
        ResponseDTO Get(string token, string secret);
        ResponseDTO Authenticate(string userName, string password);
        bool ValidateToken(string token);
    }
}