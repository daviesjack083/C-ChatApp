using System.Text.Json;

namespace Chat;

public static class MessageService
{
    public static string EncodeMessage(Message payload)
    {
        string json = JsonSerializer.Serialize(payload);
        return Encryption.EncryptMessage(json);
    }


    public static Message DecodeMessage(string message)
    {
        message = Encryption.DecryptMessage(message);
        return JsonSerializer.Deserialize<Message>(message);
    }
}