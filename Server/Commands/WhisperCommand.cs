namespace Chat_Server;

public class WhisperCommand : ICommand
{
    private readonly User _user;
    private readonly IServiceFacade _serviceFacade;
    private readonly string[] _message;

    public WhisperCommand(User user, IServiceFacade serviceFacade, string[] message)
    {
        _user = user;
        _serviceFacade = serviceFacade;
        _message = message;
    }


    public void Execute()
    {
        var whisperTarget = _serviceFacade.LookupUsername(_message[1]);
        if (whisperTarget is null)
        {
            _serviceFacade.Speak("User not found!", _user);
        }else{
            string message = String.Join(" ", _message.Skip(2));
            _serviceFacade.Speak(String.Format($"[W]{_user.Username}: {message}"),
             whisperTarget);
        }
    }
}