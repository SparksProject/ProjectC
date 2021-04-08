using Chep.DTO;

namespace Chep.Service.Interface
{
    public interface IMailService
    {
        ResponseDTO SendMail(MailDTO obj);
    }
}