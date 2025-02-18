namespace Chat_Server;

public class NullCommand : ICommand
{
    private readonly User _user;
    private readonly IServiceFacade _serviceFacade;

    public NullCommand(User user, IServiceFacade serviceFacade)
    {
        _user = user;
        _serviceFacade = serviceFacade;
    }

    public void Execute()
    {
        _serviceFacade.Speak("Invalid Command", _user);
    }
}