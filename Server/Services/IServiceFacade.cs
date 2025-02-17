namespace Chat_Server;

public interface IServiceFacade
{
    void AddUser(User user);
    void RemoveUser(User user);
    IEnumerable<User> GetConnectedUsers();

    void RecieveMessage(string incomingMessage, User user);
    void Announce(object message);
    void Speak(string message, User user);
    void LogEvent(string message);
    void LogEvent(Message message);
}