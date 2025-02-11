namespace Chat_Server;

public class NullCommand : ICommand
{
    private readonly User _user;

    public NullCommand(User user)
    {
        _user = user;
    }

    public void Execute()
    {
        Message message = new Message(Guid.NewGuid(), String.Format($"Invalid command."), "Server");
        _user.Socket.Send(MessageService.EncodeMessage(message));
    }
}