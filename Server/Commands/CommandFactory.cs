namespace Chat_Server;


public class CommandFactory
{
    public ICommand CreateCommand(Message command, User user = null)
    {
        string[] messageBody = command.Body.Split(' ');
        switch (messageBody[0])
        {
            default:
                return new NullCommand(user);
        }
    }
}