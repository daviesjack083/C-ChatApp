namespace Chat_Server;

public interface IChatService
{
    void AddUser(User user);
    void RemoveUser(User user);
    IEnumerable<User> GetConnectedUsers();
    void RecieveMessage(string incomingMessage, User user);
    void SendMessage(Byte[] payload, User user);
    void Announce(object message);
    void Speak(string message, User user);
    void LogEvent(string message);
}