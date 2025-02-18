namespace Chat_Server;

public class UserService
{
    private static UserService _instance;
    private List<User> Users = new List<User>();


    public static UserService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UserService();
            }
            return _instance;
        }
    }


    public void AddUser(User user)
    {
        Users.Add(user);
    }


    public void RemoveUser(User user)
    {
        Users.Remove(user);
    }


    public User? LookupUsername(string username)
    {
        return Users.FirstOrDefault(user => String.Equals(
            user.Username, username, StringComparison.OrdinalIgnoreCase
        ));
    }


    public IEnumerable<User> GetConnectedUsers()
    {
        return Users;
    }
}