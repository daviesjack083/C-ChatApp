using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat_Server;

public class Server
{

    private bool isRunning = false;
    private readonly ChatService _chatService;


    public Server()
    {
        _chatService = ChatService.Instance;
    }


    public void Start()
    {
        // socket parameters
        IPAddress serverHost = IPAddress.Parse("192.168.0.46");
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
        while (isRunning)
        {
            try
            {
                byte[] bytes = new Byte[1024];
                if (user.Socket.Poll(200000, SelectMode.SelectRead))
                {
                    int numByte = user.Socket.Receive(bytes);

                    // if user has disconnected
                    if (!user.IsAlive() || numByte == 0)
                    {
                        _chatService.RemoveUser(user);
                        return;
                    }

                    string incomingMessage = Encoding.UTF8.GetString(bytes, 0, numByte);
                    _chatService.RecieveMessage(incomingMessage, user);
                }
            }
            catch(SocketException e)
            {
                _chatService.LogEvent(String.Format($"Connection reset: {user.Ip}"));
                _chatService.RemoveUser(user);
            }
        }
    }


    public void Shutdown()
    {
        _chatService.Announce("Server is shutting down!");
        isRunning = false;
    }
}
