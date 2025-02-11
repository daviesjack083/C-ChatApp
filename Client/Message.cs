
namespace Chat;

public class Message
{
    public Guid Id {get; set;}
    public string Username {get; set;}
    public string Type {get; set;}
    public string Body {get; set;}
    public DateTime Sent {get; set;}


    public Message(Guid id, string body, string username = null)
    {
        Id = id;
        Username = username;
        Body = body;
        Sent = DateTime.Now;

        if (body.StartsWith('/'))
        {
            Type = "Command";
        }else{
            Type = "Message";
        }
    }
}