using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat_Server;

public class Server
{

    private bool isRunning = false;
    private readonly ChatService _chatService;
    private readonly ChatController _chatController;


    public Server(ChatService chatService)
    {
        _chatService = chatService;
        _chatController = new ChatController(chatService);
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
        _chatService.LogEvent(String.Format("Server is now live on {0}", serverHost));
        while (this.isRunning) {        
            Socket clientSocket = socket.Accept();
            User user = new User(clientSocket);
            _chatService.AddUser(user);

            Thread t = new Thread(() => Listen(user));
            t.Start();
        }
    }



    private void Listen(User user)
    {
        _chatService.Speak(String.Format($"{user.Ip} has joined ID@ {user.Id}!"));
        while (isRunning)
        {
            byte[] bytes = new Byte[1024];
            if (user.Socket.Poll(200000, SelectMode.SelectRead))
            {
                int numByte = user.Socket.Receive(bytes);

                // if user has disconnected
                if (!user.IsAlive() || numByte == 0)
                {
                    _chatService.LogEvent(String.Format($"{user.Ip} has disconnected!"));
                    _chatService.RemoveUser(user);
                    return;
                }

                _chatService.RecieveMessage(bytes, numByte);
            }
        }
    }


    public void Shutdown()
    {
        _chatService.Speak("Server is shutting down!");
        isRunning = false;
    }
}
