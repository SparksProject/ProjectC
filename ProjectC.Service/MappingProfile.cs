using AutoMapper;
using ProjectC.Core;
using ProjectC.DTO;

namespace ProjectC.Service
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<MailReport, MailReportDTO>();
            CreateMap<MailReportDTO, MailReport>();
            CreateMap<PeriodType, PeriodTypeDTO>();
            CreateMap<PeriodTypeDTO, PeriodType>();
            CreateMap<GenericReport, GenericReportDTO>();
            CreateMap<GenericReportDTO, GenericReport>();
            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<UserPermission, UserPermissionDTO>();
            CreateMap<UserPermissionDTO, UserPermission>();
            CreateMap<SparksArchiveDTO, SparksArchive>();
            CreateMap<SparksArchive, SparksArchiveDTO>();
        }
    }
}
