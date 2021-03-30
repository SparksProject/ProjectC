using ProjectC.DTO;

using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectC.Service.Interface
{
    public interface IStokGirisService
    {
        ResponseDTO List();
        ResponseDTO Add(ChepStokGirisDTO obj);
        ResponseDTO Edit(ChepStokGirisDTO obj);
        ResponseDTO Get(int id);
    }
}