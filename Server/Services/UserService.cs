namespace Chat_Server;

public class UserService
{
    private List<User> Users = new List<User>();


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