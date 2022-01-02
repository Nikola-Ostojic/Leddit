using System;

namespace Mobile.Core.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        IObservable<bool> Login(string userName, string password);
        IObservable<bool> Register(string userName, string email, string password, string confirmPassword);
        IObservable<bool> Logout();
        IObservable<bool> RenewSession(string refreshToken);
        IObservable<bool> IsTokenExpired(string token);
    }
}
