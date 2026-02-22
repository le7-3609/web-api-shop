using DTO;

namespace Services
{
    public interface IMainCategoryService
    {
        Task<MainCategoryDTO> AddMainCategoryAsync(AdminMainCategoryDTO dto);
        Task<bool> DeleteMainCategoryAsync(int id);
        Task<IEnumerable<MainCategoryDTO>> GetMainCategoryAsync();
        Task<bool> UpdateMainCategoryAsync(int id, AdminMainCategoryDTO dto);
    }
}