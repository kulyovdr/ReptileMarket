namespace ReptileMarket.Frontend.Services
{
    public class AccountState
    {
        public bool IsLoggedIn { get; private set; }

        public string DisplayName { get; private set; } = string.Empty;

        public event Action? Changed;

        public void Login(string displayName)
        {
            IsLoggedIn = true;
            DisplayName = displayName;
            Changed?.Invoke();
        }

        public void Logout()
        {
            IsLoggedIn = false;
            DisplayName = string.Empty;
            Changed?.Invoke();
        }
    }
}
