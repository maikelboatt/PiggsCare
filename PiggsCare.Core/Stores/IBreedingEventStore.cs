using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Stores
{
    public interface IBreedingEventStore
    {
        IEnumerable<BreedingEvent> BreedingEvents { get; }

        Task Load( int id );

        Task Create( BreedingEvent breedingEvent );

        Task Modify( BreedingEvent breedingEvent );

        Task Remove( int id );

        event Action OnLoad;
        event Action<BreedingEvent> OnSave;
        event Action<BreedingEvent> OnUpdate;
        event Action<int> OnDelete;
    }
}
