using AutoMapper;
using ProjectC.Data.Repository;
using ProjectC.DTO;
using ProjectC.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectC.Service
{
    public class InvoiceDetailService : BaseService, IInvoiceDetailService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public InvoiceDetailService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }


        public ResponseDTO Add(InvoiceDTO obj)
        {
            throw new NotImplementedException();
        }

        public ResponseDTO Edit(InvoiceDTO obj)
        {
            throw new NotImplementedException();
        }

        public ResponseDTO Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public ResponseDTO List(Guid id)
        {
            try
            {
                var entities = _uow.InvoiceDetails.Search(x => x.InvoiceId == id);

                List<InvoiceDetailDTO> list = new List<InvoiceDetailDTO>();

                foreach (var item in entities)
                {
                    InvoiceDetailDTO obj = new InvoiceDetailDTO
                    {
                        InvoiceDetailId = item.InvoiceDetailId,
                        InvoiceId = item.InvoiceId,
                        HsCode = item.HsCode,
                        DescGoods = item.DescGoods,
                        ProductNo = item.ProductNo,
                        CountryOfOrigin = item.CountryOfOrigin
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
    }
}
