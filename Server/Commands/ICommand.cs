namespace Chat_Server;

public interface ICommand
{
    public const string WhoCommand = "/who";

    public abstract void Execute();
}