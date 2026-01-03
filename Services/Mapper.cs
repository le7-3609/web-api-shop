using AutoMapper;
using DTO;
using Entities;

namespace Services
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<RegisterAndUpdateDTO, User>();
            CreateMap<User, UserProfileDTO>();

            CreateMap<AddCartItemDTO, CartItem>();
            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.BasicSiteName, opt => opt.MapFrom(src => src.BasicSite.SiteName))
                .ForMember(dest => dest.BasicSiteUserDescription, opt => opt.MapFrom(src => src.BasicSite.UserDescreption));
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.Product.SubCategory.SubCategoryName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.SubCategory.ImageUrl))
                .ForMember(dest => dest.SubCategoryDescription, opt => opt.MapFrom(src => src.Product.SubCategory.CategoryDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (float)src.Product.Price))
                .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.Platform.PlatformName));

            CreateMap<Order, OrderSummaryDTO>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.StatusNavigation.StatusName));
            CreateMap<Order, OrderDetailsDTO>()
                .IncludeBase<Order, OrderSummaryDTO>() 
                .ForMember(dest => dest.SiteName, opt => opt.MapFrom(src => src.BasicSite.SiteName))
                .ForMember(dest => dest.SiteTypeName, opt => opt.MapFrom(src => src.BasicSite.SiteType.SiteTypeName))
                .ForMember(dest => dest.SiteDescription, opt => opt.MapFrom(src => src.BasicSite.UserDescreption))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (float)src.Product.Price))
                .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.Platform.PlatformName));

            CreateMap<MainCategory, MainCategoryDTO>().ReverseMap();
            CreateMap<ManegerMainCategoryDTO, MainCategory>();

            CreateMap<SubCategory, SubCategoryDTO>().ReverseMap();
            CreateMap<AddSubCategoryDTO, SubCategory>();

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.SubCategoryName));
            CreateMap<AddProductDTO, Product>();

            CreateMap<Platform, PlatformsDTO>().ReverseMap();
            CreateMap<AddPlatformDTO, Platform>();

            CreateMap<BasicSite, BasicSiteDTO>()
                .ForMember(dest => dest.SiteTypeName, opt => opt.MapFrom(src => src.SiteType.SiteTypeName))
                .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.Platform.PlatformName))
                .ForMember(dest => dest.SiteTypeDescreption, opt => opt.MapFrom(src => src.SiteType.SiteTypeDescription));
            CreateMap<AddBasicSiteDTO, BasicSite>();
            CreateMap<UpdateBasicSiteDTO, BasicSite>();

            CreateMap<SiteType, SiteTypeDTO>().ReverseMap();

            CreateMap<AddReviewDTO, Review>();
            CreateMap<Review, ReviewDTO>();
        }
    }
}