using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;
using QnaLuisBot.Dialogs;
using QnaLuisBot.Handlers;
using QnaLuisBot.Services;
using System.Configuration;

namespace QnaLuisBot.Modules
{
    internal sealed class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            string luisSubscriptionKey = ConfigurationManager.AppSettings["LuisSubscriptionKey"];
            string luisModelId= ConfigurationManager.AppSettings["LuisModelId"];

            builder.Register(c => new LuisModelAttribute(
                    luisModelId,             // modelID
                    luisSubscriptionKey,     // subscriptionKey
                    LuisApiVersion.V2        
                   )).AsSelf().AsImplementedInterfaces().SingleInstance();

            // Top Level Dialog
            builder.RegisterType<MainDialog>().As<IDialog<object>>().InstancePerDependency();

            // Singlton services
            builder.RegisterType<LuisService>().Keyed<ILuisService>(FiberModule.Key_DoNotSerialize).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<QnAMakerService>().Keyed<ISearchService>(FiberModule.Key_DoNotSerialize).AsImplementedInterfaces().SingleInstance();
            
            // Objects depending on incoming messages
            builder.RegisterType<HandlerFactory>().Keyed<IHandlerFactory>(FiberModule.Key_DoNotSerialize).AsImplementedInterfaces().InstancePerMatchingLifetimeScope(DialogModule.LifetimeScopeTag);
            builder.RegisterType<SearchIntentHandler>().Keyed<IIntentHandler>(FiberModule.Key_DoNotSerialize).Named<IIntentHandler>(QnaLuisBotStrings.SearchIntentName).AsImplementedInterfaces().InstancePerMatchingLifetimeScope(DialogModule.LifetimeScopeTag);
            builder.RegisterType<GreetingIntentHandler>().Keyed<IIntentHandler>(FiberModule.Key_DoNotSerialize).Named<IIntentHandler>(QnaLuisBotStrings.GreetingIntentName).AsImplementedInterfaces().InstancePerMatchingLifetimeScope(DialogModule.LifetimeScopeTag);
        }
    }
}