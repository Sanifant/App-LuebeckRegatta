namespace LuebeckRegatta.App.Messages;

/// <summary>
/// Message that is sent when settings have changed
/// </summary>
public class SettingsChangedMessage
{
    public bool EventChanged { get; set; }
}
