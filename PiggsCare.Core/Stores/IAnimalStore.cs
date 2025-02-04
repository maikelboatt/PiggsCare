using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Stores
{
    public interface IAnimalStore
    {
        IEnumerable<Animal> Animals { get; }

        Task Load();

        Task Create( Animal animal );

        Task Modify( Animal animal );

        Task Remove( int id );

        event Action OnLoad;
        event Action<Animal> OnSave;
        event Action<Animal> OnUpdate;
        event Action<int> OnDelete;
    }
}
