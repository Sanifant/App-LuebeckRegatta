using System.Collections.Generic;
using de.openelp.regatta.ViewModels;
using Xunit;

namespace de.openelp.regatta.Tests.ViewModels;

/// <summary>
/// Tests for <see cref="MainViewModel"/>.
/// </summary>
public class MainViewModelTests
{
    /// <summary>
    /// Ensures the default greeting text is initialized correctly.
    /// </summary>
    [Fact]
    public void Greeting_HasExpectedDefaultValue()
    {
        var viewModel = new MainViewModel();

        Assert.Equal("Welcome to Avalonia!", viewModel.Greeting);
    }

    /// <summary>
    /// Ensures property changed notifications are raised when greeting changes.
    /// </summary>
    [Fact]
    public void Greeting_RaisesPropertyChanged_WhenValueChanges()
    {
        var viewModel = new MainViewModel();
        var changedProperties = new List<string?>();
        viewModel.PropertyChanged += (_, args) => changedProperties.Add(args.PropertyName);

        viewModel.Greeting = "Hallo Lübeck";

        Assert.Contains(nameof(MainViewModel.Greeting), changedProperties);
    }
}

