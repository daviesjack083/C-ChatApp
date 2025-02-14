using System.Text;
using System.Text.Json;
using System.Reflection.Metadata;

namespace Chat;

public static class MessageService
{
    public static byte[] EncodeMessage(Message payload)
    {
        string json = JsonSerializer.Serialize(payload);
        return Encoding.UTF8.GetBytes(Encryption.EncryptMessage(json));
    }


    public static Message DecodeMessage(string message)
    {
        message = Encryption.DecryptMessage(message);
        return JsonSerializer.Deserialize<Message>(message);
    }
}