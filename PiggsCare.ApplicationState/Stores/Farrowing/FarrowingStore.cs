using Microsoft.Extensions.Logging;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Helpers;

namespace PiggsCare.ApplicationState.Stores.Farrowing
{
    public class FarrowingStore( ILogger<FarrowingStore> logger ):IFarrowingStore
    {
        private readonly List<FarrowEvent> _farrowings = [];
        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<FarrowingStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public IEnumerable<FarrowEvent> Farrowings
        {
            get
            {
                using (_lock.Read())
                {
                    return _farrowings.ToList();
                }

            }
        }

        public void LoadFarrowingEvent( IEnumerable<FarrowEvent> farrowings, CancellationToken cancellationToken = default )
        {
            try
            {
                using (_lock.Write())
                {
                    _farrowings.Clear();
                    _farrowings.AddRange(farrowings);
                }

                OnFarrowingsLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading farrowings: {ExMessage}", ex.Message);
                throw;
            }
        }

        public FarrowEvent? GetFarrowingByAnimalId( int farrowEventId ) => _farrowings.FirstOrDefault(f => f.FarrowingEventId == farrowEventId);

        public FarrowEvent? GetFarrowingById( int farrowingId )
        {
            using (_lock.Read())
            {
                return _farrowings.FirstOrDefault(f => f.FarrowingEventId == farrowingId);
            }

        }


        public void AddFarrowing( FarrowEvent farrowing )
        {
            using (_lock.Write())
            {
                _farrowings.Add(farrowing);
                OnFarrowingAdded?.Invoke(farrowing);
            }
        }

        public void UpdateFarrowing( FarrowEvent farrowing )
        {
            using (_lock.Write())
            {
                int index = _farrowings.FindIndex(f => f.FarrowingEventId == farrowing.FarrowingEventId);
                if (index != -1)
                {
                    _farrowings[index] = farrowing;
                }
            }

            OnFarrowingUpdated?.Invoke(farrowing);
        }

        public void DeleteFarrowing( int farrowingId )
        {
            FarrowEvent? farrowing = null;
            using (_lock.Write())
            {
                farrowing = GetFarrowingById(farrowingId);
                if (farrowing != null)
                {
                    _farrowings.Remove(farrowing);
                }
            }
            if (farrowing != null) OnFarrowingDeleted?.Invoke(farrowing);
        }

        public event Action? OnFarrowingsLoaded;
        public event Action<FarrowEvent>? OnFarrowingAdded;
        public event Action<FarrowEvent>? OnFarrowingUpdated;
        public event Action<FarrowEvent>? OnFarrowingDeleted;

        public FarrowEvent CreateFarrowingWithCorrectId( int farrowEventId, FarrowEvent farrowing ) => new(
            farrowEventId,
            farrowing.BreedingEventId,
            farrowing.FarrowDate,
            farrowing.LitterSize,
            farrowing.BornAlive,
            farrowing.BordDead,
            farrowing.Mummified);
    }
}
