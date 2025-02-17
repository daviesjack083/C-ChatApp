namespace Chat_Server;

class Program
{
    static void Main()
    {
        // Initialise singletons & Server
        ChatService chatService = new ChatService();
        UserService userService = new UserService();
        Server server = new Server();

        // Intercept ctrl+c for graceful shutdown 
        Console.CancelKeyPress += delegate {
            server.Shutdown();
        };

        server.Start();
    }
}