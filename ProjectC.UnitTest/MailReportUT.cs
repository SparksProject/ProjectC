using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectC.Data.Repository;
using ProjectC.DTO;
using ProjectC.Service;
using ProjectC.Service.Interface;

namespace ProjectC.UnitTest
{
    [TestClass]
    public class MailReportUT
    {
        private IUnitOfWork _uow;
        private IMailService _mailService;
        private IOptions<SparksConfig> _options;

        [TestInitialize]
        public void Setup()
        {
            _uow = new UnitOfWork();
            _mailService = new MailService();
        }

        //[TestMethod]
        //public void Add()
        //{
        //    MailReportDTO obj = new MailReportDTO
        //    {
        //        MailReportName = "G�nl�k rapor",
        //        Subject = "Rapor ba�l���",
        //        BodyTemplate = "body template",
        //        SqlScript = "select * from User",
        //        PeriodTypeId = (int)Enums.PeriodType.Gunluk,
        //        CreatedBy = 1
        //    };

        //    obj.MailReportUserList = new List<MailReportUserDTO>
        //    {
        //        new MailReportUserDTO
        //        {
        //            EmailAddress = "bulentbasyurt@gmail.com",
        //            ReceiverTypeId = 1
        //        },

        //        new MailReportUserDTO
        //        {
        //            EmailAddress = "bulentbasyurt@hotmail.com",
        //            ReceiverTypeId = 2
        //        }
        //    };

        //    var mailMock = new Mock<IMailService>();

        //    var service = new MailReportService(_uow, mailMock.Object);

        //    var result = service.Add(obj);

        //    Assert.IsTrue(result.IsSuccesful);
        //}

        //[TestMethod]
        //public void SendMailReport()
        //{
        //    var service = new MailReportService(_uow, _mailService);

        //    var mailReport = service.Get(8).Result as MailReportDTO;

        //    var dataSet = service.GetScriptData(mailReport.SqlScript).Result as DataSet;

        //    var excelFile = service.SaveExcel(dataSet).Result as byte[];

        //    var mailResult = service.SendReportMail(excelFile, mailReport);

        //    //var sentMail = service.SaveSentMails(mailResult);

        //    Assert.IsTrue(true);
        //}
    }
}