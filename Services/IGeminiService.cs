using DTO;
using Entities;

namespace Services
{
    public interface IGeminiService
    {
        Task<Resulte<GeminiPrompt>> AddGeminiForUserProductAsync(int categoryId, string userRequest);
        Task<GeminiPrompt?> GetByIdPromptAsync(long promptId);
        Task<Resulte<GeminiPrompt>> UpdateGeminiForUserProductAsync(long promptId, string userRequest);
    }
}