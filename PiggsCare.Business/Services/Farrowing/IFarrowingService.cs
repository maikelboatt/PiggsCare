using PiggsCare.Domain.Models;

namespace PiggsCare.Business.Services.Farrowing
{
    public interface IFarrowingService
    {
        Task GetAllFarrowingsAsync( int id );

        FarrowEvent? GetFarrowingById( int id );

        Task CreateFarrowingAsync( FarrowEvent farrowing );

        Task UpdateFarrowingAsync( FarrowEvent farrowing );

        Task DeleteFarrowingAsync( int farrowingId );
    }
}
