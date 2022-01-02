using Mobile.Core.Api;
using Mobile.Core.Api.Rest;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Dtos.Response;
using Mobile.Core.Runtime;
using Mobile.Core.Services.AuthenticationService;
using Moq;
using NUnit.Framework;
using System;
using System.Reactive.Linq;

namespace Mobile.Core.UnitTests.Services
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        private IAuthenticationService _target;
        private Mock<IApiService<IAuthApi>> _authApiServiceMock;
        private Mock<IAuthApi> _authApiMock;
        private Mock<IRuntimeContext> _runtimeContextMock;


        [SetUp]
        public void Setup()
        {
            _authApiMock = new Mock<IAuthApi>();
            _authApiServiceMock = new Mock<IApiService<IAuthApi>>();
            _runtimeContextMock = new Mock<IRuntimeContext>();

            _authApiMock
                .Setup(x => x.Login(It.IsAny<LoginRequestDTO>()))
                .Returns(() => Observable.Return(
                    new TokenResponseDTO
                    {
                        RefreshToken = "Refresh token",
                        AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJjNWFkZTIyNy1hMTMxLTQ2ZGEtYjIxYS0wNmI5OTZiNzU4Y2YiLCJpYXQiOiI5LzIyLzIwMjAgMTU6MDU6MzUiLCJzdWIiOiJ1c2VyQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiVXNlciIsImV4cCI6MTYwMDc4NzM3NSwiaXNzIjoiTGV2aTkgQmFja2VuZCIsImF1ZCI6Imxldmk5VXNlcnMiLCJyb2xlcyI6WyJVc2VyIl19.9yNEUjI3dlwE0GKoG7g3uAkgN8CEb8WrZ0AhTu1ViJk"
                    })
                );

            _authApiMock
               .Setup(x => x.Register(It.IsAny<RegisterRequestDTO>()))
               .Returns(() => Observable.Return(
                   new TokenResponseDTO
                   {
                       RefreshToken = "Refresh token",
                       AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJjNWFkZTIyNy1hMTMxLTQ2ZGEtYjIxYS0wNmI5OTZiNzU4Y2YiLCJpYXQiOiI5LzIyLzIwMjAgMTU6MDU6MzUiLCJzdWIiOiJ1c2VyQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiVXNlciIsImV4cCI6MTYwMDc4NzM3NSwiaXNzIjoiTGV2aTkgQmFja2VuZCIsImF1ZCI6Imxldmk5VXNlcnMiLCJyb2xlcyI6WyJVc2VyIl19.9yNEUjI3dlwE0GKoG7g3uAkgN8CEb8WrZ0AhTu1ViJk"
                   }));

            _authApiMock
              .Setup(x => x.Token(It.IsAny<TokenRequestDTO>()))
              .Returns(() => Observable.Return(
                   new TokenResponseDTO
                   {
                       RefreshToken = "Refresh token",
                       AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJjNWFkZTIyNy1hMTMxLTQ2ZGEtYjIxYS0wNmI5OTZiNzU4Y2YiLCJpYXQiOiI5LzIyLzIwMjAgMTU6MDU6MzUiLCJzdWIiOiJ1c2VyQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiVXNlciIsImV4cCI6MTYwMDc4NzM3NSwiaXNzIjoiTGV2aTkgQmFja2VuZCIsImF1ZCI6Imxldmk5VXNlcnMiLCJyb2xlcyI6WyJVc2VyIl19.9yNEUjI3dlwE0GKoG7g3uAkgN8CEb8WrZ0AhTu1ViJk"
                   }));

            _authApiServiceMock = new Mock<IApiService<IAuthApi>>();
            _authApiServiceMock
                .Setup(x => x.GetClient())
                .Returns(() => _authApiMock.Object);

        }

        [Test]
        public void Login_ShouldCallLoginRestEndpoint()
        {
            // Arrange
            _target = new AuthenticationService(_authApiServiceMock.Object, _runtimeContextMock.Object);


            // Act
            _target.Login("Email", "Password").Subscribe();


            // Assert
            _authApiMock.Verify(x => x.Login(It.IsAny<LoginRequestDTO>()), Times.Once);
        }

        [Test]
        public void Register_ShouldCallRegisterRestEndpoint()
        {
            // Arrange
            _target = new AuthenticationService(_authApiServiceMock.Object, _runtimeContextMock.Object);


            // Act
            _target.Register("UserName", "Email", "Pass", "Pass").Subscribe();


            // Assert
            _authApiMock.Verify(x => x.Register(It.IsAny<RegisterRequestDTO>()), Times.Once);
        }

        [Test]
        public void RenewSession_ShouldCallTokenRestEndpoint()
        {
            // Arrange
            _target = new AuthenticationService(_authApiServiceMock.Object, _runtimeContextMock.Object);


            // Act
            _target.RenewSession("Refresh token").Subscribe();


            // Assert
            _authApiMock.Verify(x => x.Token(It.IsAny<TokenRequestDTO>()), Times.Once);
        }
    }
}
