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

using SparksX.Data.Models;

namespace SparksX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveDetailController : ControllerBase
    {
        private readonly IHostingEnvironment Environment;

        public ArchiveDetailController(IHostingEnvironment hostingenvironment)
        {
            Environment = hostingenvironment;
        }

        private const string ZipFilePassword = "EDA201171";
        private const ExtractExistingFileAction Action = ExtractExistingFileAction.OverwriteSilently;

        [HttpGet("List/{id}")]
        public IActionResult List(int id)
        {
            using (SparksXContext context = new SparksXContext())
            {
                var DosyaNoParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };

                //var wwwPath = Path.Combine(Environment.WebRootPath, "files", "arsivler");
                var contentPath = Path.Combine(Environment.ContentRootPath, "files", "arsivler");
                var zipFileForTest = Path.Combine(Environment.ContentRootPath, "files", "0A771983-606C-45CE-9D48-5BF2C5A8CE72.zip");

                //var GetArsivPdf = context.ArsivDetails.FromSql("Select * From vw_ArchivePath @DosyaNo", DNO).ToList();
                var list = context.Arsiv.FromSql($"Select * from vw_Archive where (Id = {DosyaNoParameter})").ToList();
                var target = new List<Arsiv>();

                foreach (var item in list)
                {
                    try
                    {
#if DEBUG
                        item.ArsivPath = zipFileForTest;
#endif

                        using (Ionic.Zip.ZipFile zipFile = Ionic.Zip.ZipFile.Read(item.ArsivPath))
                        {
                            var unzipDirectory = Path.Combine(contentPath, item.DosyaNo);

                            foreach (ZipEntry zipEntry in zipFile)
                            {
                                zipEntry.ExtractWithPassword(unzipDirectory, Action, ZipFilePassword);
                            }

                            var zipFileName = Path.Combine(contentPath, $"{item.DosyaNo}.zip");

                            if (System.IO.File.Exists(zipFileName))
                            {
                                System.IO.File.Delete(zipFileName);
                            }

                            System.IO.Compression.ZipFile.CreateFromDirectory(unzipDirectory, zipFileName);

                            Directory.Delete(unzipDirectory, true);
                        }

                        target.Add(item);
                    }
                    catch (Exception)
                    {
                    }

                }

                return Ok(target);
            }
        }
    }
}