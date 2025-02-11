//using Microsoft.AspNetCore.Mvc;


namespace Chat_Server;

public class ChatController
{
    private readonly IChatService _chatService;


    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }


}