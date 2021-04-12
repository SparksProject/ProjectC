using System;
using System.Collections.Generic;

using Chep.Data.Repository;
using Chep.DTO;
using Chep.Service.Interface;
using System.Linq;

namespace Chep.Service
{
    public class DefinitionService : BaseService, IDefinitionService
    {
        private readonly IUnitOfWork _uow;

        public DefinitionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResponseDTO GetCustomers()
        {
            var entities = _uow.Customers.GetAll();

            var list = new List<CustomerDTO>();

            foreach (var item in entities)
            {
                list.Add(new CustomerDTO
                {
                    CustomerId = item.CustomerId,
                    Name = item.Name
                });
            }

            return Success(list);
        }

        public ResponseDTO GetPeriodTypes()
        {
            var entities = _uow.PeriodTypes.GetAll();

            var list = new List<PeriodTypeDTO>();

            foreach (var item in entities)
            {
                list.Add(new PeriodTypeDTO
                {
                    PeriodTypeId = item.PeriodTypeId,
                    PeriodTypeName = item.PeriodTypeName
                });
            }

            return Success(list);
        }

        public ResponseDTO GetRecordStatuses()
        {
            var entities = _uow.RecordStatuses.GetAll();

            var list = new List<RecordStatusDTO>();

            foreach (var item in entities)
            {
                list.Add(new RecordStatusDTO
                {
                    RecordStatusId = item.RecordStatusId,
                    RecordStatusName = item.RecordStatusName
                });
            }

            return Success(list);
        }

        public ResponseDTO GetUsers()
        {
            var entities = _uow.Users.GetAll();

            var list = new List<UserDTO>();

            foreach (var item in entities)
            {
                list.Add(new UserDTO
                {
                    UserId = item.UserId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                });
            }

            return Success(list);
        }

        public ResponseDTO GetMailDefinitions()
        {
            var entities = _uow.MailDefinitions.GetAll();

            var list = new List<MailDefinitionDto>();

            foreach (var item in entities)
            {
                list.Add(new MailDefinitionDto
                {
                    MailDefinitionId = item.MailDefinitionId,
                    DisplayName = item.RecipientName + " | " + item.EmailAddress
                });
            }

            return Success(list);
        }

        public ResponseDTO GetUserTypes()
        {
            var entities = _uow.UserTypes.GetAll();

            var list = new List<UserTypeDTO>();

            foreach (var item in entities)
            {
                list.Add(new UserTypeDTO
                {
                    UserTypeId = item.UserTypeId,
                    UserTypeName = item.UserTypeName
                });
            }

            return Success(list);
        }

        public ResponseDTO GetParameterTypes()
        {
            var target = new List<ParameterTypeDTO>();

            foreach (Enums.ParameterType item in Enum.GetValues(typeof(Enums.ParameterType)))
            {
                target.Add(new ParameterTypeDTO
                {
                    ParameterTypeName = item.ToString(),
                    ParameterTypeId = (short)item
                });
            }

            return Success(target);
        }

        public ResponseDTO GetCustoms()
        {
            var entities = _uow.Customs.GetAll();

            var list = entities.Select(item => new CustomDTO
            {
                CustomsId = item.CustomsId,
                CustomsName = item.CustomsName,
                EdiCode = item.EdiCode,
                Status = item.Status
            });

            return Success(list);
        }

        public ResponseDTO GetNextReferenceNumber(string stockType)
        {
            try
            {
                var target = string.Empty;

                switch (stockType)
                {
                    case "Giris":
                        if (_uow.ChepStokGiris.Any(x => true))
                        {
                            var max = _uow.ChepStokGiris.Set().Max(x => x.ReferansNo);
                            ++max;

                            target = max.ToString();
                        }
                        break;
                    case "Cikis":
                        if (_uow.ChepStokCikis.Any(x => true))
                        {
                            var max = _uow.ChepStokCikis.Set().Max(x => x.ReferansNo);
                            ++max;

                            target = max.ToString();
                        }
                        break;
                }

                if (string.IsNullOrEmpty(target))
                {
                    target = $"{DateTime.Now.Year}{"1".PadLeft(5, '0')}"; // 202100001
                }

                return Success(target);
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

    }
}