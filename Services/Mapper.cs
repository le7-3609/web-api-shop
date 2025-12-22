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
                .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.BasicSitesPlatform.Platform.PlatformName))
                .ForMember(dest => dest.UserDescription, opt => opt.MapFrom(src => src.UserDescription))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
            CreateMap<OrderSummaryDTO, Order>();
            CreateMap<Order, OrderDetailsDTO>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.OrderSum))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate));
            CreateMap<AddReviewDTO, Review>();
            CreateMap<Review, ReviewDTO>();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<Order, OrderItemDTO>();
            CreateMap<SiteTypeDTO, SiteType>();
            CreateMap<SiteType, SiteTypeDTO>();
        }
    }
}
