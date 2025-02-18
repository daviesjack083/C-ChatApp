using System.Linq;

namespace Chat_Server;

public class WhoCommand : ICommand
{
    private readonly User _user;
    private readonly IServiceFacade _serviceFacade;

    public WhoCommand(User user, IServiceFacade serviceFacade)
    {
        _user = user;
        _serviceFacade = serviceFacade;
    }

    public void Execute()
    {
        // Break down userlist and display them as a string
        IEnumerable<User> user_list = _serviceFacade.GetConnectedUsers();
        string users = String.Join(", ", user_list.Select(user => user.Username));
        _serviceFacade.Speak(String.Format($"Connected users: {users}"), _user);
    }
}