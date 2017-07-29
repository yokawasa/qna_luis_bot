using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using QnaLuisBot.Handlers;

namespace QnaLuisBot.Dialogs
{
    /// <summary>
    /// The top-level natural language dialog for sample.
    /// </summary>
    [Serializable]
    internal sealed class MainDialog : LuisDialog<object>
    {
        private readonly IHandlerFactory handlerFactory;

        public MainDialog(ILuisService luis, IHandlerFactory handlerFactory)
            : base(luis)
        {
            SetField.NotNull(out this.handlerFactory, nameof(handlerFactory), handlerFactory);
        }

        [LuisIntent(QnaLuisBotStrings.GreetingIntentName)]
        public async Task GreetingIntentHandlerAsync(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            await this.handlerFactory.CreateIntentHandler(QnaLuisBotStrings.GreetingIntentName).Respond(activity, result);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent(QnaLuisBotStrings.SearchIntentName)]
        public async Task FindArticlesIntentHandlerAsync(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            await this.handlerFactory.CreateIntentHandler(QnaLuisBotStrings.SearchIntentName).Respond(activity, result);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task FallbackIntentHandlerAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(string.Format(Strings.FallbackIntentMessage));
            context.Wait(this.MessageReceived);
        }
    }
}