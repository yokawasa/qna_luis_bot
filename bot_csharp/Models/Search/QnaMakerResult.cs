using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QnaLuisBot.Models.Search
{
    public class QnaMakerResult
    {
        // The top answer found in the QnA Service.
        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        // The score in range [0, 100] corresponding to the top answer found in the QnA    Service.
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }
    public class QnaMakerResultsRoot
    {
        public List<QnaMakerResult> answers { get; set; }
    }

}
