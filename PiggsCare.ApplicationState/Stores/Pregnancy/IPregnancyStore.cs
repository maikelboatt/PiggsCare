using PiggsCare.Domain.Models;

namespace PiggsCare.ApplicationState.Stores.Pregnancy
{
    public interface IPregnancyStore
    {
        IEnumerable<PregnancyScan> Pregnancies { get; }

        void LoadPregnancy( IEnumerable<PregnancyScan> pregnancies, CancellationToken cancellationToken = default );

        PregnancyScan? GetPregnancyById( int pregnancyId );

        PregnancyScan CreatePregnancyScanWithCorrectId( int scanId, PregnancyScan pregnancyScan );

        void AddPregnancy( PregnancyScan pregnancy );

        void UpdatePregnancy( PregnancyScan pregnancy );

        void DeletePregnancy( int pregnancyId );

        event Action? OnPregnanciesLoaded;
        event Action<PregnancyScan>? OnPregnancyAdded;
        event Action<PregnancyScan>? OnPregnancyUpdated;
        event Action<PregnancyScan>? OnPregnancyDeleted;
    }
}
