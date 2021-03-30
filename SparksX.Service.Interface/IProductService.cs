using SparksX.DTO;
using System;
using System.Data;

namespace SparksX.Service.Interface
{
    public interface IProductService
    {
        ResponseDTO List(Guid customerId);
        ResponseDTO Add(ProductDTO obj);
        ResponseDTO Edit(ProductDTO obj);
        ResponseDTO Get(Guid id);
        ResponseDTO UploadFile(Guid customerId, int createdBy, string file);
        ResponseDTO AddRange(DataTable dt, Guid customerId, int createdBy);
    }
}
