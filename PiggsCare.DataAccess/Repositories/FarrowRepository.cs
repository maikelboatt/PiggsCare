using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class FarrowRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IFarrowRepository
    {
        public async Task<IEnumerable<FarrowEvent>> GetAllFarrowEventAsync( int id )
        {
            IEnumerable<FarrowingEventDto> result = await dataAccess.QueryAsync<FarrowingEventDto, dynamic>("sp.Farrowing_GetAll", new { BreedingEventId = id });
            return result.Select(x => new FarrowEvent(
                                     x.FarrowingEventId,
                                     x.BreedingEventId,
                                     dateConverterService.GetDateOnly(x.FarrowDate),
                                     x.LitterSize,
                                     x.BornAlive,
                                     x.BornDead,
                                     x.Mummified));
        }

        public async Task<FarrowEvent?> GetFarrowEventByIdAsync( int id )
        {
            // Query the database for any record with matching id
            IEnumerable<FarrowingEventDto> result = await dataAccess.QueryAsync<FarrowingEventDto, dynamic>("sp.Farrowing_GetUnique", new { FarrowingEventId = id });
            FarrowingEventDto? farrowingEventDto = result.FirstOrDefault();

            // Convert FarrowingEventDto to PregnancyScan Object
            return farrowingEventDto is not null
                ? new FarrowEvent(
                    farrowingEventDto.FarrowingEventId,
                    farrowingEventDto.BreedingEventId,
                    dateConverterService.GetDateOnly(farrowingEventDto.FarrowDate),
                    farrowingEventDto.LitterSize,
                    farrowingEventDto.BornAlive,
                    farrowingEventDto.BornDead,
                    farrowingEventDto.Mummified)
                : null;
        }

        public async Task CreateFarrowEventAsync( FarrowEvent farrow )
        {
            // Convert FarrowingEvent to FarrowingEventDto
            FarrowingEventDto record = new()
            {
                BreedingEventId = farrow.BreedingEventId,
                FarrowDate = dateConverterService.GetDateTime(farrow.FarrowDate),
                LitterSize = farrow.LitterSize,
                BornAlive = farrow.BornAlive,
                BornDead = farrow.BordDead,
                Mummified = farrow.Mummified
            };

            // Insert record into the database
            await dataAccess.CommandAsync(
                "sp.Farrowing_Insert",
                new
                {
                    record.BreedingEventId,
                    record.FarrowDate,
                    record.LitterSize,
                    record.BornAlive,
                    record.BornDead,
                    record.Mummified
                });
        }

        public async Task UpdateFarrowEventAsync( FarrowEvent farrow )
        {
            // Convert FarrowingEvent to FarrowingEventDto
            FarrowingEventDto recordDto = new()
            {
                FarrowingEventId = farrow.FarrowingEventId,
                BreedingEventId = farrow.BreedingEventId,
                FarrowDate = dateConverterService.GetDateTime(farrow.FarrowDate),
                LitterSize = farrow.LitterSize,
                BornAlive = farrow.BornAlive,
                BornDead = farrow.BordDead,
                Mummified = farrow.Mummified
            };

            // Update existing record in the database
            await dataAccess.CommandAsync("sp.Farrowing_Modify", recordDto);
        }

        public async Task DeleteFarrowEventAsync( int id )
        {
            await dataAccess.CommandAsync("sp.Farrowing_Delete", new { FarrowingEventId = id });
        }
    }
}
