using System;
using Chep.Data.Repository;
using Chep.Service.Interface;

namespace Chep.Service
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IUnitOfWork _uow;

        public WorkOrderService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public int SetWorkOrderMastersModel(int id)
        {
            try
            {
                _uow.UserTypes.GetAll();

                return 0;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        //public WorkOrderMasterModel SetWorkOrderMastersModel(int id)
        //{
        //    var master = _uow.VwWorkOrderMaster.Search(x => x.StokCikisId == id);
        //    var invoice = _uow.VwWorkOrderInvoice.Search(x => x.StokCikisId == id);
        //    var invoiceDetails = _uow.VwWorkOrderInvoiceDetails.Search(x => x.StokCikisId == id);
        //    WorkOrderMasterModel model = new WorkOrderMasterModel
        //    {
        //        WorkOrderNo = "1",
        //        DeclarationType = "EX",
        //        InvoiceList = new InvoiceModel[1],
        //    };
        //    model.InvoiceList[0] = new InvoiceModel
        //    {
        //        InvoiceAmount = 5M,
        //        InvoiceCurrency = "TRL",
        //        InvoiceDetailList = new InvoiceDetailModel[1]
        //    };
        //    model.InvoiceList[0].InvoiceDetailList[0] = new InvoiceDetailModel
        //    {
        //        DescGoods = "DescGoods",
        //        ProductNo = "ProductNo",
        //        Uom = "Uom",
        //        InvoiceNo = "ABC20180000000001",
        //        InvoiceDate = DateTime.Now.Date,
        //        PkgType = "ABC"
        //    };
        //    return model;
        //}
    }
}