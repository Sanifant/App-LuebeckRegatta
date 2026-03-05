namespace LuebeckRegatta.App.Services;

/// <summary>
/// Service implementation for managing application settings using .NET MAUI Preferences
/// </summary>
public class SettingsService : ISettingsService
{
    private const string NewsFeedUrlKey = "news_feed_url";
    private const string RegattaApiUrlKey = "regatta_api_url";
    private const string RegattaUsernameKey = "regatta_username";
    private const string RegattaPasswordKey = "regatta_password";
    private const string SelectedEventIdKey = "selected_event_id";
    
    private const string DefaultNewsFeedUrl = "https://www.rudern.de/news.xml";
    private const string DefaultRegattaApiUrl = "https://regatta-test.grinch-tech.de/api";

    public string NewsFeedUrl
    {
        get => Preferences.Get(NewsFeedUrlKey, DefaultNewsFeedUrl);
        set => Preferences.Set(NewsFeedUrlKey, value);
    }

    public string RegattaApiUrl
    {
        get => Preferences.Get(RegattaApiUrlKey, DefaultRegattaApiUrl);
        set => Preferences.Set(RegattaApiUrlKey, value);
    }

    public string RegattaUsername
    {
        get => Preferences.Get(RegattaUsernameKey, string.Empty);
        set => Preferences.Set(RegattaUsernameKey, value);
    }

    public string RegattaPassword
    {
        get => Preferences.Get(RegattaPasswordKey, string.Empty);
        set => Preferences.Set(RegattaPasswordKey, value);
    }

    public int? SelectedEventId
    {
        get
        {
            var value = Preferences.Get(SelectedEventIdKey, -1);
            return value == -1 ? null : value;
        }
        set => Preferences.Set(SelectedEventIdKey, value ?? -1);
    }

    public void ResetToDefaults()
    {
        NewsFeedUrl = DefaultNewsFeedUrl;
        RegattaApiUrl = DefaultRegattaApiUrl;
        RegattaUsername = string.Empty;
        RegattaPassword = string.Empty;
        SelectedEventId = null;
    }
}
