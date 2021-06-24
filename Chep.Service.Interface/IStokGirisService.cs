using Chep.DTO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Chep.Service.Interface
{
    public interface IStokGirisService
    {
        ResponseDTO List(int? referansNo, string beyannameNo, string tpsNo);
        ResponseDTO Add(ChepStokGirisDTO obj);
        ResponseDTO Edit(ChepStokGirisDTO obj);
        ResponseDTO Get(int id);
        ResponseDTO Delete(int id);
        ResponseDTO ListDetails();
        ResponseDTO Import(IFormFile files, int userId);
    }
}