using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Services;

namespace de.openelp.regatta.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
	private readonly IAppConfiguration _configuration;

	public SettingsViewModel(IAppConfiguration? configuration = null)
	{
		_configuration = configuration ?? AppConfiguration.Current;
	}

	public string WebApiBaseUrl
	{
		get => _configuration.WebApiBaseUrl;
		set
		{
			if (_configuration.WebApiBaseUrl == value)
			{
				return;
			}

			_configuration.WebApiBaseUrl = value;
			OnPropertyChanged();
		}
	}

	public int SelectedEventId
	{
		get => _configuration.SelectedEventId;
		set
		{
			if (_configuration.SelectedEventId == value)
			{
				return;
			}

			_configuration.SelectedEventId = value;
			OnPropertyChanged();
		}
	}

	public string SelectedTheme
	{
		get => _configuration.AppTheme;
		set
		{
			if (_configuration.AppTheme == value)
			{
				return;
			}

			_configuration.AppTheme = value;
			OnPropertyChanged();
		}
	}

	public string UserName
	{
		get => _configuration.UserName;
		set
		{
			if (_configuration.UserName == value)
			{
				return;
			}

			_configuration.UserName = value;
			OnPropertyChanged();
		}
	}
	
	public string Password
	{
		get => _configuration.Password;
		set
		{
			if (_configuration.Password == value)
			{
				return;
			}

			_configuration.Password = value;
			OnPropertyChanged();
		}
	}

	public bool IsDebugMode
	{
		get => _configuration.IsDebugMode;
		set
		{
			if (_configuration.IsDebugMode == value)
			{
				return;
			}

			_configuration.IsDebugMode = value;
			OnPropertyChanged();
		}
	}
}