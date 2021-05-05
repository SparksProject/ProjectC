using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using Microsoft.Extensions.Configuration;

using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;

namespace Chep.Service
{
    /// <summary>
    /// Mail Report operations class
    /// </summary>
    public class MailReportService : BaseService, IMailReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMailService _mailService;

        public MailReportService(IUnitOfWork uow, IMailService mailService)
        {
            _uow = uow;
            _mailService = mailService;
        }

        /// <summary>
        /// Lists all mail reports
        /// </summary>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO List()
        {
            try
            {
                var entities = _uow.MailReports.Set().Include(x => x.RecordStatus).Include(x => x.PeriodType).ToList();

                if (entities.Count == 0)
                {
                    return NotFound();
                }

                var list = new List<MailReportDTO>();

                foreach (var item in entities)
                {
                    var obj = new MailReportDTO
                    {
                        MailReportId = item.MailReportId,
                        MailReportName = item.MailReportName,
                        PeriodTypeName = item.PeriodType.PeriodTypeName,
                        RecordStatusName = item.RecordStatus.RecordStatusName,
                        CreatedDate = item.CreatedDate,
                        PeriodValue = item.PeriodValue,
                        PeriodTypeId = item.PeriodTypeId
                    };

                    list.Add(obj);
                }

                return Success(list);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Add a new Mail Report.
        /// </summary>
        /// <param name="obj">MailReportDTO</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Add(MailReportDTO obj)
        {
            try
            {
                obj.RecordStatusId = 1;
                obj.CreatedDate = DateTime.Now;
                var entity = Mapper.MapSingle<MailReportDTO, MailReport>(obj);

                if (obj.ToEmails != null && obj.ToEmails.Length > 0)
                {
                    foreach (var item in obj.ToEmails)
                    {
                        var mailReportUser = new MailReportUser
                        {
                            MailDefinitionId = item,
                            ReceiverTypeId = (byte)Enums.ReceiverType.To,
                        };

                        entity.MailReportUser.Add(mailReportUser);
                    }
                }

                if (obj.CcEmails != null && obj.CcEmails.Length > 0)
                {
                    foreach (var item in obj.CcEmails)
                    {
                        var mailReportUser = new MailReportUser
                        {
                            MailDefinitionId = item,
                            ReceiverTypeId = (byte)Enums.ReceiverType.Cc,
                        };

                        entity.MailReportUser.Add(mailReportUser);
                    }
                }

                var result = _uow.MailReports.Add(entity);

                bool saveResult = _uow.Commit();

                return Success(result.MailReportId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Edits the given Mail Report object.
        /// </summary>
        /// <param name="obj">MailReportDTO</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Edit(MailReportDTO obj)
        {
            try
            {
                if (obj.RecordStatusId == 1)
                {
                    obj.ModifiedDate = DateTime.Now;
                }
                else
                {
                    obj.DeletedDate = DateTime.Now;
                }

                _uow.MailReportUsers.Delete(x => x.MailReportId == obj.MailReportId);
                _uow.Commit();

                if (obj.ToEmails != null && obj.ToEmails.Length > 0)
                {
                    foreach (var item in obj.ToEmails)
                    {
                        var mailReportUser = new MailReportUser
                        {
                            MailDefinitionId = item,
                            MailReportId = obj.MailReportId,
                            ReceiverTypeId = (byte)Enums.ReceiverType.To,
                        };

                        _uow.MailReportUsers.Add(mailReportUser);
                    }
                }

                if (obj.CcEmails != null && obj.CcEmails.Length > 0)
                {
                    foreach (var item in obj.CcEmails)
                    {
                        var mailReportUser = new MailReportUser
                        {
                            MailDefinitionId = item,
                            MailReportId = obj.MailReportId,
                            ReceiverTypeId = (byte)Enums.ReceiverType.Cc,
                        };

                        _uow.MailReportUsers.Add(mailReportUser);
                    }
                }
                if (obj.PeriodDay == null)
                {
                    obj.PeriodDay = string.Empty;
                }
                var entity = Mapper.MapSingle<MailReportDTO, MailReport>(obj);

                var result = _uow.MailReports.Update(entity);

                bool saveResult = _uow.Commit();

                return Success(result.MailReportId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Gets the Mail Report by id.
        /// </summary>
        /// <param name="id">MailReportId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(int id)
        {
            var entity = _uow.MailReports.Set()
                                         .Include(x => x.RecordStatus)
                                         .Include(x => x.PeriodType)
                                         .FirstOrDefault(x => x.MailReportId == id);

            var mailList = _uow.MailReportUsers.Set()
                                               .Include(x => x.MailDefinition)
                                               .Where(x => x.MailReportId == id)
                                               .ToList();

            var result = Mapper.MapSingle<MailReport, MailReportDTO>(entity);
            result.RecordStatusName = entity.RecordStatus.RecordStatusName;
            result.PeriodTypeName = entity.PeriodType.PeriodTypeName;

            result.ToEmails = mailList.Where(x => x.ReceiverTypeId == 1).Select(x => x.MailDefinitionId).ToArray();
            result.CcEmails = mailList.Where(x => x.ReceiverTypeId == 2).Select(x => x.MailDefinitionId).ToArray();

            result.MailReportUserList = new List<MailReportUserDTO>();

            foreach (var item in mailList)
            {
                result.MailReportUserList.Add(new MailReportUserDTO
                {
                    MailReportUserId = item.MailReportUserId,
                    MailDefinitionId = item.MailDefinitionId,
                    EmailAddress = item.MailDefinition.EmailAddress,
                    RecipientName = item.MailDefinition.RecipientName,
                    ReceiverTypeId = item.ReceiverTypeId,
                    ReceiverTypeName = item.ReceiverTypeId == 1 ? "To" : "Cc"
                });
            }

            return Success(result);
        }

        public void SendScheduledMailReports(int mailReportId, DateTime startDate)
        {
            var mailReport = Get(mailReportId).Result as MailReportDTO;
            var toSentMails = GetAllReceivers(mailReport, startDate);
            toSentMails = CheckDuplicateMails(toSentMails, startDate);

            if (toSentMails == null || toSentMails.Count == 0)
            {
                return;
            }

            var resultSet = GetScriptData(mailReport.SqlScript);

            if (!resultSet.IsSuccesful)
            {
                return;
            }

            var excelFile = SaveExcel(resultSet.Result as DataSet).Result as byte[];

            var sendResult = SendReportMail(excelFile, mailReport);

            if (sendResult != null)
            {
                SaveSentMails(toSentMails, startDate);
            }
        }

        public List<MailDefinitionDto> GetMailDefinitions()
        {
            var entities = _uow.MailDefinitions.GetAll().OrderByDescending(x => x.CreatedDate).ToList();

            if (entities.Count == 0)
            {
                return null;
            }

            List<MailDefinitionDto> list = new List<MailDefinitionDto>();

            foreach (var item in entities)
            {
                list.Add(new MailDefinitionDto { EmailAddress = item.EmailAddress, RecipientName = item.RecipientName, CreatedDate = item.CreatedDate });
            }

            return list;
        }

        public bool AddMailDefinition(MailDefinitionDto obj)
        {
            try
            {
                bool isExisting = _uow.MailDefinitions.Any(x => x.EmailAddress == obj.EmailAddress);

                if (isExisting)
                {
                    return false;
                }

                MailDefinition mailDefinition = new MailDefinition
                {
                    RecipientName = obj.RecipientName,
                    EmailAddress = obj.EmailAddress,
                    CreatedDate = DateTime.Now
                };

                _uow.MailDefinitions.Add(mailDefinition);
                return _uow.Commit();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the sql script and shows the report.
        /// </summary>
        /// <param name="id">GenericReportId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO GetMailResultSet(int id, int userId)
        {
            try
            {
                if (userId < 1)
                {
                    return Unauthorized();
                }

                var entity = _uow.MailReports.Single(x => x.MailReportId == id);

                using (var sqlDataAdapter = new SqlDataAdapter(entity.SqlScript, GetConnectionString()))
                {
                    using (var dataTable = new DataTable())
                    {
                        sqlDataAdapter.Fill(dataTable);

                        var result = dataTable.ToDynamicList(userId);

                        return Success(result);
                    }
                }
            }
            catch (ArgumentException ex) // ToDynamicList metodunda kullanılıyor.
            {
                return Warning(ex.Message);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }


        // Privates
        private List<SentMailDTO> CheckDuplicateMails(List<SentMailDTO> toSentMails, DateTime endDate)
        {
            if (toSentMails == null || toSentMails.Count == 0)
            {
                return toSentMails;
            }

            var beginDate = endDate.AddYears(-1);
            var sendedMails = _uow.SentMails.Search(x => x.MailReportId == toSentMails.FirstOrDefault().MailReportId && x.SentDate >= beginDate);

            if (sendedMails.Count == 0)
            {
                return toSentMails;
            }

            var periodTypeId = sendedMails.FirstOrDefault().MailReport.PeriodTypeId;

            if (periodTypeId == 1) // günlük
            {
                beginDate = endDate.Date;
            }
            else if (periodTypeId == 2) // saatlik
            {
                beginDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, 0, 0);
            }
            else if (periodTypeId == 3) // haftalik
            {
                beginDate = endDate.AddDays((int)DayOfWeek.Monday - (int)endDate.Date.DayOfWeek);
            }
            else if (periodTypeId == 4) // aylık
            {
                beginDate = new DateTime(endDate.Year, endDate.Month, 1);
            }
            else if (periodTypeId == 5) // tekrar eden tarih
            {

            }

            var sendedAddresses = sendedMails.Where(x => x.SentDate >= beginDate).Select(y => y.EmailAddress).ToList();

            //foreach (var item in sendedMails)
            //{
            //    //if (toSentMailAddresses.Contains(item.EmailAddress))
            //    //{
            //    //    toSentMails.RemoveAll(x => x.EmailAddress == item.EmailAddress);
            //    //}

            //    toSentMails.RemoveAll(x => x.EmailAddress == item.EmailAddress);
            //}

            toSentMails.RemoveAll(x => sendedAddresses.Contains(x.EmailAddress));

            return toSentMails;
        }

        private string GetConnectionString()
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            //   .AddJsonFile("appsettings.json")
            //   .Build();

            //var connectionString = configuration.GetSection("SparksXConfig")["ConnectionString"];

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
            var appconfig = configuration.GetSection("SparksXConfig").Get<SparksConfig>();

            return appconfig.ConnectionString;
        }

        /// <summary>
        /// Sends the mail report email.
        /// </summary>
        /// <param name="file">Excel file</param>
        /// <param name="mailReport">MailReport object</param>
        /// <returns>bool</returns>
        private List<bool> SendReportMail(byte[] file, MailReportDTO mailReport)
        {
            if (mailReport == null || mailReport.MailReportUserList == null)
            {
                return null;
            }

            var company = _uow.Companies.Single(x => x.CompanyId != Guid.NewGuid()); // zaten toplamda bir tane company olacak

            var sendResult = new List<bool>();

            using (var stream = new MemoryStream(file))
            using (var attachment = new Attachment(stream, mailReport.MailReportName + ".xlsx"))
            {
                var ccList = mailReport.MailReportUserList.Where(x => x.ReceiverTypeId == (byte)Enums.ReceiverType.Cc);

                for (int i = 0; i < mailReport.MailReportUserList.Where(x => x.ReceiverTypeId == (byte)Enums.ReceiverType.To).Count(); i++)
                {
                    var item = mailReport.MailReportUserList[i];

                    var mail = new MailDTO
                    {
                        Host = company.SmtpHost,
                        Port = company.SmtpPort,
                        UserName = company.SmtpUserName,
                        Password = company.SmtpPassword,
                        Subject = mailReport.Subject,
                        Body = mailReport.BodyTemplate.Replace("#FullName#", item.RecipientName),
                        Attachment = attachment,
                        To = new List<string> { item.EmailAddress },
                    };

                    if (i == 0 && ccList != null && ccList.Count() > 0)
                    {
                        mail.Cc = ccList.Select(x => x.EmailAddress).ToList();
                    }

                    var mailSendResult = (bool)_mailService.SendMail(mail).Result;

                    sendResult.Add(mailSendResult);
                }

            }

            return sendResult;
        }

        private List<SentMailDTO> GetAllReceivers(MailReportDTO obj, DateTime sentDate)
        {
            var sentMails = new List<SentMailDTO>();

            if (obj == null || obj.MailReportUserList == null)
            {
                return sentMails;
            }

            obj.MailReportUserList.ForEach(x => sentMails.Add(new SentMailDTO
            {
                MailReportId = obj.MailReportId,
                Subject = obj.Subject,
                Body = obj.BodyTemplate,
                EmailAddress = x.EmailAddress,
                SentDate = sentDate
            }));

            return sentMails;
        }

        /// <summary>
        /// Logs the sent emails.
        /// </summary>
        /// <param name="obj">List<SentMailDTO></param>
        /// <returns>ResponseDTO</returns>
        private ResponseDTO SaveSentMails(List<SentMailDTO> obj, DateTime sendDate)
        {
            foreach (var item in obj)
            {
                item.SentDate = sendDate;
                var entity = Mapper.MapSingle<SentMailDTO, SentMail>(item);

                var result = _uow.SentMails.Add(entity);
            }

            var saveResult = _uow.Commit();

            return Success(saveResult);
        }

        /// <summary>
        /// Converts the given dataset to an excel file as byte array.
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO SaveExcel(DataSet dataSet)
        {
            using (var ms = new MemoryStream())
            using (var workbook = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new Sheets();

                uint sheetId = 1;

                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                if (sheets.Elements<Sheet>().Any())
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                var stylesPart = workbook.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = new Stylesheet();

                // blank font list
                stylesPart.Stylesheet.Fonts = new Fonts();
                stylesPart.Stylesheet.Fonts.AppendChild(new Font());
                stylesPart.Stylesheet.Fonts.AppendChild(new Font(new Bold(), new FontSize() { Val = 11 }, new FontName() { Val = "Arial" }));
                stylesPart.Stylesheet.Fonts.AppendChild(new Font(new FontSize() { Val = 10 }, new FontName() { Val = "Arial" }));
                //stylesPart.Stylesheet.Fonts.Count = 2;

                // create fills
                stylesPart.Stylesheet.Fills = new Fills();

                // create a solid red fill
                var solidRed = new PatternFill() { PatternType = PatternValues.Solid };
                solidRed.ForegroundColor = new ForegroundColor { Rgb = HexBinaryValue.FromString("FFFF0000") }; // red fill
                solidRed.BackgroundColor = new BackgroundColor { Indexed = 64 };

                stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.None } }); // required, reserved by Excel
                stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = new PatternFill { PatternType = PatternValues.Gray125 } }); // required, reserved by Excel
                stylesPart.Stylesheet.Fills.AppendChild(new Fill { PatternFill = solidRed });
                //stylesPart.Stylesheet.Fills.Count = 3;

                // blank border list
                stylesPart.Stylesheet.Borders = new Borders();
                stylesPart.Stylesheet.Borders.AppendChild(new Border());
                stylesPart.Stylesheet.Borders.AppendChild(new Border()
                {
                    RightBorder = new RightBorder { Style = BorderStyleValues.Thin },
                    LeftBorder = new LeftBorder { Style = BorderStyleValues.Thin },
                    TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
                    BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
                }); // border 1
                //stylesPart.Stylesheet.Borders.Count = 2;

                // blank cell format list
                stylesPart.Stylesheet.CellStyleFormats = new CellStyleFormats();
                //stylesPart.Stylesheet.CellStyleFormats.Count = 1;
                stylesPart.Stylesheet.CellStyleFormats.AppendChild(new CellFormat());

                // cell format list
                stylesPart.Stylesheet.CellFormats = new CellFormats();
                stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat()); // empty one for index 0, seems to be required
                stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat
                {
                    FormatId = 0,
                    BorderId = 1,
                    FontId = 2,
                    FillId = 0
                });

                //// cell format references style format 0, font 0, border 0, fill 2 and applies the fill
                //stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = 1, BorderId = 0, FillId = 2, ApplyFill = true }).AppendChild(new Alignment { Horizontal = HorizontalAlignmentValues.Center });

                stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat // header cell format
                {
                    FormatId = 0,
                    BorderId = 1,
                    FontId = 1,
                    FillId = 0
                });
                //stylesPart.Stylesheet.CellFormats.Count = 3;

                stylesPart.Stylesheet.Save();


                foreach (DataTable table in dataSet.Tables)
                {
                    Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    Row headerRow = new Row();

                    List<string> columns = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(column.ColumnName);
                        cell.StyleIndex = 2;

                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);

                    var convertedList = table.ToDynamicList();

                    foreach (IDictionary<string, dynamic> row in convertedList)
                    {
                        var newRow = new Row();

                        foreach (var column in row)
                        {
                            var columnName = column.Key;

                            var tuple = column.Value as IDictionary<string, dynamic>;

                            var dataType = tuple["DataType"];
                            var value = tuple["Value"];

                            var cell = new Cell
                            {
                                StyleIndex = 1
                            };

                            if (dataType == "Decimal")
                            {
                                cell.DataType = CellValues.Number;

                                var parsedDateResult = decimal.TryParse(Convert.ToString(value), out decimal parsedResult);

                                var res = string.Format("{0:N2}", parsedResult);

                                cell.CellValue = new CellValue(res);
                            }
                            else if (dataType == "Int16" || dataType == "Int32" || dataType == "Int64")
                            {
                                cell.DataType = CellValues.Number;

                                var parsedDateResult = int.TryParse(Convert.ToString(value), out int parsedResult);

                                var res = string.Format("{0:N0}", parsedResult);

                                cell.CellValue = new CellValue(res);
                            }
                            else if (dataType == "DateTime")
                            {
                                cell.DataType = CellValues.Date;

                                //var parsedDateResult = DateTime.TryParse(Convert.ToString(value), out DateTime parsedDate);

                                var res = ((string)Convert.ToString(value)).Replace("  ", " ").Replace(" 00:00:00", "");

                                cell.CellValue = new CellValue(res);
                            }
                            else // string vb.
                            {
                                cell.CellValue = new CellValue(Convert.ToString(value));
                                cell.DataType = CellValues.String;
                            }

                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }
                }


                //get your columns (where your width is set)
                //Columns sheetColumns = AutoSize(sheetData);

                //add to a WorksheetPart.WorkSheet
                //workSheetPart.Worksheet = new Worksheet();
                //sheetPart.Worksheet.Append(sheetColumns);
                //workSheetPart.Worksheet.Append(sheetData);


                workbook.Close();

                return Success(ms.ToArray());
            }
        }

        private Columns AutoSize(SheetData sheetData)
        {
            var maxColWidth = GetMaxCharacterWidth(sheetData);

            Columns columns = new Columns();
            //this is the width of my font - yours may be different
            double maxWidth = 7;
            foreach (var item in maxColWidth)
            {
                //width = Truncate([{Number of Characters} * {Maximum Digit Width} + {5 pixel padding}]/{Maximum Digit Width}*256)/256
                double width = Math.Truncate((item.Value * maxWidth + 5) / maxWidth * 256) / 256;

                //pixels=Truncate(((256 * {width} + Truncate(128/{Maximum Digit Width}))/256)*{Maximum Digit Width})
                double pixels = Math.Truncate(((256 * width + Math.Truncate(128 / maxWidth)) / 256) * maxWidth);

                //character width=Truncate(({pixels}-5)/{Maximum Digit Width} * 100+0.5)/100
                double charWidth = Math.Truncate((pixels - 5) / maxWidth * 100 + 0.5) / 100;

                Column col = new Column()
                {
                    BestFit = true,
                    Min = (uint)(item.Key + 1),
                    Max = (uint)(item.Key + 1),
                    CustomWidth = true,
                    Width = width
                };

                columns.Append(col);
            }

            return columns;
        }

        private Dictionary<int, int> GetMaxCharacterWidth(SheetData sheetData)
        {
            //iterate over all cells getting a max char value for each column
            var maxColWidth = new Dictionary<int, int>();
            var rows = sheetData.Elements<Row>();
            var numberStyles = new uint[] { 5, 6, 7, 8 }; //styles that will add extra chars
            var boldStyles = new uint[] { 1, 2, 3, 4, 6, 7, 8 }; //styles that will bold

            foreach (var r in rows)
            {
                var cells = r.Elements<Cell>().ToArray();

                //using cell index as my column
                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var cellValue = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
                    var cellTextLength = cellValue.Length;

                    if (cell.StyleIndex != null && numberStyles.Contains(cell.StyleIndex))
                    {
                        int thousandCount = (int)Math.Truncate((double)cellTextLength / 4);

                        //add 3 for '.00' 
                        cellTextLength += (3 + thousandCount);
                    }

                    if (cell.StyleIndex != null && boldStyles.Contains(cell.StyleIndex))
                    {
                        //add an extra char for bold - not 100% acurate but good enough for what i need.
                        cellTextLength += 1;
                    }

                    if (maxColWidth.ContainsKey(i))
                    {
                        var current = maxColWidth[i];
                        if (cellTextLength > current)
                        {
                            maxColWidth[i] = cellTextLength;
                        }
                    }
                    else
                    {
                        maxColWidth.Add(i, cellTextLength);
                    }
                }
            }

            return maxColWidth;
        }

        /// <summary>
        /// Executes the mail report script and returns a Dataset.
        /// </summary>
        /// <param name="script">Sql script</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO GetScriptData(string script)
        {
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(script, GetConnectionString()))
            {
                DataSet ds = new DataSet();

                sqlDataAdapter.Fill(ds);

                return Success(ds);
            }
        }

        /// <summary>
        /// Saves the given exception as a log.
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="specialMessage">Custom error label.</param>
        public int AddExceptionLog(Exception ex, string specialMessage = null)
        {
            ExceptionLog log = new ExceptionLog
            {
                Message = ex.ToString(),
                SpecialMessage = specialMessage,
                InnerException = ex.InnerException != null ? ex.InnerException.ToString() : null,
                StackTrace = ex.StackTrace,
                ExceptionDate = DateTime.Now
            };

            var exLog = _uow.ExceptionLogs.Add(log);

            _uow.Commit();

            return exLog.Id;
        }
    }
}