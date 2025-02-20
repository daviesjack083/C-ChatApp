using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Chat_Server;

public class ChatService
{
    private readonly CommandFactory _commandFactory;

    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);
    private ConcurrentQueue<Tuple<Byte[], User>> _messageQueue = new ConcurrentQueue<Tuple<Byte[], User>>();


    public ChatService()
    {
        _commandFactory = new CommandFactory();
        Task.Run(() => SendMessage());
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
            _commandFactory.CreateCommand(recievedMessage, user).Execute();
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
            _messageQueue.Enqueue(Tuple.Create(messageToSend, user));
            _semaphore.Release();
        }
    }


    private async Task SendMessage()
    {
        while(true)
        {
            await _semaphore.WaitAsync();

            Tuple<Byte[], User> itemQueue;
            while(_messageQueue.TryDequeue(out itemQueue))
            {
                await itemQueue.Item2.Socket.SendAsync(itemQueue.Item1);
            }
            
        }
    }


    public void Speak(string message, User user)
    {
        LogEvent(message);
        Message messagetosend = new Message(user.Id, message, "");
        _messageQueue.Enqueue(Tuple.Create(MessageService.EncodeMessage(messagetosend), user));
        _semaphore.Release();
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