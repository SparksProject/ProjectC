using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SparksX.DTO;
using SparksX.Service.Interface;

namespace SparksX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SparksArchiveController : Controller
    {
        private readonly ISparksArchiveService _service;
        private readonly ICompanyService _comservice;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SparksArchiveController(ISparksArchiveService service, ICompanyService comservice, IHostingEnvironment hostingEnvironment)
        {
            _service = service;
            _comservice = comservice;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("List")]
        public IActionResult List([FromForm] ArchiveFiltersDTO obj)
        {
            var result = _service.List(obj);

            var downloadPath = Path.Combine(_hostingEnvironment.ContentRootPath, "files", "sparksarchive", "download");

            if (!System.IO.File.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            foreach (var item in result.Result as List<ViewSparksArchiveDTO>)
            {
                var fullPath = item.ArsivPath;

                if (!System.IO.File.Exists(fullPath))
                {
                    continue;
                }

                var newFileName = $"{item.Id}{Path.GetExtension(fullPath)}";

                var toCopyFile = Path.Combine(downloadPath, newFileName);

                System.IO.File.Copy(fullPath, toCopyFile, true);

                item.ArsivPath = toCopyFile;

                item.DosyaAdi = newFileName;
            }

            try
            {
                var files = new DirectoryInfo(downloadPath).GetFiles().Where(x => x.CreationTime.Date != DateTime.Now.Date);

                try
                {
                    foreach (var file in files)
                    {
                        file.Delete();
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result.Result);
                case Enums.ResponseMessage.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
                case Enums.ResponseMessage.NOTFOUND:
                    return StatusCode(StatusCodes.Status404NotFound);
                case Enums.ResponseMessage.UNAUTHORIZED:
                    return StatusCode(StatusCodes.Status401Unauthorized);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpPost("Add")]
        public IActionResult Post([FromForm] SparksArchiveDTO obj)
        {
            var result = _service.Add(obj);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result.Result);
                case Enums.ResponseMessage.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
                case Enums.ResponseMessage.NOTFOUND:
                    return StatusCode(StatusCodes.Status404NotFound);
                case Enums.ResponseMessage.UNAUTHORIZED:
                    return StatusCode(StatusCodes.Status401Unauthorized);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(Guid id)
        {
            var result = _service.Get(id).Result;
            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _service.Delete(id).Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(SparksArchiveDTO obj)
        {

            var result = _service.Edit(obj);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result.Result);
                case Enums.ResponseMessage.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
                case Enums.ResponseMessage.NOTFOUND:
                    return StatusCode(StatusCodes.Status404NotFound);
                case Enums.ResponseMessage.UNAUTHORIZED:
                    return StatusCode(StatusCodes.Status401Unauthorized);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpPost("Upload")]
        public IActionResult UploadFile(Guid id) // acrhiveId, Guid id,  List<IFormFile> files
        {
            var obj = _service.Get(id).Result as SparksArchiveDTO;

            var postedFiles = HttpContext.Request.Form.Files;

            if (postedFiles != null && postedFiles.Count > 0)
            {
                try
                {
                    var newFilesNames = new List<string>();

                    var contentPath = Path.Combine(_hostingEnvironment.ContentRootPath, "files", "sparksarchive", $"{id}");
                    var fromZipDirectory = Path.Combine(contentPath, "temp");

                    if (!Directory.Exists(fromZipDirectory))
                    {
                        Directory.CreateDirectory(fromZipDirectory);
                    }

                    foreach (var postedFile in postedFiles)
                    {
                        try
                        {
                            var fileExtension = Path.GetExtension(postedFile.FileName);
                            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(postedFile.FileName);
                            var newFileNameWithOldName = Path.Combine(fileNameWithoutExtension + fileExtension);

                            if (System.IO.File.Exists(Path.Combine(fromZipDirectory, newFileNameWithOldName)) || !IsValidFileName(fileNameWithoutExtension))
                            {
                                newFileNameWithOldName = Path.Combine(Guid.NewGuid() + fileExtension);
                            }

                            if (postedFile.Length > 0)
                            {
                                using (var fileStream = new FileStream(Path.Combine(fromZipDirectory, newFileNameWithOldName), FileMode.Create))
                                {
                                    postedFile.CopyTo(fileStream);
                                }

                                newFilesNames.Add(newFileNameWithOldName);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    var toZipDirectory = Path.Combine($"{_comservice.GetArchivePath().Result}", $"{obj.InsDate.Year}", $"{obj.InsDate.Month}", $"{obj.InsDate.Day}");

                    if (!Directory.Exists(toZipDirectory))
                    {
                        Directory.CreateDirectory(toZipDirectory);
                    }

                    // dosyanın son hali "D" nin altında
                    System.IO.Compression.ZipFile.CreateFromDirectory(fromZipDirectory, Path.Combine(toZipDirectory, $"{id}.zip"));

                    Directory.Delete(contentPath, true);

                    //var result = _service.Edit(obj);

                    //switch (result.ResultMessage)
                    //{
                    //    case Enums.ResponseMessage.OK: return StatusCode(StatusCodes.Status200OK, result);
                    //    case Enums.ResponseMessage.ERROR:
                    //    case Enums.ResponseMessage.NOTFOUND:
                    //    case Enums.ResponseMessage.UNAUTHORIZED:
                    //    default: return StatusCode(StatusCodes.Status500InternalServerError, result);
                    //}

                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex);
                }
            }
            else
            {
                return NotFound();
            }
        }

        // Helpers
        private bool IsValidFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (name.Length > 1 && name[1] == ':')
            {
                if (name.Length < 4 || name.ToLower()[0] < 'a' || name.ToLower()[0] > 'z' || name[2] != '\\') return false;
                name = name.Substring(3);
            }
            if (name.StartsWith("\\\\")) name = name.Substring(1);
            if (name.EndsWith("\\") || !name.Trim().Equals(name) || name.Contains("\\\\") ||
                name.IndexOfAny(Path.GetInvalidFileNameChars().Where(x => x != '\\').ToArray()) >= 0) return false;
            return true;
        }
    }
}