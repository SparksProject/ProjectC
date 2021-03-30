using ProjectC.DTO;

using System;
using System.Collections.Generic;
using System.Data;

namespace ProjectC.Service.Interface
{
    public interface IMailReportService
    {
        ResponseDTO List();
        ResponseDTO Add(MailReportDTO obj);
        ResponseDTO Get(int id);
        ResponseDTO Edit(MailReportDTO obj);
        void SendScheduledMailReports(int mailReportId, DateTime sentDate);
        List<MailDefinitionDto> GetMailDefinitions();
        bool AddMailDefinition(MailDefinitionDto obj);
        ResponseDTO GetMailResultSet(int id, int userId);
        ResponseDTO GetScriptData(string script);
        ResponseDTO SaveExcel(DataSet dataSet);
        int AddExceptionLog(Exception ex, string specialMessage = null);
    }
}