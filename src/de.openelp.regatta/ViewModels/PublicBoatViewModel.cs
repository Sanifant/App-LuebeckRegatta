using CommunityToolkit.Mvvm.Input;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace de.openelp.regatta.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="de.openelp.regatta.ViewModels.ViewModelBase" />
    public partial class PublicBoatViewModel : ViewModelBase
    {
        private readonly IBoatApiService _boatService;
        private readonly IRaceApiService _raceApiService;
        private readonly int _eventId;

        private RaceDto _selectedRace;
        private int _selectedRaceIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublicBoatViewModel"/> class.
        /// </summary>
        /// <param name="boatService">The boat service.</param>
        /// <param name="raceApiService">The race API service.</param>
        public PublicBoatViewModel(IBoatApiService boatService, IRaceApiService raceApiService)
        {
            _boatService = boatService;
            _raceApiService = raceApiService;
            _eventId = AppConfiguration.Current.SelectedEventId;

            LoadRaces();
        }

        /// <summary>
        /// Gets or sets the selected race.
        /// </summary>
        /// <value>
        /// The selected race.
        /// </value>
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
                    _selectedRaceIndex = Races.IndexOf(_selectedRace);
                    LoadBoats();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the boats.
        /// </summary>
        /// <value>
        /// The boats.
        /// </value>
        public ObservableCollection<BoatDto> Boats { get; set; } = new ObservableCollection<BoatDto>();

        /// <summary>
        /// Gets or sets the races.
        /// </summary>
        /// <value>
        /// The races.
        /// </value>
        public ObservableCollection<RaceDto> Races { get; set; } = new ObservableCollection<RaceDto>();


        [RelayCommand]
        public void Refresh()
        {
            LoadRaces();
        }


        [RelayCommand]
        public void NextRace()
        {
            if (_selectedRaceIndex < Races.Count - 1)
            {
                _selectedRaceIndex++;
                SelectedRace = Races[_selectedRaceIndex];
            }
        }

        [RelayCommand]
        public void PreviousRace()
        {
            if (_selectedRaceIndex > 0)
            {
                _selectedRaceIndex--;
                SelectedRace = Races[_selectedRaceIndex];
            }
        }

        private async void LoadRaces()
        {
            var races = await _raceApiService.FindAllAsync(_eventId);
            if (races != null)
            {
                this.Races = new ObservableCollection<RaceDto>(races.OrderBy(r => r.RaceStartTime));
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

    }
}
