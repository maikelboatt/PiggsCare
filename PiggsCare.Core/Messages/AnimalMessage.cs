using MvvmCross.Plugin.Messenger;
using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Messages
{
    public class AnimalMessage( object sender, IEnumerable<Animal> animals ):MvxMessage(sender)
    {
        public IEnumerable<Animal> Animals => animals;
    }
}
