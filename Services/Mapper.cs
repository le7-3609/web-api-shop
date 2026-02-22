using AutoMapper;
using DTO;
using Entities;

namespace Services
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<RegisterDTO, User>();
            CreateMap<User, UserProfileDTO>();
            CreateMap<User, UserDTO>();


            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.BasicSiteName, opt => opt.MapFrom(src => src.BasicSite != null ? src.BasicSite.SiteName : null))
                .ForMember(dest => dest.BasicSiteUserDescription, opt => opt.MapFrom(src => src.BasicSite != null ? src.BasicSite.UserDescreption : null));
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.Product.SubCategory.SubCategoryName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.SubCategory.ImageUrl))
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Product.SubCategory.CategoryDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.Platform.PlatformName))
                .ForMember(dest => dest.PromptId, opt => opt.MapFrom(src => src.PromptId))
                .ForMember(dest => dest.Prompt, opt => opt.MapFrom(src => src.Prompt != null ? src.Prompt.Prompt : null))
                .ForMember(dest => dest.UserDescription, opt => opt.MapFrom(src => src.Prompt != null ? src.Prompt.Prompt : string.Empty));
            CreateMap<AddCartItemForEmptyProductDTO, CartItem>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(dest => dest.PromptId, opt => opt.MapFrom(src => src.PromptId));

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
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.Platform.PlatformName))
                .ForMember(dest => dest.PromptId, opt => opt.MapFrom(src => src.PromptId))
                .ForMember(dest => dest.Prompt, opt => opt.MapFrom(src => src.Prompt != null ? src.Prompt.Prompt : null))
                .ForMember(dest => dest.UserDescription, opt => opt.MapFrom(src => src.Prompt != null ? src.Prompt.Prompt : string.Empty));

            CreateMap<MainCategory, MainCategoryDTO>().ReverseMap();
            CreateMap<AdminMainCategoryDTO, MainCategory>();
            CreateMap<MainCategory, AdminMainCategoryDTO>();

            CreateMap<SubCategory, SubCategoryDTO>().ReverseMap();
            CreateMap<AddSubCategoryDTO, SubCategory>();

            CreateMap<Product, ProductDTO>()
                .ForCtorParam("ProductId", opt => opt.MapFrom(src => src.ProductId))
                .ForCtorParam("SubCategoryId", opt => opt.MapFrom(src => src.SubCategoryId))
                .ForCtorParam("ProductName", opt => opt.MapFrom(src => src.ProductName))
                .ForCtorParam("SubCategoryName", opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.SubCategoryName : string.Empty))
                .ForCtorParam("Price", opt => opt.MapFrom(src => src.Price))
                .ForCtorParam("ProductPrompt", opt => opt.MapFrom(src => src.ProductPrompt))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory != null ? src.SubCategory.SubCategoryName : string.Empty))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
            CreateMap<AddProductDTO, Product>()
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.SubCategoryId));
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());  // Ignore ProductId during update
            CreateMap<AdminProductDTO, Product>()
                .ForMember(dest => dest.ProductId, opt => opt.Condition(src => src.ProductId.HasValue))
                .ForMember(dest => dest.ProductPrompt, opt => opt.MapFrom(src => src.ProductPrompt));

            CreateMap<Platform, PlatformsDTO>().ReverseMap();
            CreateMap<AddPlatformDTO, Platform>();

            CreateMap<BasicSite, BasicSiteDTO>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.SiteType != null ? src.SiteType.Price : 0))
                .ForMember(dest => dest.SiteTypeName, opt => opt.MapFrom(src => src.SiteType != null ? src.SiteType.SiteTypeName : null))
                .ForMember(dest => dest.PlatformName, opt => opt.MapFrom(src => src.Platform != null ? src.Platform.PlatformName : null))
                .ForMember(dest => dest.SiteTypeDescreption, opt => opt.MapFrom(src => src.SiteType != null ? src.SiteType.SiteTypeDescription : null));
            CreateMap<AddBasicSiteDTO, BasicSite>();
            CreateMap<UpdateBasicSiteDTO, BasicSite>();

            CreateMap<SiteType, SiteTypeDTO>().ReverseMap();

            CreateMap<AddReviewDTO, Review>();
            CreateMap<Review, ReviewDTO>();

            CreateMap<GeminiPrompt, GeminiPromptDTO>().ReverseMap();
            CreateMap<AddGeminiPromptDTO, GeminiPrompt>();
        }
    }
}