using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace FistWeb.Data.Services
{
    public class AuthenticationService
    {

        private readonly ISessionStorageService _sessionStorage;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(
            ISessionStorageService sessionStorage,
            ILocalStorageService localStorage)
        {
            _sessionStorage = sessionStorage;
            _localStorage = localStorage;
        }

        public async Task LoginAsync(string username, bool rememberMe)
        {
            if (rememberMe)
            {
                await _localStorage.SetItemAsync("username", username);
                await _localStorage.SetItemAsync("isLoggedIn", true);
            }
            else
            {
                await _sessionStorage.SetItemAsync("username", username);
                await _sessionStorage.SetItemAsync("isLoggedIn", true);
            }
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var local = await _localStorage.GetItemAsync<bool>("isLoggedIn");
            if (local) return true;

            return await _sessionStorage.GetItemAsync<bool>("isLoggedIn");
        }

        public async Task<string> GetUsernameAsync()
        {
            var localUser = await _localStorage.GetItemAsync<string>("username");
            if (!string.IsNullOrEmpty(localUser)) return localUser;

            return await _sessionStorage.GetItemAsync<string>("username");
        }

        public async Task LogoutAsync()
        {
            await _sessionStorage.ClearAsync();
            await _localStorage.ClearAsync();
        }
    }
}
