using System.Configuration;
using System.Threading.Tasks;
using QnaLuisBot.Models.Search;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace QnaLuisBot.Services
{
    /// <summary>
    /// Responsible for calling QnA Maker API
    /// </summary>
    internal sealed class QnAMakerService : ISearchService
    {

        public async Task<QnaMakerResultsRoot> FindAnswers(string question)
        {

            string qnamakerSubscriptionKey = ConfigurationManager.AppSettings["QnaMakerSubscriptionKey"];
            string knowledgebaseId = ConfigurationManager.AppSettings["QnaMakerKnowledgebaseId"];

            var client = new HttpClient();

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{question}\"}}";
            System.Console.WriteLine(postBody);
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);

            var uri = $"https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/{knowledgebaseId}/generateAnswer";

            System.Console.WriteLine(uri.ToString());
            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(postBody);
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
                System.Console.WriteLine(response.ToString());
                string text = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine(text);

                return JsonConvert.DeserializeObject<QnaMakerResultsRoot>(text);
            }
        }

    }
}