
namespace Services
{
    public interface IGemini
    {
        Task<string> RunGeminiForUserProduct(string userRequest, string category);
    }
}