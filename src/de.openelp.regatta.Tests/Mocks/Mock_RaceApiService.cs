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
    public class Mock_RaceApiService : IRaceApiService
    {
        List<RaceDto> _raceDtoList;

        public Mock_RaceApiService(List<RaceDto> raceDtoList)
        {
            _raceDtoList = raceDtoList;
        }


        public Task<List<RaceDto>?> FindAllAsync(int eventId, CancellationToken ct = default)
        {
            return Task.FromResult<List<RaceDto>?>(_raceDtoList);
        }
    }
}
