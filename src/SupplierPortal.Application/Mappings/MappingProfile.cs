using AutoMapper;
using SupplierPortal.Application.DTOs;
using SupplierPortal.Domain.Entities;

namespace SupplierPortal.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // =====================
        // SupplierProfile <-> SupplierDTO
        // =====================
        CreateMap<SupplierProfile, SupplierDTO>().ReverseMap();

        CreateMap<CreateSupplierDTO, SupplierProfile>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // =====================
        // Account mappings
        // =====================
        CreateMap<Account, AccountDTO>();

        CreateMap<CreateUserDTO, Account>()
            .ForMember(d => d.PasswordHash, opt => opt.Ignore()) // hash in service
            .ForMember(d => d.Role, o => o.MapFrom(s => s.IsAdmin ? Domain.Enums.AccountRole.Admin : Domain.Enums.AccountRole.User));

        CreateMap<UpdateUserDTO, Account>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // =====================
        // Purchase Request mappings
        // =====================
        CreateMap<PurchaseRequest, PurchaseRequestDTO>()
            .ForMember(dest => dest.SupplierTitle,
                opt => opt.MapFrom(src => src.Account != null && src.Account.SupplierProfile != null
                    ? src.Account.SupplierProfile.Title
                    : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.Account, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<CreatePurchaseRequestDTO, PurchaseRequest>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RequestDate, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore()); 

        CreateMap<PurchaseRequest, CompletedPurchaseRequestDTO>()
            .ForMember(dest => dest.SupplierCode,
                opt => opt.MapFrom(src => src.Account != null && src.Account.SupplierProfile != null
                    ? src.Account.SupplierProfile.Code
                    : string.Empty))
            .ForMember(dest => dest.SupplierTitle,
                opt => opt.MapFrom(src => src.Account != null && src.Account.SupplierProfile != null
                    ? src.Account.SupplierProfile.Title
                    : string.Empty))
            .ForMember(dest => dest.Items,
                opt => opt.MapFrom(src => src.Items));

        // =====================
        // Purchase Request Item mappings
        // =====================
        CreateMap<PurchaseRequestItem, PurchaseRequestItemDTO>()
            .ReverseMap()
            .ForMember(dest => dest.PurchaseRequest, opt => opt.Ignore());

        CreateMap<CreatePurchaseRequestItemDTO, PurchaseRequestItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PurchaseRequestId, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.DeliveryDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsPriced, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.PurchaseRequest, opt => opt.Ignore());

        CreateMap<UpdatePurchaseRequestItemDTO, PurchaseRequestItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.PurchaseRequestId, opt => opt.Ignore())
            .ForMember(dest => dest.ProductName, opt => opt.Ignore())
            .ForMember(dest => dest.Quantity, opt => opt.Ignore())
            .ForMember(dest => dest.Unit, opt => opt.Ignore())
            .ForMember(dest => dest.IsPriced, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.PurchaseRequest, opt => opt.Ignore());

        // =====================
        // Account <-> SupplierResult mappings
        // =====================
        CreateMap<Account, SupplierResultDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.SupplierProfileDTO, opt => opt.MapFrom(src => src.SupplierProfile));

        CreateMap<SupplierResultDTO, Account>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.SupplierProfile, opt => opt.MapFrom(src => src.SupplierProfileDTO))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<SupplierProfileDTO, SupplierProfile>().ReverseMap();
    }
}