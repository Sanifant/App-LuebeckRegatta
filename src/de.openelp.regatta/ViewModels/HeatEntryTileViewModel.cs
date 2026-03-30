using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using de.openelp.regatta.Models;

namespace de.openelp.regatta.ViewModels;

public partial class HeatEntryTileViewModel : ViewModelBase
{
    private int _warningCount;

    public HeatEntryModel Model { get; }

    public HeatEntryTileViewModel(HeatEntryModel model)
        => Model = model ?? throw new ArgumentNullException(nameof(model));

    public int? Id => Model.Id;
    public int? Lane => Model.Lane;
    public string? ShortTeamName => Model.ShortTeamName;
    public string? TeamName => Model.TeamName;
    public string? Status => Model.Status;

    public string AthletesText =>
        (Model.Athletes?.Count ?? 0) == 0 ? "—" : string.Join(", ", Model.Athletes);

    public int WarningCount
    {
        get => _warningCount;
        set
        {
            if (_warningCount == value) return;
            _warningCount = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(WarningColor));
        }
    }

    // 0=blau, 1=gelb, >=2=rot (wie in RefereeView.java)
    public string WarningColor =>
        WarningCount <= 0 ? "#FF113355" :
        WarningCount == 1 ? "#FF665500" :
        "#FF660000";
}