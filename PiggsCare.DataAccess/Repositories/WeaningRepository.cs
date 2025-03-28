using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class WeaningRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IWeaningRepository
    {
        private const string Connectionstring = @"Server=--THEBARON--\SQLEXPRESS;Database=PiggsCare;Integrated Security=True;TrustServerCertificate=True;";

        public async Task<IEnumerable<WeaningEvent>> GetAllWeaningEventAsync( int id )
        {
            IEnumerable<WeaningEventDto> result = await dataAccess.QueryAsync<WeaningEventDto, dynamic>("sp.Weaning_GetAll", new { FarrowingEventId = id }, Connectionstring);
            return result.Select(x => new WeaningEvent(x.WeaningEventId,
                                                       x.FarrowingEventId,
                                                       dateConverterService.GetDateOnly(x.WeaningDate),
                                                       x.NumberWeaned,
                                                       x.MalesWeaned,
                                                       x.FemalesWeaned,
                                                       x.AverageWeaningWeight));
        }

        public async Task<WeaningEvent?> GetWeaningEventByIdAsync( int id )
        {
            // Query the database for any record with matching id
            IEnumerable<WeaningEventDto> result = await dataAccess.QueryAsync<WeaningEventDto, dynamic>("sp.Weaning_GetUnique", new { WeaningEventId = id }, Connectionstring);
            WeaningEventDto? weaningEventDto = result.FirstOrDefault();

            // Convert WeaningEventDto to PregnancyScan Object
            return weaningEventDto is not null
                ? new WeaningEvent(weaningEventDto.WeaningEventId,
                                   weaningEventDto.FarrowingEventId,
                                   dateConverterService.GetDateOnly(weaningEventDto.WeaningDate),
                                   weaningEventDto.NumberWeaned,
                                   weaningEventDto.MalesWeaned,
                                   weaningEventDto.FemalesWeaned,
                                   weaningEventDto.AverageWeaningWeight)
                : null;
        }

        public async Task CreateWeaningEventAsync( WeaningEvent weaning )
        {
            // Convert WeaningEvent to WeaningEventDto
            WeaningEventDto record = new()
            {
                FarrowingEventId = weaning.FarrowingEventId,
                WeaningDate = dateConverterService.GetDateTime(weaning.WeaningDate),
                NumberWeaned = weaning.NumberWeaned,
                MalesWeaned = weaning.MalesWeaned,
                FemalesWeaned = weaning.FemalesWeaned,
                AverageWeaningWeight = weaning.AverageWeaningWeight
            };

            // Insert record into the database
            await dataAccess.CommandAsync("sp.Weaning_Insert",
                                          new
                                          {
                                              record.FarrowingEventId, record.WeaningDate, record.NumberWeaned, record.MalesWeaned, record.FemalesWeaned,
                                              record.AverageWeaningWeight
                                          },
                                          Connectionstring
                );
        }

        public async Task UpdateWeaningEventAsync( WeaningEvent weaning )
        {
            // Convert WeaningEvent to WeaningEventDto
            WeaningEventDto recordDto = new()
            {
                WeaningEventId = weaning.WeaningEventId,
                FarrowingEventId = weaning.FarrowingEventId,
                WeaningDate = dateConverterService.GetDateTime(weaning.WeaningDate),
                NumberWeaned = weaning.NumberWeaned,
                MalesWeaned = weaning.MalesWeaned,
                FemalesWeaned = weaning.FemalesWeaned,
                AverageWeaningWeight = weaning.AverageWeaningWeight
            };

            // Update existing record in the database
            await dataAccess.CommandAsync("sp.Weaning_Modify", recordDto, Connectionstring);
        }

        public async Task DeleteWeaningEventAsync( int id )
        {
            await dataAccess.CommandAsync("sp.Weaning_Delete", new { WeaningEventId = id }, Connectionstring);

        }
    }
}
