using System;
using System.Threading.Tasks;
using de.openelp.regatta.ViewModels;
using Xunit;

namespace de.openelp.regatta.Tests.ViewModels;

/// <summary>
/// Tests for <see cref="HomeViewModel"/>.
/// </summary>
public class HomeViewModelTests
{
    public Task Test_that_welcome_message_is_set()
    {
        // Arrange
        var viewModel = new HomeViewModel();

        // Act
        var welcomeMessage = viewModel.WelcomeMessage;

        // Assert
        Assert.NotNull(welcomeMessage);
        Assert.Contains("Lübeck Regatta", welcomeMessage);
        Assert.Contains(DateTime.Now.Year.ToString(), welcomeMessage);
        return Task.CompletedTask;
    }
}