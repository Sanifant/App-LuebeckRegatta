using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;

namespace de.openelp.regatta.ViewModels;

public partial class RefereeDashboardViewModel : ViewModelBase, IDisposable
{
    private readonly IRaceHeatApiClient _api;
    private readonly IAppConfiguration _configuration;

    private readonly Timer _clockTimer;

    private int _eventId;
    private RefereeModel? _selectedReferee;
    private RaceHeatModel? _selectedHeat;
    private string _noteText = "";
    private string _statusText = "";
    private string _clockText = "";

    public RefereeDashboardViewModel(IRaceHeatApiClient api, IAppConfiguration? configuration = null)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _configuration = configuration ?? AppConfiguration.Current;

        Heats = new ObservableCollection<RaceHeatModel>();
        SelectedHeatEntries = new ObservableCollection<HeatEntryTileViewModel>();

        Referees = new ObservableCollection<RefereeModel>(); // kommt bei euch vermutlich aus eigenem Endpoint/Storage

        EventId = _configuration.SelectedEventId;

        _clockTimer = new Timer(_ =>
        {
            ClockText = DateTime.Now.ToString("HH:mm:ss");
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        StatusText = "Bereit.";

        
    }

    public ObservableCollection<RefereeModel> Referees { get; }

    public ObservableCollection<RaceHeatModel> Heats { get; }

    public ObservableCollection<HeatEntryTileViewModel> SelectedHeatEntries { get; }

    [RelayCommand]
    public async Task RefreshCommand()
    {
        CancellationToken ct = default;
        await RefreshAsync(ct);
    }

    [RelayCommand]
    public async Task TakeOverCommand()
    {
        CancellationToken ct = default;
        await TakeOverAsync(ct);
    }

    [RelayCommand]
    public async Task SaveNoteCommand()
    {
        CancellationToken ct = default;
        await SaveNoteAsync(ct);
    }

    [RelayCommand]
    public async Task CreateWarningForEntry(HeatEntryTileViewModel? entry)
    {
        await Task.CompletedTask;

        if (entry == null)
        {
            StatusText = "Kein Eintrag ausgewählt.";
            return;
        }

        if (SelectedReferee == null)
        {
            StatusText = "Bitte einen Wettkampfrichter auswählen!";
            return;
        }

        // Backend-Warnungsservice/Endpoint ist in RaceHeatController nicht enthalten.
        // Daher: UI-Mock-Logik wie in RefereeView.java (farblich eskalieren).
        entry.WarningCount += 1;
        StatusText = $"Verwarnung gesetzt (Bahn {entry.Lane})";
    }

    public int EventId
    {
        get => _eventId;
        set
        {
            if (_eventId == value) return;
            _eventId = value;
            _configuration.SelectedEventId = value;
            OnPropertyChanged();
        }
    }

    public RefereeModel? SelectedReferee
    {
        get => _selectedReferee;
        set { if (_selectedReferee == value) return; _selectedReferee = value; OnPropertyChanged(); }
    }

    public RaceHeatModel? SelectedHeat
    {
        get => _selectedHeat;
        set
        {
            if (_selectedHeat == value) return;
            _selectedHeat = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedHeatTitle));
            OnPropertyChanged(nameof(SelectedHeatMeta));
            _ = LoadHeatDetailsAsync(CancellationToken.None); // fire & forget für Mock
        }
    }

    public string SelectedHeatTitle =>
        SelectedHeat == null ? "Kein Rennen ausgewählt" : $"{SelectedHeat.RaceNumber} • {SelectedHeat.Title}";

    public string SelectedHeatMeta =>
        SelectedHeat == null
            ? ""
            : $"HeatID: {SelectedHeat.HeatID} • Start: {SelectedHeat.ActualStartDate:HH:mm:ss} • Status: {SelectedHeat.Status}";

    public string NoteText
    {
        get => _noteText;
        set
        {
            if (_noteText == value) return;
            _noteText = value;
            OnPropertyChanged();

            if (SelectedHeat != null)
                SelectedHeat.Comment = value;
        }
    }

    public string StatusText
    {
        get => _statusText;
        set { if (_statusText == value) return; _statusText = value; OnPropertyChanged(); }
    }

    public string ClockText
    {
        get => _clockText;
        set { if (_clockText == value) return; _clockText = value; OnPropertyChanged(); }
    }

    public async Task InitializeAsync(int eventId, CancellationToken ct = default)
    {
        EventId = eventId;
        await RefreshAsync(ct);
    }

    private async Task RefreshAsync(CancellationToken ct)
    {
        var heats = await _api.GetHeatsAsync(EventId, ct);

        // optional: nur Running wie in RefereeView.java
        var running = heats.Where(h => string.Equals(h.Status, "Running", StringComparison.OrdinalIgnoreCase)).ToList();

        Heats.Clear();
        foreach (var h in running)
            Heats.Add(h);

        SelectedHeat ??= Heats.FirstOrDefault();
        StatusText = $"Aktualisiert: {DateTime.Now:HH:mm:ss}";
    }

    private async Task LoadHeatDetailsAsync(CancellationToken ct)
    {
        if (SelectedHeat?.HeatID == null)
        {
            SelectedHeatEntries.Clear();
            NoteText = SelectedHeat?.Comment ?? "";
            return;
        }

        // Controller: GET /{eventId}/RaceHeat/{raceId} filtert nach RACE_HEAT.ID,
        // daher verwenden wir HeatID (nicht Race.Id).
        var details = await _api.GetHeatDetailsAsync(EventId, SelectedHeat.HeatID.Value, ct);
        if (details == null) return;

        // Merge (UI arbeitet weiter mit SelectedHeat-Instanz aus Liste)
        SelectedHeat.Comment = details.Comment;
        SelectedHeat.Entries = details.Entries;

        SelectedHeatEntries.Clear();
        foreach (var e in details.Entries ?? Enumerable.Empty<HeatEntryModel>())
            SelectedHeatEntries.Add(new HeatEntryTileViewModel(e));

        NoteText = details.Comment ?? "";
    }

    private async Task TakeOverAsync(CancellationToken ct)
    {
        if (SelectedHeat?.HeatID == null)
        {
            StatusText = "Kein Rennen ausgewählt.";
            return;
        }
        if (SelectedReferee?.Id == null)
        {
            StatusText = "Bitte einen Wettkampfrichter auswählen!";
            return;
        }

        await _api.SetRefereeAsync(EventId, SelectedHeat.HeatID.Value, SelectedReferee, ct);
        StatusText = "Rennen begleiten: gesetzt.";
    }

    private Task SaveNoteAsync(CancellationToken ct)
    {
        // RaceHeatController hat aktuell PUT noch TODO. Daher nur lokal im Mock.
        StatusText = "Notiz gespeichert (lokal, Backend-PUT ist TODO).";
        return Task.CompletedTask;
    }

    [RelayCommand]
    public void NextHeat()
    {
        if (SelectedHeat == null) return;
        int currentIndex = Heats.IndexOf(SelectedHeat);
        if (currentIndex < Heats.Count - 1)
        {
            SelectedHeat = Heats[currentIndex + 1];
        }
    }

    [RelayCommand]
    public void PrevHeat()
    {
        if (SelectedHeat == null) return;
        int currentIndex = Heats.IndexOf(SelectedHeat);
        if (currentIndex > 0)
        {
            SelectedHeat = Heats[currentIndex - 1];
        }
    }

    public void Dispose()
    {
        _clockTimer.Dispose();
    }
}