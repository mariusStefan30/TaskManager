using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Controllers;
using Xunit;

namespace TaskManager.Tests
{
	public class AuthControllerTest
	{
	

		[Fact]
		public async Task Login_ReturnsUnauthorized_WhenUserDoesNotExist()
		{
			// Arrange
			var loginDto = new LoginDto("invalidUser", "anyPassword");

			var userMgrMock = new Mock<UserManager<IdentityUser>>(
				new Mock<IUserStore<IdentityUser>>().Object,
				null, null, null, null, null, null, null, null);

			userMgrMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
					   .ReturnsAsync((IdentityUser?)null); // simulăm că nu găsește userul

			var signInMgrMock = new Mock<SignInManager<IdentityUser>>(
				userMgrMock.Object,
				new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
				new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
				null, null, null, null);

			var configMock = new Mock<IConfiguration>();

			var controller = new AuthController(userMgrMock.Object, signInMgrMock.Object, configMock.Object);

			// Act
			var result = await controller.Login(loginDto);

			// Assert
			var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("User not found", unauthorized.Value);
		}


		[Fact]
		public async Task Login_ReturnsUnauthorized_WhenPasswordIsInvalid()
		{
			// Arrange
			var user = new IdentityUser { UserName = "testuser" };
			var loginDto = new LoginDto("testuser", "wrongpassword");

			var userMgrMock = new Mock<UserManager<IdentityUser>>(
				new Mock<IUserStore<IdentityUser>>().Object,
				null, null, null, null, null, null, null, null);

			userMgrMock.Setup(x => x.FindByNameAsync(loginDto.Username))
					   .ReturnsAsync(user);

			var signInMgrMock = new Mock<SignInManager<IdentityUser>>(
				userMgrMock.Object,
				new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
				new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
				null, null, null, null);

			signInMgrMock.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false))
						 .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed); // parola greșită

			var configMock = new Mock<IConfiguration>();

			var controller = new AuthController(userMgrMock.Object, signInMgrMock.Object, configMock.Object);

			// Act
			var result = await controller.Login(loginDto);

			// Assert
			var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("Invalid credentials", unauthorized.Value);
		}

	}
}
