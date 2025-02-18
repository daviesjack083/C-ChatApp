namespace Chat_Server;


public class CommandFactory
{
    private readonly IServiceFacade _serviceFacade;

    public CommandFactory()
    {
        _serviceFacade = new ServiceFacade();
    }


    public ICommand CreateCommand(Message command, User user)
    {
        string[] messageBody = command.Body.Split(' ');
        switch (messageBody[0])
        {
            case ICommand.WhoCommand:
                return new WhoCommand(user, _serviceFacade);
            default:
                return new NullCommand(user, _serviceFacade);
        }
    }
}