using Mobile.Core.Api;
using Mobile.Core.Api.Rest;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Runtime;
using Splat;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Mobile.Core.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IApiService<IAuthApi> _authApi;
        private readonly IRuntimeContext _runtimeContext;

        public AuthenticationService(IApiService<IAuthApi> authApi = null, IRuntimeContext runtimeContext = null)
        {
            _authApi = authApi ?? Locator.Current.GetService<IApiService<IAuthApi>>();
            _runtimeContext = runtimeContext ?? Locator.Current.GetService<IRuntimeContext>();

        }
        public IObservable<bool> Login(string email, string password)
        {
            var loginRequestDto = new LoginRequestDTO
            {
                Email = email,
                Password = password
            };
            return _authApi.GetClient().Login(loginRequestDto).Select(response =>
             {
                 var res = response;
                 var handler = new JwtSecurityTokenHandler();
                 var token = handler.ReadJwtToken(response.AccessToken);
                 _runtimeContext.AccessToken = response.AccessToken;
                 _runtimeContext.RefreshToken = response.RefreshToken;
                 _runtimeContext.UserName = token.Payload.FirstOrDefault(x => x.Key == "username").Value.ToString();
                 _runtimeContext.Role = token.Claims.FirstOrDefault(x => x.Type == "roles").Value.ToString();
                 _runtimeContext.Email = email;
                 return true;
             });
        }

        public IObservable<bool> Register(string username, string email, string password, string confirmPassword)
        {
            var registerDto = new RegisterRequestDTO
            {
                Email = email,
                UserName = username,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            return _authApi.GetClient().Register(registerDto).Select(response =>
            {
                var res = response;
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(response.AccessToken);
                _runtimeContext.AccessToken = response.AccessToken;
                _runtimeContext.RefreshToken = response.RefreshToken;
                _runtimeContext.UserName = token.Payload.FirstOrDefault(x => x.Key == "username").Value.ToString();
                _runtimeContext.Role = token.Claims.FirstOrDefault(x => x.Type == "roles").Value.ToString();
                _runtimeContext.Email = email;
                return true;
            });
        }

        public IObservable<bool> Logout()
        {
            return Observable.Create((IObserver<bool> observer) =>
            {
                _runtimeContext.RemoveData();
                observer.OnNext(true);
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        public IObservable<bool> RenewSession(string refreshToken)
        {
            return _authApi.GetClient().Token(new TokenRequestDTO { RefreshToken = refreshToken }).SelectMany(response =>
            {
                var res = response;
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(response.AccessToken);
                _runtimeContext.AccessToken = response.AccessToken;
                _runtimeContext.RefreshToken = response.RefreshToken;
                _runtimeContext.UserName = token.Payload.FirstOrDefault(x => x.Key == "username").Value.ToString();
                _runtimeContext.Role = token.Claims.FirstOrDefault(x => x.Type == "roles").Value.ToString();
                _runtimeContext.Email = token.Payload.FirstOrDefault(x => x.Key == "sub").Value.ToString(); ;
                return Observable.Return(true);
            });
        }

        public IObservable<bool> IsTokenExpired(string token)
        {
            return Observable.Create((IObserver<bool> observer) =>
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)handler.ReadToken(token);
                var result = DateTime.UtcNow > jwtToken.ValidTo;
                observer.OnNext(result);
                observer.OnCompleted();

                return Disposable.Empty;
            });

        }
    }
}
