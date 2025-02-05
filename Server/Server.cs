using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatEncrypt;

namespace Chat_Server;

public class Server
{
    Encryption _encrypt = new Encryption();

    private bool isRunning = false;
    public static List<User> Users = new();


    public static void Main()
    {
        Server server = new Server();
        
        // Shutdown server when CTRL+C is pressed
        Console.CancelKeyPress += delegate {
            server.Shutdown();
        };
        server.Start();
    }


    public void Start()
    {
        // socket parameters
        IPAddress serverHost = IPAddress.Parse("127.0.0.1");
        IPEndPoint serverEnd = new IPEndPoint(serverHost, 6441);
        
        // Declare listen socket
        Socket socket = new Socket(serverHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(serverEnd);
        socket.Listen(10);

        // Listen for connections
        isRunning = true;
        LogEvent(String.Format("Server is now live on {0}", serverHost));
        while (this.isRunning) {        
            Socket clientSocket = socket.Accept();
            User user = new User(clientSocket);
            Users.Add(user);

            Thread t = new Thread(() => Listen(user));
            t.Start();
        }
    }



    private void Listen(User user)
    {
        Speak(String.Format($"{user.Ip} has joined! Connected Clients: {Users.Count()}"));
        while (this.isRunning)
        {
            byte[] bytes = new Byte[1024];
            if (user.Socket.Poll(200000, SelectMode.SelectRead))
            {
                int numByte = user.Socket.Receive(bytes);
                string message = Encoding.UTF8.GetString(bytes, 0, numByte);

                // if user has disconnected
                if (!user.IsAlive() || numByte == 0)
                {
                    LogEvent(String.Format($"{user.Ip} has disconnected!"));
                    Users.Remove(user);
                    return;
                }

                // interpret message
                IDictionary<String, String> payload = DecodeMessage(message);
                if (payload["message"].StartsWith("/"))
                {
                    continue;
                } else {
                    // Broadcast message
                    Users.ForEach(delegate(User user1) { user1.Socket.Send(Encoding.UTF8.GetBytes(message)); });
                    //Speak(message);
                }
            }
        }
    }

    
    public void Speak(String message)
    {
        LogEvent(message);
        IDictionary<String, String> payload = new Dictionary<String, String>
        {
            { "username", "[*]Server" },
            { "message", message }
        };

        DateTime currentDateTime = DateTime.Now;
        payload.Add("sent", currentDateTime.ToString("HH:mm:ss"));

        string json = JsonSerializer.Serialize(payload);
        string encrypted = _encrypt.EncryptMessage(json);
        
        Users.ForEach(delegate(User user1)
        {
            user1.Socket.Send(Encoding.UTF8.GetBytes(encrypted));
        });
    }
    
    
    public void Speak(String message, Socket client)
    {
        LogEvent(message);
        IDictionary<String, String> payload = new Dictionary<String, String>
        {
            { "username", "[*]Server" },
            { "message", message }
        };

        DateTime currentDateTime = DateTime.Now;
        payload.Add("sent", currentDateTime.ToString("HH:mm:ss"));

        string json = JsonSerializer.Serialize(payload);
        string encrypted = _encrypt.EncryptMessage(json);

        client.Send(Encoding.UTF8.GetBytes(encrypted));
    }
    

    public IDictionary<String, String> DecodeMessage(String message)
    {
        try
        {
            message = _encrypt.DecryptMessage(message);
            IDictionary<String, String> payload = JsonSerializer.Deserialize<Dictionary<String, String>>(message);
            message = String.Format("{0}: {1}", payload["username"], payload["message"]);
            LogEvent(message);
            return payload;
        } catch (JsonException)
        {
            return null;
        }
    }
    

    public static void LogEvent(String message)
    {
        DateTime currentTime = DateTime.Now;
        string log = String.Format("{0} - {1}", currentTime.ToString("HH:mm:ss"), message);
        Console.WriteLine(log);
    }



    public void Shutdown()
    {
        LogEvent("Shutting down server...");
        isRunning = false;
    }
    
}
