namespace Chat_Server;


public class CommandFactory
{
    public ICommand CreateCommand(Message command, User user)
    {
        IServiceFacade serviceFacade = ServiceFacade.Instance;
        
        string[] messageBody = command.Body.Split(' ');
        switch (messageBody[0])
        {
            case ICommand.WhoCommand:
                return new WhoCommand(user, serviceFacade);
            case ICommand.WhisperCommand:
                return new WhisperCommand(user, serviceFacade, messageBody);
            default:
                return new NullCommand(user, serviceFacade);
        }
    }
}