using System.Reflection.Metadata;
using System.Text;

namespace Chat_Server;

public class ChatService : IChatService
{
    private static ChatService _instance;
    private static List<User> Users = new();
    private CommandFactory commandFactory;


    public ChatService()
    {
        commandFactory = new CommandFactory();
    }


    public static ChatService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ChatService();
            }
            return _instance;
        }
    }


    public void AddUser(User user)
    {
        Users.Add(user);
        Message message = new Message(user.Id, "/guid", "Server");
        user.Socket.Send(MessageService.EncodeMessage(message));
        Speak(String.Format($"{user.Ip} has joined!"));
    }


    public void RemoveUser(User user)
    {
        Users.Remove(user);
        Speak(String.Format($"{user.Ip} has disconnected!"));
    }


    public IEnumerable<User> GetConnectedUsers()
    {
        return Users;
    }


    public void RecieveMessage(byte[] bytes, int byteLength, User user)
    {
        string message = Encoding.UTF8.GetString(bytes, 0, byteLength);
        Message recievedMessage = MessageService.DecodeMessage(message);
        LogEvent(recievedMessage);

        if(user.Username != recievedMessage.Username)
        {
            user.Username = recievedMessage.Username;
        }

        if (recievedMessage.Type == "Command")
        {
            commandFactory.CreateCommand(recievedMessage, user).Execute();
        } else {
            BroadcastMessage(MessageService.EncodeMessage(recievedMessage));
        }
    }


    public void BroadcastMessage(byte[] message)
    {
        foreach(var user in Users)
        {
            user.Socket.Send(message);
        }
    }


    public void Speak(string message)
    {
        Message messagetosend = new Message(Guid.NewGuid(), message, "[*]Server");
        LogEvent(messagetosend);
        BroadcastMessage(MessageService.EncodeMessage(messagetosend));
    }


    public void LogEvent(string message)
    {
        DateTime currentTime = DateTime.Now;
        Console.WriteLine(String.Format($"{currentTime:HH:mm:ss} - {message}"));
    }


    public void LogEvent(Message message)
    {
        Console.WriteLine(String.Format($"{message.Sent:HH:mm:ss} - {message.Username}: {message.Body}"));    
    }
}