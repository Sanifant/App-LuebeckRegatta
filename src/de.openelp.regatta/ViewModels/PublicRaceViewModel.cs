using CommunityToolkit.Mvvm.Input;
using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace de.openelp.regatta.ViewModels
{
    public partial class PublicRaceViewModel : ViewModelBase, IDisposable
    {
        private IRaceApiService raceApiService;
        private int _eventId;

        public PublicRaceViewModel(IRaceApiService raceApiService)
        {
            this.raceApiService = raceApiService;
            _eventId = AppConfiguration.Current.SelectedEventId;
            LoadRaces();
        }

        public ObservableCollection<RaceDto> Races { get; set; }


        private async void LoadRaces()
        {
            var races = await raceApiService.FindAllAsync(_eventId);
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
