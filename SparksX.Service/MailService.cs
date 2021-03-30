using SparksX.DTO;
using SparksX.Service.Interface;
using System;
using System.Net;
using System.Net.Mail;

namespace SparksX.Service
{
    public class MailService : BaseService, IMailService
    {
        /// <summary>
        /// Sends the mail with given mail object.
        /// </summary>
        /// <param name="obj">MailDTO</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO SendMail(MailDTO obj)
        {
            if (obj == null || obj.To == null || obj.To.Count == 0)
            {
                return NotFound();
            }

            using (var client = new SmtpClient(obj.Host, obj.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(obj.UserName, obj.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            })
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(obj.UserName),
                    Subject = obj.Subject,
                    Body = obj.Body,
                    IsBodyHtml = true
                };

                foreach (var item in obj.To)
                {
                    mail.To.Add(item);
                }

                if (obj.Attachment != null)
                {
                    mail.Attachments.Add(obj.Attachment);
                }

                if (obj.Cc != null && obj.Cc.Count > 0)
                {
                    mail.CC.Add(string.Join(",", obj.Cc));
                }

                client.Send(mail);
            }

            return Success(true);
        }
    }
}