using Backend.Api.DTOs.Request;
using Backend.Api.DTOs.Response;
using Backend.Api.Security;
using Backend.Core.Interfaces;
using Backend.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtHandler _jwtHandler;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
           IUserService userService,
           IRefreshTokenService refreshTokenService,
           JwtHandler jwtHandler,
           ILogger<AuthController> logger
            )
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }


        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            _logger.LogDebug("Login invoked");
            var user = await _userService.GetUser(model.Email, HashPassword(model.Password));

            if (user == null)
            {
                _logger.LogDebug("Login failed");
                return BadRequest("Invalid login request");
            }

            var (token, validFor) = _jwtHandler.CreateRefreshToken();
            var refreshToken = await _refreshTokenService.Create(token, validFor, user);

            _logger.LogDebug("Login successful");
            return Ok(CreateTokensResponse(user.UserName, user.Email, user.Role.ToString(), refreshToken.Token));
        }


        [HttpPost]
        [ActionName("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            _logger.LogDebug("Register invoked");
            var userExistsWithTheGivenEmail = await _userService.GetUserByEmail(model.Email);
            if (userExistsWithTheGivenEmail != null)
            {
                _logger.LogDebug("Register failed");
                return BadRequest("The user with the given email already exists.");
            }

            // Create an appropriate mapper going from DTO to Entity
            var userEntity = new UserEntity
            {
                Email = model.Email,
                UserName = model.UserName,
                Password = HashPassword(model.Password),
                // Hard coded role
                Role = Role.User
            };

            var user = await _userService.Create(userEntity);

            var (token, validFor) = _jwtHandler.CreateRefreshToken();
            var refreshToken = await _refreshTokenService.Create(token, validFor, user);
            var tokens = CreateTokensResponse(user.UserName, user.Email, user.Role.ToString(), refreshToken.Token);

            _logger.LogDebug("Register successful");
            return Ok(tokens);
        }

        [HttpPost]
        [ActionName("token")]
        public async Task<IActionResult> Token([FromBody] TokenRequestDTO model)
        {
            _logger.LogDebug("Token invoked");
            var storedRefreshToken = await _refreshTokenService.GetToken(model.RefreshToken);

            if (storedRefreshToken == null || storedRefreshToken.ValidFor < DateTime.Now)
            {
                _logger.LogDebug("Token failed");
                return Unauthorized("Invalid refresh token, please login again.");
            }

            var (token, validFor) = _jwtHandler.CreateRefreshToken();
            storedRefreshToken.ValidFor = validFor;
            storedRefreshToken.Token = token;

            var updatedRefreshToken = await _refreshTokenService.Update(storedRefreshToken);
            var tokens = CreateTokensResponse(updatedRefreshToken.User.UserName, updatedRefreshToken.User.Email, updatedRefreshToken.User.Role.ToString(), updatedRefreshToken.Token);

            _logger.LogDebug("Token successful");
            return Ok(tokens);
        }


        [HttpPost]
        [ActionName("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _logger.LogDebug("Logout invoked");
            var accessToken = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var email = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

            await _refreshTokenService.RevokeToken(email);
            _logger.LogDebug("User logged out.");
            return Ok();
        }

        // Naive hashing algorithm 
        private string HashPassword(string password)
        {
            using (var algorithm = SHA256.Create())
            {
                var bytes = algorithm.ComputeHash(Encoding.ASCII.GetBytes(password));
                return string.Concat(bytes.Select(b => b.ToString("x2")));
            }
        }

        private TokenResponseDTO CreateTokensResponse(string username, string email, string role, string refreshToken) => new TokenResponseDTO
        {
            AccessToken = _jwtHandler.CreateAccessToken(username, email, role),
            RefreshToken = refreshToken
        };
    }
}