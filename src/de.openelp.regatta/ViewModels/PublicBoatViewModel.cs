using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace de.openelp.regatta.ViewModels
{
    public class PublicBoatViewModel : ViewModelBase
    {
        private readonly IBoatApiService _boatService;
        private readonly IRaceApiService _raceApiService;
        private readonly int _eventId;

        private RaceDto _selectedRace;

        public PublicBoatViewModel(IBoatApiService boatService, IRaceApiService raceApiService)
        {
            _boatService = boatService;
            _raceApiService = raceApiService;
            _eventId = AppConfiguration.Current.SelectedEventId;

            LoadRaces();
        }

        private async void LoadRaces()
        {
            var races = await _raceApiService.FindAllAsync(_eventId);
            if (races != null)
            {
                this.Races = new ObservableCollection<RaceDto>(races);
                OnPropertyChanged(nameof(Races));
            }
        }

        private void LoadBoats()
        {
            _boatService.GetAllBoatsPerRaceAsync(_eventId, _selectedRace.RaceId)
                .ContinueWith(task =>
                {
                    if (task.Result != null)
                    {
                        this.Boats = new ObservableCollection<BoatDto>(task.Result);
                        OnPropertyChanged(nameof(Boats));
                    }
                });
        }

        public RaceDto SelectedRace
        {
            get
            {
                return _selectedRace;
            }
            set
            {
                if (_selectedRace != value)
                {
                    _selectedRace = value;
                    LoadBoats();
                    OnPropertyChanged();
                }
            }
        }


        public ObservableCollection<BoatDto> Boats { get; set; } = new ObservableCollection<BoatDto>();

        public ObservableCollection<RaceDto> Races { get; set; } = new ObservableCollection<RaceDto>();
    }
}
