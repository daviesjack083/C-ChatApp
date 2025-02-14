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
        ChatService _chatservice = ChatService.Instance;
        _chatservice.Speak("Invalid Command", _user);
    }
}