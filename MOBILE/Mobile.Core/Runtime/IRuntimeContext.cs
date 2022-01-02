namespace Mobile.Core.Runtime
{
    public interface IRuntimeContext
    {
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        string Role { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        void RemoveData();
    }
}
