using System.Linq;

namespace Chat_Server;

public class WhoCommand : ICommand
{
    private readonly User _user;

    public WhoCommand(User user)
    {
        _user = user;
    }

    public void Execute()
    {
        ChatService _chatservice = ChatService.Instance;
        IEnumerable<User> user_list = _chatservice.GetConnectedUsers();
        string users = String.Join(", ", user_list.Select(user => user.Username));
        _chatservice.Speak(String.Format($"Connected users: {users}"), _user);
    }
}