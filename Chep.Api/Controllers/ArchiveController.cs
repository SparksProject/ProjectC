using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

using Ionic.Zip;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ProjectC.Core;
using ProjectC.DTO;

namespace ProjectC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveController : ControllerBase
    {
        private readonly IHostingEnvironment Environment;

        public ArchiveController(IHostingEnvironment hostingenvironment)
        {
            Environment = hostingenvironment;
        }

        private const string ZipFilePassword = "EDA201171";
        private const ExtractExistingFileAction Action = ExtractExistingFileAction.OverwriteSilently;

        [HttpPost("List")]
        public IActionResult Index([FromForm] ArchiveFiltersDTO obj)
        {
            try
            {
                using (var context = new ProjectCContext())
                {
                    var sqlUserId = new SqlParameter("@Id", SqlDbType.Int) { Value = obj.UserId };
                    var sqlTescilNo = new SqlParameter("@TescilNo", SqlDbType.NVarChar) { Value = obj.TescilNo };
                    var sqlDosyaNo = new SqlParameter("@DosyaNo", SqlDbType.NVarChar) { Value = obj.DosyaNo };
                    var sqlFaturaNo = new SqlParameter("@FaturaNo", SqlDbType.NVarChar) { Value = obj.FaturaNo };
                    var sqlTescilTarihiBaslangic = new SqlParameter("@TescilTarihi1", SqlDbType.DateTime) { Value = obj.TescilTarihiBaslangic };
                    var sqlTescilTarihiBitis = new SqlParameter("@TescilTarihi2", SqlDbType.DateTime) { Value = obj.TescilTarihiBitis };

                    var parameters = new List<SqlParameter> { sqlUserId };

                    var query = $"SELECT * FROM vw_Archive WHERE UserId = @Id";

                    if (!string.IsNullOrEmpty(obj.TescilNo))
                    {
                        query += " AND TescilNo = @TescilNo";

                        parameters.Add(sqlTescilNo);
                    }
                    if (!string.IsNullOrEmpty(obj.DosyaNo))
                    {
                        query += " AND DosyaNo = @DosyaNo";

                        parameters.Add(sqlDosyaNo);
                    }
                    if (!string.IsNullOrEmpty(obj.FaturaNo))
                    {
                        query += " AND FaturaNo = @FaturaNo";

                        parameters.Add(sqlFaturaNo);
                    }
                    if (obj.TescilTarihiBaslangic.HasValue && obj.TescilTarihiBitis.HasValue)
                    {
                        query += " AND TescilTarihi BETWEEN @TescilTarihi1 and @TescilTarihi2";

                        parameters.Add(sqlTescilTarihiBaslangic);
                        parameters.Add(sqlTescilTarihiBitis);
                    }


                    var contentPath = Path.Combine(Environment.ContentRootPath, "files", "arsivler");
                    var zipFileForTest = Path.Combine(Environment.ContentRootPath, "files", "0A771983-606C-45CE-9D48-5BF2C5A8CE72.zip");

                    var target = new List<Arsiv>();

                    foreach (var item in context.Arsiv.FromSql(query, parameters.ToArray<object>()))
                    {
                        try
                        {
#if DEBUG
                            item.ArsivPath = zipFileForTest;
#endif

                            using (ZipFile zipFile = ZipFile.Read(item.ArsivPath))
                            {
                                var unzipDirectory = Path.Combine(contentPath, item.Id.ToString());

                                foreach (ZipEntry zipEntry in zipFile)
                                {
                                    zipEntry.ExtractWithPassword(unzipDirectory, Action, ZipFilePassword);
                                }

                                var zipFileName = Path.Combine(contentPath, $"{item.Id}.zip");

                                if (System.IO.File.Exists(zipFileName))
                                {
                                    System.IO.File.Delete(zipFileName);
                                }

                                System.IO.Compression.ZipFile.CreateFromDirectory(unzipDirectory, zipFileName);
                            }

                            target.Add(item);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    try
                    {
                        var directories = new DirectoryInfo(contentPath).GetDirectories();

                        foreach (var item in target)
                        {
                            try
                            {
                                foreach (var dir in directories)
                                {
                                    if (dir.Name == item.Id.ToString())
                                    {
                                        dir.Delete(true);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                    return Ok(target);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}