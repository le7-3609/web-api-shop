using DTO;
using Entities;
using Microsoft.Identity.Client;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GeminiService : IGeminiService
    {
        private readonly IGemini _gemini;
        private readonly IGeminiPromptsRepository _geminiPromptsRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;

        public GeminiService(IGemini gemini, ISubCategoryRepository subCategoryRepository,
            IGeminiPromptsRepository geminiPromptsRepository)
        {
            _gemini = gemini;
            _subCategoryRepository = subCategoryRepository;
            _geminiPromptsRepository = geminiPromptsRepository;
        }

        public async Task<Resulte<GeminiPrompt>> AddGeminiForUserProductAsync(int categoryId, string userRequest)
        {
            SubCategory? subCategory = await _subCategoryRepository.GetSubCategoryByIdAsync(categoryId);
            if (subCategory == null)
            {
                Resulte<GeminiPrompt>.Failure("The product id is incorrect");
            }
            string result = await _gemini.RunGeminiForUserProduct(userRequest, subCategory.SubCategoryName);
            GeminiPrompt prompt = new GeminiPrompt();
            prompt.Prompt = result;
            prompt.SubCategoryId = categoryId;
            if (result != null)
            {
                GeminiPrompt resultFromRepository = await _geminiPromptsRepository.AddPromptAsync(prompt);
                return Resulte<GeminiPrompt>.Success(resultFromRepository);
            }
            return Resulte<GeminiPrompt>.Success(prompt);

        }

        public async Task<Resulte<GeminiPrompt>> UpdateGeminiForUserProductAsync(long promptId, string userRequest)
        {
            GeminiPrompt? checkIfThePromptExist = await _geminiPromptsRepository.GetByIDPromptAsync(promptId);

            if (checkIfThePromptExist == null)
            {
                Resulte<GeminiPrompt>.Failure("The prompt id is incorrect");
            }
            SubCategory? subCategory = await _subCategoryRepository.GetSubCategoryByIdAsync(checkIfThePromptExist.SubCategoryId);
            string result = await _gemini.RunGeminiForUserProduct(userRequest, subCategory.SubCategoryName);
            checkIfThePromptExist.Prompt = result;
            if (result != null)
            {
                await _geminiPromptsRepository.UpdatePromptAsync(promptId, checkIfThePromptExist);

            }
            return Resulte<GeminiPrompt>.Success(null);
        }

        public async Task<GeminiPrompt?> GetByIdPromptAsync(long promptId)
        {
            return await _geminiPromptsRepository.GetByIDPromptAsync(promptId);
        }
    }
}