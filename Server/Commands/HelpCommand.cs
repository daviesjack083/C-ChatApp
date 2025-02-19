namespace Chat_Server;

public class HelpCommand : ICommand
{
    private readonly User _user;
    private readonly IServiceFacade _serviceFacade;


    public HelpCommand(User user, IServiceFacade serviceFacade)
    {
        _user = user;
        _serviceFacade = serviceFacade;
    }


    public void Execute()
    {
        string helpString = $@"Available Commands:
                    {ICommand.WhoCommand} - List users in the room
                    {ICommand.WhisperCommand} username - Send a private message";

        string helpString2 = String.Format($"");

        _serviceFacade.Speak(helpString, _user);
    }
}