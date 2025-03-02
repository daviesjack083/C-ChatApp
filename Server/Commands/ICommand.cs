namespace Chat_Server;

public interface ICommand
{
    public const string HelpCommand = "/help";
    public const string WhoCommand = "/who";
    public const string WhisperCommand = "/whisper";

    public abstract void Execute();
}