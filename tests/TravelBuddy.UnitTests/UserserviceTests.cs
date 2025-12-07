using System.Threading.Tasks;
using Moq;
using Xunit;
using TravelBuddy.Users;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IUserRepositoryFactory> _factoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();

        _factoryMock = new Mock<IUserRepositoryFactory>();
        _factoryMock
            .Setup(f => f.GetUserRepository())
            .Returns(_userRepoMock.Object);

        _userService = new UserService(_factoryMock.Object);
    }

    [Fact]
    public async Task Authenticate_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        _userRepoMock
            .Setup(r => r.GetByEmailAsync("nouser@mail.com"))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.AuthenticateAsync("nouser@mail.com", "anypass");

        // Assert
        Assert.Null(result);
    }
}