namespace Chat_Server;

class Program
{
    static void Main()
    {
        ChatService chatService = new ChatService();
        Server server = new Server();

        Console.CancelKeyPress += delegate {
            server.Shutdown();
        };

        server.Start();
    }
}