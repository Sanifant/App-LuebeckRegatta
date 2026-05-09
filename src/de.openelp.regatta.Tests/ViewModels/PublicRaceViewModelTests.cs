using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Tests.Mocks;
using de.openelp.regatta.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace de.openelp.regatta.Tests.ViewModels
{
    public class PublicRaceViewModelTests
    {
        private IRaceApiService _raceApiService = null!;

        [Fact]
        public void TestThat_Races_AreLoaded()
        {
            // Arrange
            var races = new List<RaceDto>
            {
                new RaceDto { RaceNumber = "1", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) },
                new RaceDto { RaceNumber = "2", RaceTitle = "200m Sprint", RaceStartTime = DateTime.Now.AddHours(2) },
                new RaceDto { RaceNumber = "3", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) }
            };
            _raceApiService = new Mock_RaceApiService(races);

            var viewModel = new PublicRaceViewModel(_raceApiService);

            // Act
            ObservableCollection<RaceDto> result = viewModel.Races;

            // Assert
            Assert.Equal(races.Count, result.Count);
        }
    }
}
