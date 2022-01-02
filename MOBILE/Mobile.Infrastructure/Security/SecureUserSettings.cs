using Splat;
using System;

namespace Mobile.Infrastructure.Security
{
    public class SecureUserSettings : ISecureUserSettings
    {
        public string UserName
        {
            get => GetValue(UsernameKey);
            set => SetValue(UsernameKey, value);
        }

        public string Email
        {
            get => GetValue(EmailKey);
            set => SetValue(EmailKey, value);
        }

        public string Role
        {
            get => GetValue(RoleKey);
            set => SetValue(RoleKey, value);
        }

        public string AccessToken
        {
            get => GetValue(AccessTokenKey);
            set => SetValue(AccessTokenKey, value);
        }

        public string RefreshToken
        {
            get => GetValue(RefreshTokenKey);
            set => SetValue(RefreshTokenKey, value);
        }

        public const string UsernameKey = "UserName";
        public const string EmailKey = "Email";
        public const string AccessTokenKey = "AccessToken";
        public const string RefreshTokenKey = "RefreshToken";
        public const string RoleKey = "Role";

        private readonly ISecureStorage _secureStorage;

        public SecureUserSettings(ISecureStorage secureStorage = null)
        {
            _secureStorage = secureStorage
                ?? Locator.Current.GetService<ISecureStorage>();

            if (_secureStorage == null)
            {
                throw new NullReferenceException(nameof(secureStorage));
            }
        }

        public void ClearAll()
        {
            SetValue(UsernameKey, null);
            SetValue(EmailKey, null);
            SetValue(AccessTokenKey, null);
            SetValue(RoleKey, null);
            SetValue(RefreshTokenKey, null);
        }

        private string GetValue(string keyName)
        {
            string result = null;
            if (_secureStorage.Contains(keyName))
            {
                var data = _secureStorage.Retrieve(keyName);
                if (data != null)
                {
                    result = data;
                }
            }
            return result;
        }

        private void SetValue(string keyName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _secureStorage.Store(keyName, value);
            }
            else if (_secureStorage.Contains(keyName))
            {
                _secureStorage.Delete(keyName);
            }
        }
    }
}
