using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace de.openelp.regatta.ViewModels
{
    public class PublicRaceViewModel : ViewModelBase, IDisposable
    {
        private readonly IRaceApiService _raceApiService;
        private int _eventId;

        public PublicRaceViewModel(IRaceApiService raceApiService)
        {
            _raceApiService = raceApiService;
            _eventId = AppConfiguration.Current.SelectedEventId;
            LoadRaces();
        }

        public ObservableCollection<RaceDto> Races { get; set; } = new ObservableCollection<RaceDto>();


        private async void LoadRaces()
        {
            var races = await _raceApiService.FindAllAsync(_eventId);
            if (races != null)
            {
                this.Races = new ObservableCollection<RaceDto>(races.OrderBy(r => r.RaceStartTime));
                OnPropertyChanged(nameof(Races));
            }
        }


        public void Dispose()
        {

        }
    }
}
