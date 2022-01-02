using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Refit;
using System;

namespace Mobile.Core.Api.Rest
{
    [Headers("Content-Type: application/json", "Accept: application/json")]
    public interface IAuthApi
    {
        [Post("/auth/login")]
        IObservable<TokenResponseDTO> Login([Body] LoginRequestDTO loginDto);

        [Post("/auth/register")]
        IObservable<TokenResponseDTO> Register([Body] RegisterRequestDTO registerDto);

        [Post("/auth/token")]
        IObservable<TokenResponseDTO> Token([Body] TokenRequestDTO refreshToken);
    }
}
