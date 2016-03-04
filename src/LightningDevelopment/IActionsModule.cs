namespace LightningDevelopment
{
    public interface IActionsModule
    {
        bool ContainsAction(string txt);
        void RunAction(string action, string[] arguments);
    }
}