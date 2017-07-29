namespace QnaLuisBot.Handlers
{
    public interface IHandlerFactory
    {
        IIntentHandler CreateIntentHandler(string key);
    }
}
