using de.openelp.regatta.Interfaces;
using de.openelp.regatta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace de.openelp.regatta.Tests.Mocks
{
    internal class Mock_BoatApiService : IBoatApiService
    {
        List<BoatDto> _boats;

        public Mock_BoatApiService(List<BoatDto> boats)
        {
            _boats = boats;
        }

        public Task<List<BoatDto>?> GetAllBoatsAsync(int eventId, CancellationToken ct = default)
        {
            return Task.FromResult<List<BoatDto>?>(_boats);
        }

        public Task<List<BoatDto>?> GetAllBoatsPerRaceAsync(int eventId, int raceId, CancellationToken ct = default)
        {
            return Task.FromResult<List<BoatDto>?>(_boats);
        }
    }
}
