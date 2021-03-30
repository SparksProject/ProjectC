using ProjectC.DTO;

namespace ProjectC.Service.Interface
{
    public interface IMailService
    {
        ResponseDTO SendMail(MailDTO obj);
    }
}