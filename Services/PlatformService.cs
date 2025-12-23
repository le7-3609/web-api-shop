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

        async public Task<PlatformsDTO> AddPlatformAsync(AddPlatformDTO dto)
        {
            Platform platform = _mapper.Map<Platform>(dto);
            //add prompt with gemini
            // dto.PlatformsPrompt = "fdfbnfgn";
            platform = await _platformRepository.AddPlatformAsync(platform);
            return _mapper.Map<PlatformsDTO>(platform);
        }

        async public Task UpdatePlatformAsync(int id, PlatformsDTO dto)
        {
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
