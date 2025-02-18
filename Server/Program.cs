namespace Chat_Server;

class Program
{
    static void Main()
    {
        Server server = new Server(ServiceFacade.Instance);

        // Intercept ctrl+c for graceful shutdown 
        Console.CancelKeyPress += delegate {
            server.Shutdown();
        };

        server.Start();
    }
}