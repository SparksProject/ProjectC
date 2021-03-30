using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;

using ProjectC.Data.Repository;
using ProjectC.DTO;
using ProjectC.Service;
using ProjectC.Service.Interface;

namespace ProjectC.WinService
{
    public class ScheduledMailSender : IDisposable
    {
        // Disposing
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        //// Public Methods
        public void SendScheduledMailReports()
        {
            var serviceProvider = new ServiceCollection()
                   .AddSingleton<IMailReportService, MailReportService>()
                   .AddSingleton<IUnitOfWork, UnitOfWork>()
                   .AddSingleton<IMailService, MailService>()
                   .BuildServiceProvider();

            var service = serviceProvider.GetService<IMailReportService>();

            try
            {
                var now = DateTime.Now;

                var mailReportList = service.List().Result as List<MailReportDTO>;

                foreach (var item in mailReportList)
                {
                    var times = new List<string>();
                    var days = new List<string>();
                    var periodList = GetPeriods(item.PeriodValue);

                    if (item.PeriodTypeId == (int)Enums.PeriodType.Saatlik_Gunluk)
                    {
                        times = periodList;
                    }

                    if (item.PeriodTypeId == (int)Enums.PeriodType.Haftalik)
                    {
                        days = periodList;
                    }

                    if (/*(times.Contains(now.Hour.ToString()) && times.Count > 0) ||*/ (days.Contains(now.Day.ToString()) && days.Count > 0))
                    {
                        service.SendScheduledMailReports(item.MailReportId, now);
                    }
                }
            }
            catch (Exception ex)
            {
                var exLogId = service.AddExceptionLog(ex, "SendScheduledMailReports");
                Console.WriteLine("Hata Kayıt No: " + exLogId);
            }
        }

        private List<string> GetPeriods(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return new List<string>();
                }
                else
                {
                    return value.Split(";").ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }
}