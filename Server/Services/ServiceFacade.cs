namespace Chat_Server;

public class ServiceFacade : IServiceFacade
{
    private static ServiceFacade _instance;
    private readonly UserService _userService;
    private readonly ChatService _chatService;


    public static ServiceFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ServiceFacade();
            }
            return _instance;
        }
    }


    public ServiceFacade()
    {
        if (_userService is null)
        {
            _userService = new UserService();
        }

        if (_chatService is null)
        {
            _chatService = new ChatService();
        }
    }


    public void AddUser(User user)
    {
        _userService.AddUser(user);
        _chatService.Announce(String.Format($"{user.Ip} {user.Id} has joined!"), _userService.GetConnectedUsers());
        // Assign the user their GUID
        _chatService.Speak("/guid", user);
    }


    public void RemoveUser(User user)
    {
        _userService.RemoveUser(user);
        _chatService.Announce(String.Format($"{user.Ip} has disconnected!"), _userService.GetConnectedUsers());
    }


    public User? LookupUsername(string Username)
    {
        return _userService.LookupUsername(Username);
    }

    
    public IEnumerable<User> GetConnectedUsers()
    {
        return _userService.GetConnectedUsers();
    }


    public void RecieveMessage(string incomingMessage, User user)
    {
        _chatService.RecieveMessage(incomingMessage, user, _userService.GetConnectedUsers());
    }


    public void Announce(object message)
    {
        _chatService.Announce(message, _userService.GetConnectedUsers());
    }


    public void Speak(string message, User user)
    {
        _chatService.Speak(message, user);
    }


    public void LogEvent(string message)
    {
        _chatService.LogEvent(message);
    }


    public void LogEvent(Message message)
    {
        _chatService.LogEvent(message);
    }
}