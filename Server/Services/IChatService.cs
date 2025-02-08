namespace Chat_Server;

public interface IChatService
{
    void AddUser(User user);
    void RemoveUser(User user);
    IEnumerable<User> GetConnectedUsers();
    void RecieveMessage(byte[] bytes, int byteLength);
    void BroadcastMessage(string message);
    void Speak(string message);
    void LogEvent(string message);
}