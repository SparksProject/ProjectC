using System;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;

using SparksX.Data.Repository;
using SparksX.Service;
using SparksX.Service.Interface;

using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;
using System.Collections.Generic;
using SparksX.DTO;
using System.Linq;

namespace SparksX.WinService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var serviceProvider = new ServiceCollection()
                       .AddSingleton<IMailReportService, MailReportService>()
                       .AddSingleton<IUnitOfWork, UnitOfWork>()
                       .AddSingleton<IMailService, MailService>()
                       .BuildServiceProvider();

                var service = serviceProvider.GetService<IMailReportService>();

                var now = DateTime.Now;

                var mailReportList = service.List().Result as List<MailReportDTO>;

#if DEBUG
                mailReportList = mailReportList.Where(x => x.MailReportId == 1).ToList();
#endif

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

                    //var taskResult = SetupJobs(item.PeriodValue);
#if DEBUG
                    service.SendScheduledMailReports(item.MailReportId, now);
#endif

                    if (/*(times.Contains(now.Hour.ToString()) && times.Count > 0) ||*/ (days.Contains(now.Day.ToString()) && days.Count > 0))
                    {
                        service.SendScheduledMailReports(item.MailReportId, now);
                    }
                }


                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kapanıyor!");
            }
        }

        private static List<string> GetPeriods(string value)
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

        public static async Task SetupJobs(string cronExpression)
        {
            var _scheduler = await new StdSchedulerFactory().GetScheduler();
            await _scheduler.Start();

            var userEmailsJob = JobBuilder.Create<SendUserEmailsJob>()
                .WithIdentity("SendUserEmailsJob")
                .Build();

            var userEmailsTrigger = TriggerBuilder.Create()
                .WithIdentity("SendUserEmailsCron")
                .StartNow()
                .WithCronSchedule(cronExpression) // 0 * * ? * *
                .Build();

            var result = await _scheduler.ScheduleJob(userEmailsJob, userEmailsTrigger);
        }

        public class SendUserEmailsJob : IJob
        {
            public SendUserEmailsJob(IMailReportService service, int reportId, DateTime sentDate)
            {
                Service = service;
                ReportId = reportId;
                SentDate = sentDate;
            }

            public int ReportId { get; set; }
            public DateTime SentDate { get; set; }

            private readonly IMailReportService Service;

            public Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine("İşlem Başlıyor...");

                Service.SendScheduledMailReports(ReportId, SentDate);

                Console.WriteLine("İşlem Bitti...");
                Console.WriteLine();

                return Task.CompletedTask;
            }
        }
    }
}