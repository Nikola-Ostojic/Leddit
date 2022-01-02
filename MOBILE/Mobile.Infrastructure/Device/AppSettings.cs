using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Mobile.Infrastructure.Device
{
    public static class AppSettings
    {
        private static ISettings Settings => CrossSettings.Current;

        public static string UserName
        {
            get => Settings.GetValueOrDefault(nameof(UserName), default(string));

            set => Settings.AddOrUpdateValue(nameof(UserName), value);
        }

        public static string Email
        {
            get => Settings.GetValueOrDefault(nameof(Email), default(string));

            set => Settings.AddOrUpdateValue(nameof(Email), value);
        }

        public static string AccessToken
        {
            get => Settings.GetValueOrDefault(nameof(AccessToken), default(string));

            set => Settings.AddOrUpdateValue(nameof(AccessToken), value);
        }

        public static string Role
        {
            get => Settings.GetValueOrDefault(nameof(Role), default(string));

            set => Settings.AddOrUpdateValue(nameof(Role), value);
        }

        public static string RefreshToken
        {
            get => Settings.GetValueOrDefault(nameof(RefreshToken), default(string));

            set => Settings.AddOrUpdateValue(nameof(RefreshToken), value);
        }

        public static void RemoveData()
        {
            Settings.Remove(nameof(AccessToken));
            Settings.Remove(nameof(RefreshToken));
            Settings.Remove(nameof(UserName));
            Settings.Remove(nameof(Email));
            Settings.Remove(nameof(Role));
        }
    }
}
