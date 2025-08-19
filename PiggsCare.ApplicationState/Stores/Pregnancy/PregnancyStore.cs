using Microsoft.Extensions.Logging;
using PiggsCare.Domain.Models;
using PiggsCare.Infrastructure.Helpers;

namespace PiggsCare.ApplicationState.Stores.Pregnancy
{
    public class PregnancyStore( ILogger<PregnancyStore> logger ):IPregnancyStore
    {
        private readonly ReaderWriterLockSlim _lock = new();
        private readonly ILogger<PregnancyStore> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly List<PregnancyScan> _pregnancies = [];

        public IEnumerable<PregnancyScan> Pregnancies
        {
            get
            {
                using (_lock.Read())
                {
                    return _pregnancies.ToList();
                }
            }
        }

        public void LoadPregnancy( IEnumerable<PregnancyScan> pregnancies, CancellationToken cancellationToken = default )
        {
            try
            {
                using (_lock.Write())
                {
                    _pregnancies.Clear();
                    _pregnancies.AddRange(pregnancies);
                }

                OnPregnanciesLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error loading pregnancies: {ExMessage}", ex.Message);
                throw;
            }
        }

        public PregnancyScan? GetPregnancyById( int pregnancyId )
        {
            using (_lock.Read())
            {
                return _pregnancies.FirstOrDefault(p => p.ScanId == pregnancyId);
            }
        }

        public PregnancyScan CreatePregnancyScanWithCorrectId( int scanId, PregnancyScan pregnancyScan ) => new(
            scanId,
            pregnancyScan.BreedingEventId,
            pregnancyScan.ScanDate,
            pregnancyScan.ScanResults);

        public void AddPregnancy( PregnancyScan pregnancy )
        {
            using (_lock.Write())
            {
                _pregnancies.Add(pregnancy);
                OnPregnancyAdded?.Invoke(pregnancy);
            }
        }

        public void UpdatePregnancy( PregnancyScan pregnancy )
        {
            using (_lock.Write())
            {
                int index = _pregnancies.FindIndex(p => p.ScanId == pregnancy.ScanId);
                if (index != -1)
                {
                    _pregnancies[index] = pregnancy;
                    OnPregnancyUpdated?.Invoke(pregnancy);
                }
            }
        }

        public void DeletePregnancy( int pregnancyId )
        {
            using (_lock.Write())
            {
                PregnancyScan? pregnancy = GetPregnancyById(pregnancyId);
                if (pregnancy == null) return;
                _pregnancies.Remove(pregnancy);
                OnPregnancyDeleted?.Invoke(pregnancy);
            }
        }

        public event Action? OnPregnanciesLoaded;
        public event Action<PregnancyScan>? OnPregnancyAdded;
        public event Action<PregnancyScan>? OnPregnancyUpdated;
        public event Action<PregnancyScan>? OnPregnancyDeleted;
    }
}
