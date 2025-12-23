using DTO;

namespace Services
{
    public interface IMainCategoryService
    {
        Task<MainCategoryDTO> AddMainCategoryAsync(ManegerMainCategoryDTO dto);
        Task<bool> DeleteMainCategoryAsync(int id);
        Task<IEnumerable<MainCategoryDTO>> GetMainCategoryAsync();
        Task UpdateMainCategoryAsync(int id, MainCategoryDTO dto);
    }
}