using DTO;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using AutoMapper;
namespace Services
{
    public class MainCategoryService : IMainCategoryService
    {

        private readonly IMainCategoryRepository _mainCategoryRepository;
        private readonly IMapper _mapper;
        public MainCategoryService(IMainCategoryRepository mainCategoryReposetory, IMapper mapper)
        {
            _mainCategoryRepository = mainCategoryReposetory;
            _mapper = mapper;
        }

        async public Task<IEnumerable<MainCategoryDTO>> GetMainCategoryAsync()
        {
            var MainCategories = await _mainCategoryRepository.GetMainCategoriesAsync();
            var dto = _mapper.Map<IEnumerable<MainCategoryDTO>>(MainCategories);
            return dto;

        }

        async public Task<MainCategoryDTO> AddMainCategoryAsync(ManegerMainCategoryDTO dto)
        {
            MainCategory mainCategory = _mapper.Map<MainCategory>(dto);
            //הכנסה של פרומפט ע"י gemini

            mainCategory = await _mainCategoryRepository.AddMainCategoryAsync(mainCategory);
            return _mapper.Map<MainCategoryDTO>(mainCategory);
        }

        async public Task UpdateMainCategoryAsync(int id, MainCategoryDTO dto)
        {

            MainCategory mainCategory = _mapper.Map<MainCategory>(dto);
            //הכנסה של פרומפט ע"י gemini

            await _mainCategoryRepository.UpdateMainCategoryAsync(id, mainCategory);
        }

        async public Task<bool> DeleteMainCategoryAsync(int id)
        {
            return await _mainCategoryRepository.DeleteMainCategoryAsync(id);
        }
    }
}
