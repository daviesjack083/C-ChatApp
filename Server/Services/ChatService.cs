using System.Reflection.Metadata;
using System.Text;

namespace Chat_Server;

public class ChatService : IChatService
{
    private static ChatService _instance;
    private List<User> Users = new();
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
        Announce(String.Format($"{user.Ip} {user.Id} has joined!"));
        // A terrible, terrible temporary measure. Pinkie promise. 
        Thread.Sleep(50);
        Speak("/guid", user);
    }


    public void RemoveUser(User user)
    {
        Users.Remove(user);
        Announce(String.Format($"{user.Ip} has disconnected!"));
    }


    public IEnumerable<User> GetConnectedUsers()
    {
        return Users;
    }


    public void RecieveMessage(string incomingMessage, User user)
    {
        Message recievedMessage = MessageService.DecodeMessage(incomingMessage);
        LogEvent(recievedMessage);

        if(user.Username != recievedMessage.Username)
        {
            user.Username = recievedMessage.Username;
        }

        if (recievedMessage.Type == "Command")
        {
            commandFactory.CreateCommand(recievedMessage, user).Execute();
        } else {
            Announce(MessageService.EncodeMessage(recievedMessage));
        }
    }



    public void Announce(object message)
    {
        // If message is string, create message object and encode it
        byte[] messageToSend;
        if (message is string stringMessage)
        {
            LogEvent(message.ToString());
            Message messageObj = new Message(Guid.Empty, message.ToString(), "[*]Server");
            messageToSend = MessageService.EncodeMessage(messageObj);
        
        // If message is already byte array and just needs passing through
        }else{
            messageToSend = (byte[])message;
        }

        foreach (var user in Users)
        {
            SendMessage(messageToSend, user);
        }
    }



    public void SendMessage(Byte[] payload, User user)
    {
        lock(user.Socket)
        {
            user.Socket.Send(payload);
        }
    }


    public void Speak(string message, User user)
    {
        LogEvent(message);
        Message messagetosend = new Message(user.Id, message, "[*]Server");
        SendMessage(MessageService.EncodeMessage(messagetosend), user);
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