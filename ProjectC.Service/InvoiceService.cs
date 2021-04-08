using AutoMapper;
using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;
using System;
using System.Collections.Generic;

namespace Chep.Service
{
    public class InvoiceService : BaseService, IInvoiceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public InvoiceService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new invoice.
        /// </summary>
        /// <param name="obj">Invoice to be created</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Add(InvoiceDTO obj)
        {
            try
            {
                var entity = _mapper.Map<Invoice>(obj);

                var result = _uow.Invoices.Add(entity);

                return Success(result.InvoiceId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Updates the given invoice.
        /// </summary>
        /// <param name="obj">.Invoice to be updated</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Edit(InvoiceDTO obj)
        {
            try
            {
                var entity = _mapper.Map<Invoice>(obj);

                var result = _uow.Invoices.Update(entity);

                return Success(result.InvoiceId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Retrives the invoice by id
        /// </summary>
        /// <param name="id">InvoiceId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(Guid id)
        {
            try
            {
                var entity = _uow.Invoices.Single(x => x.InvoiceId == id);

                var result = _mapper.Map<InvoiceDTO>(entity);

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Lists all invoices.
        /// </summary>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO List(Guid id)
        {
            try
            {
                var entities = _uow.Invoices.Search(x => x.WorkOrderMasterId == id);

                List<InvoiceDTO> list = new List<InvoiceDTO>();

                foreach (var item in entities)
                {
                    InvoiceDTO obj = new InvoiceDTO
                    {
                        InvoiceId = item.InvoiceId,
                        WorkOrderMasterId = item.WorkOrderMasterId,
                        SenderNo = item.SenderNo,
                        SenderName = item.SenderName,
                        SenderCity = item.SenderCity,
                        SenderCountry = item.SenderCountry,
                        SenderAddress = item.SenderAddress,
                        ConsgnName = item.ConsgnName
                        
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
