using AutoMapper;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace  Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        public PlatformService(IPlatformRepository platformsReposetory, IMapper mapper)
        {

            _platformRepository = platformsReposetory;
            _mapper = mapper;
        }

        async public Task<IEnumerable<PlatformsDTO>> GetPlatformsAsync()
        {
            IEnumerable<Platform> platformList = await _platformRepository.GetPlatformsAsync();
            return _mapper.Map<IEnumerable<PlatformsDTO>>(platformList);
        }

        async public Task<PlatformsDTO> AddPlatformAsync(string platformName)
        {
            var existing = await _platformRepository.GetPlatformByNameAsync(platformName);

            if (existing != null)
            {
                return null;
            }
            Platform platform = new Platform{PlatformName = platformName};
            platform = await _platformRepository.AddPlatformAsync(platform);
            //add prompt with gemini
            // dto.PlatformsPrompt = "fdfbnfgn";
            return _mapper.Map<PlatformsDTO>(platform);
        }

        async public Task UpdatePlatformAsync(int id, PlatformsDTO dto)
        {
            var existing = await _platformRepository.GetPlatformByNameAsync(dto.PlatformName);

            if (existing != null)
            {
                return;
            }
            Platform platform = _mapper.Map<Platform>(dto);
            //add prompt with gemini
            // platform.PlatformsPrompt = "fdfbnfgn";
            await _platformRepository.UpdatePlatformAsync(id, platform);
        }

        async public Task<bool> DeletePlatformAsync(int id)
        {
            return await _platformRepository.DeletePlatformAsync(id);
        }
    }
}
