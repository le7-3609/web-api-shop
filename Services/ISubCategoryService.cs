using DTO;

namespace Services
{
    public interface ISubCategoryService
    {
        Task<SubCategoryDTO> AddSubCategoryAsync(AddSubCategoryDTO dto);
        Task<bool> DeleteSubCategoryAsync(int id);
        Task<IEnumerable<SubCategoryDTO>> GetSubCategoryAsync(int paging, int limit, string? search, int? minPrice, int? MaxPrice, int? mainCategoryID);
        Task<SubCategoryDTO> GetSubCategoryByIdAsync(int id);
        Task UpdateSubCategoryAsync(int id, SubCategoryDTO dto);
    }
}