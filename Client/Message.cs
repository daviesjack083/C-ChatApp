
namespace Chat;

public class Message
{
    public string Username {get; set;}
    public string Body {get; set;}
    public DateTime Sent {get; set;}


    public Message(string username, string body)
    {
        Username = username;
        Body = body;
        Sent = DateTime.Now;
    }
}