using System.Net;
using System.Net.Sockets;

namespace Chat_Server;

public class User
{
    public Guid Id { get; set; }
    private string Username { get; set; }
    public readonly EndPoint Ip;
    public readonly Socket Socket;


    public User(Socket socket)
    {
        this.Id = Guid.NewGuid();
        this.Socket = socket;
        this.Ip = socket.RemoteEndPoint;
    }

    
    public bool IsAlive()
    {
        try
        {
            return !(Socket.Poll(1, SelectMode.SelectRead) && Socket.Available == 0);
        }
        catch (Exception)
        {
            return false;
        }
    }

}