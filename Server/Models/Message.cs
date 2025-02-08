using System.Net;
using System.Net.Sockets;

namespace Chat_Server;

public class Message
{
    public Guid UserID {get; set;}
    public string Body {get; set;}
    public DateTime Sent {get; set;}


    public Message()
    {
        
    }
}