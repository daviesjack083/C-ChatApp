﻿using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat;

public class Client
{
    private String username = "undefined";
    private List<String> history = new();
    
    private IPEndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6441);
    private Socket socket;


    public static void Main()
    {
        Client client = new Client();
        client.Start();
    }


    public void Start()
    {
        this.socket = new Socket(this.point.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        // Username input
        Console.Write("Username: ");
        this.username = Console.ReadLine();
        if(this.username.Length < 1 || this.username.Length > 20) { this.username = "undefined"; }
        
        Console.WriteLine("Connecting...");
        try
        {
            socket.Connect(this.point);
            Speak($"/name {username}");
        }
        catch(Exception)
        {
            Console.WriteLine("Could not connect to the server.");
            return;
        }
        
        // Start listen thread
        Thread t = new Thread(new ThreadStart(Listen));
        t.Start();

        // Begin Chat
        while(t.IsAlive)
        {
            String message = Console.ReadLine();
            
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
        Message message = new Message(username, body);
        socket.Send(Encoding.UTF8.GetBytes(MessageService.EncodeMessage(message)));
    }


    public void Listen()
    {
        while(true)
        {
            try
            {
                byte[] payload = new byte[1024];
                int incoming = socket.Receive(payload);

                String message = Encoding.UTF8.GetString(payload, 0, incoming);
                Message mess = MessageService.DecodeMessage(message);
                history.Add(String.Format("{0} - {1}: {2}", mess.Sent, mess.Username, mess.Body));
                RefreshScreen();

            }

            catch (FormatException e)
            {
                history.Add("Malformed message recieved. Ignoring");
                RefreshScreen();
            }
            
            catch(SystemException e)
            {
                Console.WriteLine("Connection lost to server... Press enter to continue...");
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
