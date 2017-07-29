using System.Threading.Tasks;
using QnaLuisBot.Models.Search;

namespace QnaLuisBot.Services
{
    public interface ISearchService
    {
        Task<QnaMakerResultsRoot> FindAnswers(string question);
    }
}
