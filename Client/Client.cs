using System.Data.Common;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat;

public class Client
{
    private Guid Id;
    private String username = "undefined";
    private List<String> history = new();
    
    private IPEndPoint point = new IPEndPoint(IPAddress.Parse("192.168.0.46"), 6441);
    private Socket socket;

    public bool isRunning = false;


    public static void Main()
    {

        Client client = new Client();
        client.Start();

        Console.CancelKeyPress += delegate {
            client.Shutdown();
        };
    }


    public void Shutdown()
    {
        isRunning = false;
    }


    public void Start()
    {
        this.socket = new Socket(this.point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        // Username input
        Console.Write("Username: ");
        this.username = Console.ReadLine();
        if(this.username.Length < 1 || this.username.Length > 20) { this.username = "undefined"; }
        
        isRunning = true;
        Console.WriteLine("Connecting...");
        try
        {
            socket.Connect(this.point);
        }
        catch(Exception e)
        {
            Console.WriteLine("Could not connect to the server.");
            Console.WriteLine(e);
            return;
        }
        
        // Start listen thread
        Thread t = new Thread(new ThreadStart(Listen));
        t.Start();

        // Begin Chat
        while(t.IsAlive)
        {
            String message = Console.ReadLine();

            if (message.StartsWith("/name"))
            {
                username = message.Split(' ')[1];
                RefreshScreen();
            }
            else if (message.StartsWith("/guid"))
            {
                history.Add(Id.ToString());
            }
            
            if (message.Length >= 1)
            {
                Speak(message);
            }else{
                RefreshScreen();
            }

        }
    }


    public void Speak(String body)
    {
        Message message = new Message(Id, body, username);
        socket.Send(MessageService.EncodeMessage(message));
    }


    public void Listen()
    {
        while(isRunning)
        {
            try
            {
                byte[] payload = new byte[1024];
                int incoming = socket.Receive(payload);

                String message = Encoding.UTF8.GetString(payload, 0, incoming);
                Message mess = MessageService.DecodeMessage(message);

                if (mess.Type == "Command")
                {
                    if (mess.Body.StartsWith("/guid"))
                    {
                        Id = mess.Id;
                    }
                }else{
                    history.Add(String.Format("{0} - {1}: {2}", mess.Sent, mess.Username, mess.Body));
                }

                RefreshScreen();

            }

            catch (FormatException e)
            {
                history.Add("Malformed message recieved: " + e);
                RefreshScreen();
            }
            
            catch(SystemException e)
            {
                Console.WriteLine("Connection lost to server... Press enter to continue...");
                Console.WriteLine(e);
                return;
            }

        }
    }


    public void RefreshScreen()
    {
        Console.Clear();
        this.history.ForEach(delegate(string line)
        {
            Console.WriteLine(line);
        });
        Console.Write(": ");
    }

}
