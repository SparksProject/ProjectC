using AutoMapper;
using Chep.Core;
using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Chep.Service
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="obj">Product to be created</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Add(ProductDTO obj)
        {
            try
            {
                obj.RecordStatusId = 1;
                obj.CreatedDate = DateTime.Now;
                obj.ProductId = Guid.NewGuid();
                if (obj.HsCode == null)
                {
                obj.HsCode = string.Empty;
                }
                var entity = _mapper.Map<Product>(obj);

                var result = _uow.Products.Add(entity);

                _uow.Commit();

                return Success(result.ProductId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Adds a range of products.
        /// </summary>
        /// <param name="obj">Product list</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO AddRange(List<Product> obj)
        {
            try
            {
                _uow.Products.AddRange(obj);

                _uow.Commit();

                return Success(true);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Updates the given product.
        /// </summary>
        /// <param name="obj">Product to be updated</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Edit(ProductDTO obj)
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

                var entity = _mapper.Map<Product>(obj);

                var result = _uow.Products.Update(entity);

                _uow.Commit();

                return Success(result.ProductId);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Retrives the product by id
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO Get(Guid id)
        {
            try
            {
                var entity = _uow.Products.Set()
                                          .Include(x => x.RecordStatus)
                                          .Include(x => x.Customer)
                                          .Include(x => x.CreatedByNavigation)
                                          .Include(x => x.ModifiedByNavigation)
                                          .Include(x => x.DeletedByNavigation)
                                          .FirstOrDefault(x => x.ProductId == id);

                var result = _mapper.Map<ProductDTO>(entity);

                result.CustomerName = entity.Customer.Name;
                result.RecordStatusName = entity.RecordStatus.RecordStatusName;
                result.CreatedByName = entity.CreatedByNavigation.FirstName + " " + entity.CreatedByNavigation.LastName;
                result.ModifiedByName = entity.ModifiedBy != null ? entity.ModifiedByNavigation.FirstName + " " + entity.ModifiedByNavigation.LastName : null;
                result.DeletedByName = entity.DeletedBy != null ? entity.DeletedByNavigation.FirstName + " " + entity.DeletedByNavigation.LastName : null;

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        /// <summary>
        /// Lists all products.
        /// </summary>
        /// <returns>ResponseDTO</returns>
        public ResponseDTO List(Guid customerId)
        {
            try
            {
                var entities = _uow.Products.Set()
                                            .Include(x => x.RecordStatus)
                                            .Include(x => x.Customer)
                                            .Where(x => x.CustomerId == customerId)
                                            .ToList();

                if (entities.Count == 0)
                {
                    return NotFound();
                }

                var list = new List<ProductDTO>();

                foreach (var item in entities)
                {
                    var obj = new ProductDTO
                    {
                        ProductId = item.ProductId,
                        CustomerName = item.Customer.Name,
                        ProductNo = item.ProductNo,
                        ProductNameTr = item.ProductNameTr,
                        ProductNameEng = item.ProductNameEng,
                        ProductNameOrg = item.ProductNameOrg,
                        HsCode = item.HsCode,
                        Uom = item.Uom,
                        GrossWeight = item.GrossWeight,
                        NetWeight = item.NetWeight,
                        SapCode = item.SapCode,
                        CountryOfOrigin = item.CountryOfOrigin,
                        RecordStatusName = item.RecordStatus.RecordStatusName
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

        public ResponseDTO UploadFile(Guid customerId, int createdBy, string file)
        {
            file = file.Replace("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,", "");
            byte[] decodedByteArray = Convert.FromBase64String(file);

            return Success(decodedByteArray);
        }

        public ResponseDTO AddRange(DataTable dt, Guid customerId, int createdBy)
        {
            try
            {
                var message = string.Empty;
                var unSuccessullRowCount = 0;

                if (dt.Rows.Count == 0)
                {
                    return NotFound();
                }

                var list = new List<Product>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var item = dt.Rows[i];

                    if (_uow.Products.Any(x => x.ProductNo == item[0].ToString() && x.CustomerId == customerId))
                    {
                        continue;
                    }
                    else
                    {
                        var product = new Product
                        {
                            ProductId = Guid.NewGuid(),
                            CustomerId = customerId,
                            ProductNo = item[0].ToString(),
                            ProductNameTr = item[1].ToString(),
                            ProductNameEng = item[2].ToString(),
                            ProductNameOrg = item[3].ToString(),
                            HsCode = item[4].ToString(),
                            Uom = item[5].ToString(),
                            GrossWeight = Convert.ToDouble(item[6]),
                            NetWeight = Convert.ToDouble(item[7]),
                            SapCode = item[8].ToString(),
                            CountryOfOrigin = item[9].ToString(),
                            RecordStatusId = 1,
                            CreatedDate = DateTime.Now,
                            CreatedBy = createdBy
                        };

                        if (product.ProductNo == null)
                        {
                            message += "Ürün No alanı NULL olamaz!";
                        }
                        else if (product.ProductNo.Length > 50)
                        {
                            message += "Ürün No alanı 50 karakterden fazla olamaz!";
                        }
                        else if (product.ProductNameTr == null)
                        {
                            message += $"{product.ProductNo} numaralı ürünün Ürün Adı alanı NULL olamaz!";
                        }
                        else if (product.ProductNameTr.Length > 100)
                        {
                            message += $"{product.ProductNo} numaralı ürünün Ürün Adı alanı 100 karakterden fazla olamaz!";
                        }
                        else if (product.ProductNameEng != null && product.ProductNameEng.Length > 100)
                        {
                            message += $"{product.ProductNo} numaralı ürünün Ürün Adı(Eng) alanı 100 karakterden fazla olamaz!";
                        }
                        else if (product.ProductNameOrg != null && product.ProductNameOrg.Length > 100)
                        {
                            message += $"{product.ProductNo} numaralı ürünün Ürün Adı(Org) alanı 100 karakterden fazla olamaz!";
                        }
                        else if (product.HsCode == null)
                        {
                            message += $"{product.ProductNo} numaralı ürünün HsKod alanı NULL olamaz!";
                        }
                        else if (product.HsCode.Length > 16)
                        {
                            message += $"{product.ProductNo} numaralı ürünün HsKod alanı 16 karakterden fazla olamaz!";
                        }
                        else if (product.Uom != null && product.Uom.Length > 3)
                        {
                            message += $"{product.ProductNo} numaralı ürünün Uom alanı 3 karakterden fazla olamaz!";
                        }
                        else if (product.CountryOfOrigin != null && product.CountryOfOrigin.Length > 3)
                        {
                            message += $"{product.ProductNo} numaralı ürünün Menşei Alanı alanı 3 karakterden fazla olamaz!";
                        }

                        if (!string.IsNullOrEmpty(message))
                        {
                            message += "<br />";
                            unSuccessullRowCount++;
                        }
                        else
                        {
                            list.Add(product);
                        }
                    }
                }

                if (list.Count > 0)
                {
                    var result = Convert.ToBoolean(AddRange(list).Result);

                    if (result && string.IsNullOrEmpty(message))
                    {
                        return Success(result);
                    }
                    else if (result && !string.IsNullOrEmpty(message))
                    {
                        if (list.Count - unSuccessullRowCount > 0)
                        {
                            message += "<br /><br /> Geri kalanlar başarıyla kaydedildi.";
                        }

                        return Success(result, message);
                    }
                    else
                    {
                        return Success(result, "Liste kaydedilirken bir hata ile karşılaşıldı!");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
    }
}