using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using de.openelp.regatta.Tests.Mocks;
using de.openelp.regatta.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace de.openelp.regatta.Tests.ViewModels
{
    public class PublicBoatViewModelTest
    {
        private IBoatApiService boatService;
        private IRaceApiService raceApiService;

        [Fact]
        public async Task TestThat_Races_AreLoaded()
        {
            // Arrange
            var boats = new List<BoatDto>
            {
                new BoatDto { Number = 1, TeamShort= "LRG", Team = "Lübecker Regatta Gesellschaft", Athletes = "Maya, Willi" },
                new BoatDto { Number = 2, TeamShort= "BRG", Team = "Berliner Regatta Gesellschaft", Athletes = "Anna, John" },
                new BoatDto { Number = 3, TeamShort= "MRG", Team = "Münchner Regatta Gesellschaft", Athletes = "Lisa, Tom" },
                new BoatDto { Number = 4, TeamShort= "HRG", Team = "Hamburger Regatta Gesellschaft", Athletes = "Sophie, Max" }
            };
            boatService = new Mock_BoatApiService(boats);
            var races = new List<RaceDto>
            {
                new RaceDto { RaceNumber = "1", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) },
                new RaceDto { RaceNumber = "2", RaceTitle = "200m Sprint", RaceStartTime = DateTime.Now.AddHours(2) },
                new RaceDto { RaceNumber = "3", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) }
            };
            raceApiService = new Mock_RaceApiService(races);

            var viewModel = new PublicBoatViewModel(boatService, raceApiService);

            // Act
            ObservableCollection<RaceDto> result = viewModel.Races;

            // Assert
            Assert.Equal(races.Count, result.Count);
        }

        [Fact]
        public async Task TestThat_Boats_AreLoaded()
        {
            // Arrange
            var boats = new List<BoatDto>
            {
                new BoatDto { Number = 1, TeamShort= "LRG", Team = "Lübecker Regatta Gesellschaft", Athletes = "Maya, Willi" },
                new BoatDto { Number = 2, TeamShort= "BRG", Team = "Berliner Regatta Gesellschaft", Athletes = "Anna, John" },
                new BoatDto { Number = 3, TeamShort= "MRG", Team = "Münchner Regatta Gesellschaft", Athletes = "Lisa, Tom" },
                new BoatDto { Number = 4, TeamShort= "HRG", Team = "Hamburger Regatta Gesellschaft", Athletes = "Sophie, Max" }
            };
            boatService = new Mock_BoatApiService(boats);
            var races = new List<RaceDto>
            {
                new RaceDto { RaceNumber = "1", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) },
                new RaceDto { RaceNumber = "2", RaceTitle = "200m Sprint", RaceStartTime = DateTime.Now.AddHours(2) },
                new RaceDto { RaceNumber = "3", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) }
            };
            raceApiService = new Mock_RaceApiService(races);

            var viewModel = new PublicBoatViewModel(boatService, raceApiService);


            // Act
            ObservableCollection<BoatDto> result = viewModel.Boats;
            Assert.Equal(0, result.Count);
            // Assert
        }

        [Fact]
        public async Task TestThat_Boats_For_SelectedRace_IsLoaded()
        {
            // Arrange
            bool propertyCalled = false;
            var raceToSelect = new RaceDto { RaceId = 4, RaceNumber = "4", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) };
            var boats = new List<BoatDto>
            {
                new BoatDto { Number = 1, TeamShort= "LRG", Team = "Lübecker Regatta Gesellschaft", Athletes = "Maya, Willi" },
                new BoatDto { Number = 2, TeamShort= "BRG", Team = "Berliner Regatta Gesellschaft", Athletes = "Anna, John" },
                new BoatDto { Number = 3, TeamShort= "MRG", Team = "Münchner Regatta Gesellschaft", Athletes = "Lisa, Tom" },
                new BoatDto { Number = 4, TeamShort= "HRG", Team = "Hamburger Regatta Gesellschaft", Athletes = "Sophie, Max" }
            };
            var races = new List<RaceDto>
            {
                new RaceDto { RaceId = 1, RaceNumber = "1", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) },
                new RaceDto { RaceId = 2, RaceNumber = "2", RaceTitle = "200m Sprint", RaceStartTime = DateTime.Now.AddHours(2) },
                new RaceDto { RaceId = 3, RaceNumber = "3", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) }
            };
            races.Add(raceToSelect);

            boatService = new Mock_BoatApiService(boats);
            raceApiService = new Mock_RaceApiService(races);

            var viewModel = new PublicBoatViewModel(boatService, raceApiService);

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PublicBoatViewModel.SelectedRace))
                {
                    // Assert
                    Assert.NotNull(viewModel.SelectedRace);
                    Assert.Equal(raceToSelect.RaceId, viewModel.SelectedRace.RaceId);
                    Assert.Equal(raceToSelect.RaceNumber, viewModel.SelectedRace.RaceNumber);
                    propertyCalled = true;
                }

            };

            viewModel.SelectedRace = raceToSelect;

            Assert.True(propertyCalled);
        }

        [Fact]
        public async Task TestThat_NextCommand_Selectes_Next_Race()
        {
            bool propertyCalled = false;
            var raceToSelect = new RaceDto { RaceId = 4, RaceNumber = "4", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now };
            var boats = new List<BoatDto>
            {
                new BoatDto { Number = 1, TeamShort= "LRG", Team = "Lübecker Regatta Gesellschaft", Athletes = "Maya, Willi" },
                new BoatDto { Number = 2, TeamShort= "BRG", Team = "Berliner Regatta Gesellschaft", Athletes = "Anna, John" },
                new BoatDto { Number = 3, TeamShort= "MRG", Team = "Münchner Regatta Gesellschaft", Athletes = "Lisa, Tom" },
                new BoatDto { Number = 4, TeamShort= "HRG", Team = "Hamburger Regatta Gesellschaft", Athletes = "Sophie, Max" }
            };
            var races = new List<RaceDto>
            {
                new RaceDto { RaceId = 1, RaceNumber = "1", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) },
                new RaceDto { RaceId = 2, RaceNumber = "2", RaceTitle = "200m Sprint", RaceStartTime = DateTime.Now.AddHours(2) },
                new RaceDto { RaceId = 3, RaceNumber = "3", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(3) }
            };
            races.Add(raceToSelect);

            boatService = new Mock_BoatApiService(boats);
            raceApiService = new Mock_RaceApiService(races);

            var viewModel = new PublicBoatViewModel(boatService, raceApiService);
            viewModel.SelectedRace = raceToSelect;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PublicBoatViewModel.SelectedRace))
                {
                    // Assert
                    Assert.NotNull(viewModel.SelectedRace);
                    Assert.Equal(1, viewModel.SelectedRace.RaceId);
                    Assert.Equal("1", viewModel.SelectedRace.RaceNumber);
                    propertyCalled = true;
                }

            };

            viewModel.NextRace();

            Assert.True(propertyCalled);
        }

        [Fact]
        public async Task TestThat_NextCommand_Throws_NoError_When_Reached_End()
        {
            // Arrange
            bool propertyCalled = true;
            var raceToSelect = new RaceDto { RaceId = 4, RaceNumber = "4", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(4) };
            var boats = new List<BoatDto>
            {
                new BoatDto { Number = 1, TeamShort= "LRG", Team = "Lübecker Regatta Gesellschaft", Athletes = "Maya, Willi" },
                new BoatDto { Number = 2, TeamShort= "BRG", Team = "Berliner Regatta Gesellschaft", Athletes = "Anna, John" },
                new BoatDto { Number = 3, TeamShort= "MRG", Team = "Münchner Regatta Gesellschaft", Athletes = "Lisa, Tom" },
                new BoatDto { Number = 4, TeamShort= "HRG", Team = "Hamburger Regatta Gesellschaft", Athletes = "Sophie, Max" }
            };
            var races = new List<RaceDto>
            {
                new RaceDto { RaceId = 1, RaceNumber = "1", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) },
                new RaceDto { RaceId = 2, RaceNumber = "2", RaceTitle = "200m Sprint", RaceStartTime = DateTime.Now.AddHours(2) },
                new RaceDto { RaceId = 3, RaceNumber = "3", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(3) }
            };
            races.Add(raceToSelect);

            boatService = new Mock_BoatApiService(boats);
            raceApiService = new Mock_RaceApiService(races.OrderBy(r => r.RaceId).ToList());

            var viewModel = new PublicBoatViewModel(boatService, raceApiService);
            viewModel.SelectedRace = raceToSelect;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PublicBoatViewModel.SelectedRace))
                {
                    propertyCalled = false;
                }

            };

            viewModel.NextRace();

            Assert.True(propertyCalled);
        }

        [Fact]
        public async Task TestThat_PrevCommand_Selectes_Next_Race()
        {
            // Arrange
            bool propertyCalled = false;
            var raceToSelect = new RaceDto { RaceId = 4, RaceNumber = "4", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(4) };
            var boats = new List<BoatDto>
            {
                new BoatDto { Number = 1, TeamShort= "LRG", Team = "Lübecker Regatta Gesellschaft", Athletes = "Maya, Willi" },
                new BoatDto { Number = 2, TeamShort= "BRG", Team = "Berliner Regatta Gesellschaft", Athletes = "Anna, John" },
                new BoatDto { Number = 3, TeamShort= "MRG", Team = "Münchner Regatta Gesellschaft", Athletes = "Lisa, Tom" },
                new BoatDto { Number = 4, TeamShort= "HRG", Team = "Hamburger Regatta Gesellschaft", Athletes = "Sophie, Max" }
            };
            var races = new List<RaceDto>
            {
                new RaceDto { RaceId = 1, RaceNumber = "1", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) },
                new RaceDto { RaceId = 2, RaceNumber = "2", RaceTitle = "200m Sprint", RaceStartTime = DateTime.Now.AddHours(2) },
                new RaceDto { RaceId = 3, RaceNumber = "3", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(3) }
            };
            races.Add(raceToSelect);

            boatService = new Mock_BoatApiService(boats);
            raceApiService = new Mock_RaceApiService(races);

            var viewModel = new PublicBoatViewModel(boatService, raceApiService);
            viewModel.SelectedRace = raceToSelect;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PublicBoatViewModel.SelectedRace))
                {
                    // Assert
                    Assert.NotNull(viewModel.SelectedRace);
                    Assert.Equal(3, viewModel.SelectedRace.RaceId);
                    Assert.Equal("3", viewModel.SelectedRace.RaceNumber);
                    propertyCalled = true;
                }

            };

            viewModel.PreviousRace();

            Assert.True(propertyCalled);
        }

        [Fact]
        public async Task TestThat_PrevCommand_Throws_NoError_When_Reached_End()
        {
            // Arrange
            bool propertyCalled = true;
            var raceToSelect = new RaceDto { RaceId = 4, RaceNumber = "4", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now };
            var boats = new List<BoatDto>
            {
                new BoatDto { Number = 1, TeamShort= "LRG", Team = "Lübecker Regatta Gesellschaft", Athletes = "Maya, Willi" },
                new BoatDto { Number = 2, TeamShort= "BRG", Team = "Berliner Regatta Gesellschaft", Athletes = "Anna, John" },
                new BoatDto { Number = 3, TeamShort= "MRG", Team = "Münchner Regatta Gesellschaft", Athletes = "Lisa, Tom" },
                new BoatDto { Number = 4, TeamShort= "HRG", Team = "Hamburger Regatta Gesellschaft", Athletes = "Sophie, Max" }
            };
            var races = new List<RaceDto>
            {
                new RaceDto { RaceId = 1, RaceNumber = "1", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(1) },
                new RaceDto { RaceId = 2, RaceNumber = "2", RaceTitle = "200m Sprint", RaceStartTime = DateTime.Now.AddHours(2) },
                new RaceDto { RaceId = 3, RaceNumber = "3", RaceTitle = "100m Sprint", RaceStartTime = DateTime.Now.AddHours(3) }
            };
            races.Add(raceToSelect);

            boatService = new Mock_BoatApiService(boats);
            raceApiService = new Mock_RaceApiService(races);

            var viewModel = new PublicBoatViewModel(boatService, raceApiService);
            viewModel.SelectedRace = raceToSelect;

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PublicBoatViewModel.SelectedRace))
                {
                    propertyCalled = false;
                }

            };

            viewModel.PreviousRace();

            Assert.True(propertyCalled);
        }
    }
}
