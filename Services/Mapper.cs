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
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.Product.SubCategory.SubCategoryName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.SubCategory.ImageUrl))
                .ForMember(dest => dest.SubCategoryDescription, opt => opt.MapFrom(src => src.Product.SubCategory.CategoryDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.UserDescription, opt => opt.MapFrom(src => src.UserDescription))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
            CreateMap<OrderSummaryDTO, Order>();
            CreateMap<Order, OrderDetailsDTO>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.OrderSum, opt => opt.MapFrom(src => src.OrderSum))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate));
            CreateMap<AddReviewDTO, Review>();
            CreateMap<Review, ReviewDTO>();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<Order, OrderItemDTO>();
            CreateMap<SiteTypeDTO, SiteType>();
            CreateMap<SiteType, SiteTypeDTO>();
            CreateMap<MainCategory, MainCategoryDTO>().ReverseMap();
            CreateMap<ManegerMainCategoryDTO, MainCategory>();
            CreateMap<SubCategory, SubCategoryDTO>().ReverseMap();
            CreateMap<AddSubCategoryDTO, SubCategory>();
            CreateMap<Platform, PlatformsDTO>().ReverseMap();
            CreateMap<AddPlatformDTO, Platform>();
            CreateMap<Product, ProductDTO>()
            .ForMember(
            dest => dest.CategoryName,
            opts => opts.MapFrom(src => src.SubCategory.SubCategoryName));
            CreateMap<ProductDTO, Product>();
            CreateMap<AddProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>()
           .ForMember(
            dest => dest.ProductId,
            opts => opts.MapFrom(src => src.ProductId));
            CreateMap<BasicSite, BasicSiteDTO>();
            CreateMap<AddBasicSiteDTO, BasicSite>();
            CreateMap<UpdateBasicSiteDTO, BasicSite>();

        }
    }
}
