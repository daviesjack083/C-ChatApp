using System.Reflection.Metadata;
using System.Text;

namespace Chat_Server;

public class ChatService
{
    private CommandFactory commandFactory;


    public ChatService()
    {
        commandFactory = new CommandFactory();
    }


    public void RecieveMessage(string incomingMessage, User user, IEnumerable<User> users = null)
    {
        Message recievedMessage = MessageService.DecodeMessage(incomingMessage);
        LogEvent(recievedMessage);

        // Set username to whatever the clients current username is
        if(user.Username != recievedMessage.Username)
        {
            user.Username = recievedMessage.Username;
        }

        // If command, pass to CommandFactory, else retransmit the message to other clients
        if (recievedMessage.Type == "Command")
        {
            commandFactory.CreateCommand(recievedMessage, user).Execute();
        } else {
            Announce(MessageService.EncodeMessage(recievedMessage), users);
        }
    }



    public void Announce(object message, IEnumerable<User> users)
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

        foreach (var user in users)
        {
            SendMessage(messageToSend, user);
        }
    }



    private void SendMessage(Byte[] payload, User user)
    {
        lock(user.Socket)
        {
            user.Socket.Send(payload);
        }
    }


    public void Speak(string message, User user)
    {
        LogEvent(message);
        Message messagetosend = new Message(user.Id, message, "");
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