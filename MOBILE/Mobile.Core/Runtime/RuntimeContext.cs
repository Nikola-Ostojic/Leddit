using Mobile.Core.Device;

namespace Mobile.Core.Runtime
{
    public class RuntimeContext : IRuntimeContext
    {
        public string AccessToken
        {
            get => AppSettings.AccessToken;
            set => AppSettings.AccessToken = value;
        }
        public string RefreshToken
        {
            get => AppSettings.RefreshToken;
            set => AppSettings.RefreshToken = value;
        }
        public string UserName
        {
            get => AppSettings.UserName;
            set => AppSettings.UserName = value;
        }
        public string Email
        {
            get => AppSettings.Email;
            set => AppSettings.Email = value;
        }
        public string Role
        {
            get => AppSettings.Role;
            set => AppSettings.Role = value;
        }

        public void RemoveData()
        {
            AppSettings.RemoveData();
        }
    }
}
