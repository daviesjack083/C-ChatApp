using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat_Server;

public class Server
{

    private bool isRunning = false;
    private readonly IServiceFacade _serviceFacade;


    public Server()
    {
        _serviceFacade = new ServiceFacade();
    }


    public void Start()
    {
        // socket parameters
        IPAddress serverHost = IPAddress.Parse("127.0.0.1");
        IPEndPoint serverEnd = new IPEndPoint(serverHost, 6441);
        
        // Open listen socket
        Socket socket = new Socket(serverHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(serverEnd);
        socket.Listen(10);

        // Listen for connections and create thread for new users
        isRunning = true;
        _serviceFacade.LogEvent(String.Format("Server is now live on {0}", serverHost));
        while (this.isRunning) {        
            Socket clientSocket = socket.Accept();
            User user = new User(clientSocket);
            _serviceFacade.AddUser(user);

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

                    // if user has disconnected, clean them up
                    if (!user.IsAlive() || numByte == 0)
                    {
                        _serviceFacade.RemoveUser(user);
                        return;
                    }

                    string incomingMessage = Encoding.UTF8.GetString(bytes, 0, numByte);
                    _serviceFacade.RecieveMessage(incomingMessage, user);
                }
            }
            catch(SocketException)
            {
                _serviceFacade.LogEvent(String.Format($"Connection reset: {user.Ip}"));
                _serviceFacade.RemoveUser(user);
            }
        }
    }


    public void Shutdown()
    {
        _serviceFacade.Announce("Server is shutting down! Goodbye!");
        isRunning = false;
    }
}
