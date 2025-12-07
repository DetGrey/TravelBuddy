using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TravelBuddy.Users;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Users.Repositories;

namespace TravelBuddy.Tests;
    public class UserServiceTests
    {
       private readonly Mock<IUserRepository> _mockRepo;
       private readonly UserService _userService;

       public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepo(_mockRepo.Object));
    }

    [Fact]
    public async Task Authenticate_ShouldReturnNull_WhenUserNotFound()
    {
        _mockRepo.Setup(r => r.GetUserByEmailASync("nouser@mail.com")).ReturnsAsync((User)null);

        var result = await _userService.AuthenticateAsync("nouser@mail.com", "anypass");

        Assert.Null(result);
    }
    }
