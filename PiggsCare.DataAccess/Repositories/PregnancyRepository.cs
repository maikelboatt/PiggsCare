using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.DTO;
using PiggsCare.Domain.Models;
using PiggsCare.Domain.Repositories;
using PiggsCare.Domain.Services;

namespace PiggsCare.DataAccess.Repositories
{
    public class PregnancyRepository( ISqlDataAccess dataAccess, IDateConverterService dateConverterService ):IPregnancyRepository
    {
        public async Task<IEnumerable<PregnancyScan>> GetAllPregnancyScansAsync( int id )
        {
            IEnumerable<PregnancyScanDto> result =
                await dataAccess.QueryAsync<PregnancyScanDto, dynamic>("sp.PregnancyScan_GetAll", new { BreedingEventId = id });
            return result.Select(x => new PregnancyScan(x.ScanId, x.BreedingEventId, dateConverterService.GetDateOnly(x.ScanDate), x.ScanResults));
        }

        public async Task<PregnancyScan?> GetPregnancyScanByIdAsync( int id )
        {
            // Query the database for any record with matching id
            IEnumerable<PregnancyScanDto> result = await dataAccess.QueryAsync<PregnancyScanDto, dynamic>("sp.PregnancyScan_GetUnique", new { ScanId = id });
            PregnancyScanDto? pregnancyScanDto = result.FirstOrDefault();

            // Convert PregnancyScanDto to PregnancyScan Object
            return pregnancyScanDto is not null
                ? new PregnancyScan(pregnancyScanDto.ScanId, pregnancyScanDto.BreedingEventId, dateConverterService.GetDateOnly(pregnancyScanDto.ScanDate), pregnancyScanDto.ScanResults)
                : null;
        }

        public async Task CreatePregnancyScanAsync( PregnancyScan scan )
        {
            // Convert PregnancyScan to PregnancyScanDto
            PregnancyScanDto record = new()
            {
                ScanId = scan.ScanId,
                BreedingEventId = scan.BreedingEventId,
                ScanDate = dateConverterService.GetDateTime(scan.ScanDate),
                ScanResults = scan.ScanResults
            };

            // Insert record into the database
            await dataAccess.CommandAsync(
                "sp.PregnancyScan_Insert",
                new
                {
                    record.BreedingEventId,
                    record.ScanDate,
                    record.ScanResults
                }
            );
        }

        public async Task UpdatePregnancyScanAsync( PregnancyScan scan )
        {
            // Convert PregnancyScan to PregnancyScanDto
            PregnancyScanDto recordDto = new()
            {
                ScanId = scan.ScanId,
                BreedingEventId = scan.BreedingEventId,
                ScanDate = dateConverterService.GetDateTime(scan.ScanDate),
                ScanResults = scan.ScanResults
            };

            // Update existing record in the database
            await dataAccess.CommandAsync("sp.PregnancyScan_Modify", recordDto);
        }

        public async Task DeletePregnancyScanAsync( int id )
        {
            await dataAccess.CommandAsync("sp.PregnancyScan_Delete", new { ScanId = id });
        }
    }
}
