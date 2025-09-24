
namespace FistWeb.Data.Services
{
    public class LoginStateService
    {
        public bool IsLoggedIn { get; private set; } = false;
        public string Username { get; private set; }

        public void Login(string username)
        {
            IsLoggedIn = true;
            Username = username;
        }

        public void Logout()
        {
            IsLoggedIn = false;
            Username = null;
        }
    }
}
