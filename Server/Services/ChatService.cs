using System.Text;

namespace Chat_Server;

public class ChatService : IChatService
{
    private static List<User> Users = new();


    public void AddUser(User user)
    {
        Users.Add(user);
    }


    public void RemoveUser(User user)
    {
        Users.Remove(user);
    }


    public IEnumerable<User> GetConnectedUsers()
    {
        return Users;
    }


    public void RecieveMessage(byte[] bytes, int byteLength)
    {
        string message = Encoding.UTF8.GetString(bytes, 0, byteLength);
        Message recievedMessage = MessageService.DecodeMessage(message);
        string decoded_message = String.Format("{0}: {1}", recievedMessage.Username, recievedMessage.Body);
        LogEvent(decoded_message);

        if (recievedMessage.Body.StartsWith("/"))
        {
            
        } else {
            BroadcastMessage(message);
        }
    }


    public void BroadcastMessage(string message)
    {
        foreach(var user in Users)
        {
            user.Socket.Send(Encoding.UTF8.GetBytes(message));
        }
    }


    public void Speak(string message)
    {
        LogEvent(message);
        Message messagetosend = new Message("[*]Server", message);
        BroadcastMessage(MessageService.EncodeMessage(messagetosend));
    }


    public void LogEvent(string message)
    {
        DateTime currentTime = DateTime.Now;
        string log = String.Format("{0} - {1}", currentTime.ToString("HH:mm:ss"), message);
        Console.WriteLine(log);
    }
}