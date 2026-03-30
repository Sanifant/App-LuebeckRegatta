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
}