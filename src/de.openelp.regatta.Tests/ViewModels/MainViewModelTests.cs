using System.Collections.Generic;
using System.Threading.Tasks;
using de.openelp.regatta.Models;
using de.openelp.regatta.ViewModels;
using Xunit;

namespace de.openelp.regatta.Tests.ViewModels;

/// <summary>
/// Tests for <see cref="MainViewModel"/>.
/// </summary>
public class MainViewModelTests
{
    /// <summary>
    /// Tests the that pages are initialized.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public Task Test_that_pages_are_initialized()
    {
        // Arrange
        var viewModel = new MainViewModel();

        // Act
        var pages = viewModel.Pages;

        // Assert
        Assert.NotNull(pages);
        Assert.Equal(3, pages.Count);
        var pageItem = pages[0];
        Assert.Equal("Home", pageItem.Header);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Tests the that navigator is null by default.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public Task Test_that_navigator_is_null_by_default()
    {
        // Arrange
        var viewModel = new MainViewModel();

        // Act
        var navigator = viewModel.Navigator;

        // Assert
        Assert.Null(navigator);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Test_that_version_is_set()
    {
        // Arrange
        var viewModel = new MainViewModel();

        // Act
        var version = viewModel.AppVersion;

        // Assert
        Assert.NotNull(version);
        Assert.Contains("Regatta App", version);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Test_that_is_drawer_opened_is_false_by_default()
    {
        // Arrange
        var viewModel = new MainViewModel();

        // Act
        var isDrawerOpened = viewModel.IsDrawerOpened;

        // Assert
        Assert.False(isDrawerOpened);
        return Task.CompletedTask;
    }

    [Fact]
    public Task Test_that_selected_page_index_navigates_to_page()
    {
        // Arrange
        var viewModel = new MainViewModel();
        var pageItem = new PageItem("Test Page", () => "test", "M10,20 L30,20 L30,40 L10,40 Z");
        viewModel.Pages.Add(pageItem);

        // Act
        viewModel.SelectedPageIndex = 3;

        // Assert
        Assert.Equal(3, viewModel.SelectedPageIndex);
        Assert.False(viewModel.IsDrawerOpened);

        return Task.CompletedTask;
    }
}

