using System.Text.Json;

namespace Chat_Common;

public static class MessageService
{
    public static string EncodeMessage<T>(T Payload)
    {
        string json = JsonSerializer.Serialize(Payload);
        return Encryption.EncryptMessage(json);
    }


    public static IDictionary<String, String> DecodeMessage<T>(string message)
    {
        message = Encryption.DecryptMessage(message);
        return JsonSerializer.Deserialize<Dictionary<String, String>>(message);
    }
}