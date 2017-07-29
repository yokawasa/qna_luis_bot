using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using QnaLuisBot.Services;

namespace QnaLuisBot.Handlers
{
    internal sealed class SearchIntentHandler : IIntentHandler
    {
        private readonly ISearchService qnaMakerService;
        private readonly IBotToUser botToUser;

        public SearchIntentHandler(IBotToUser botToUser, ISearchService qnaMakerService)
        {
            SetField.NotNull(out this.qnaMakerService, nameof(qnaMakerService), qnaMakerService);
            SetField.NotNull(out this.botToUser, nameof(botToUser), botToUser);
        }

        public async Task Respond(IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            EntityRecommendation entityRecommendation;

            var query = result.TryFindEntity(QnaLuisBotStrings.QuestionsEntityTopic, out entityRecommendation)
                ? entityRecommendation.Entity
                : result.Query;
            Console.WriteLine(string.Format("query={0}", query));

            var qnaMakerResultsRoot = await this.qnaMakerService.FindAnswers(query);

            var summaryText =  String.Empty;
            if (qnaMakerResultsRoot.answers.Count < 1)
            {
                summaryText = "Answers NOT FOUND!!";
            }
            else
            {
                foreach (var answer in qnaMakerResultsRoot.answers)
                {
                    Console.WriteLine(string.Format("answer={0} score={1}", answer.Answer, answer.Score));
                }
                var bestAnswer = qnaMakerResultsRoot.answers[0];
                summaryText = bestAnswer.Answer;
                //summaryText = summaryText + " ([debug]query=" + query + ")"; // debug
                await this.botToUser.PostAsync(string.Format(Strings.SearchTopicTypeMessage));
            }
            await this.botToUser.PostAsync(summaryText);
        }

    }
}