using SparksX.DTO;

namespace SparksX.Service.Interface
{
    public interface IMailService
    {
        ResponseDTO SendMail(MailDTO obj);
    }
}