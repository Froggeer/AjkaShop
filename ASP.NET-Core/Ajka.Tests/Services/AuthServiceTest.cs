using Ajka.BL.Queries.Interfaces;
using Ajka.BL.Services.Base;
using Ajka.Common.Helpers;
using Ajka.DAL.Model.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ajka.Tests.Services
{
    public class AuthServiceTest
    {
        [Fact]
        public void Authenticate_Succeeds()
        {
            // Arrange

            var _ajkaShopDbContext = new Mock<IAjkaShopDbContext>();
            var password = "pass1234";
            var randomString = new Random().Next(9, 999).ToString();

            IOptions<AppSettings> _appSettings = Options.Create(new AppSettings
            {
                PasswordSalt = "123456",
                ClientSecret = "12345678913456789456134567894561"
            });

            var _userQueries = new Mock<IUserQueries>();
            _userQueries.Setup(x => x.GetUserAsync(It.IsAny<IAjkaShopDbContext>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new BL.Models.User.UserDto
                {
                    Name = randomString,
                    Surname = randomString,
                    Email = randomString,
                    Password = "+PVoavUV8nRFTavrTinisH4YEZsiC2/15MejjZ0PLik=",
                    IsAdministrator = true
                }));

            var serviceForTest = new AuthService(_ajkaShopDbContext.Object, _userQueries.Object, _appSettings);

            // Act

            var result = serviceForTest.Authenticate(randomString, password, CancellationToken.None).Result;

            // Assert

            // Login is successfull and access token is created.
            Assert.NotEmpty(result.AccessToken);

            Assert.Null(result.ErrorMessage);
        }
    }
}
