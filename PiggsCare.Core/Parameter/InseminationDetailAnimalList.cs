// This is model that represents a list of insemination events for an animal.

using PiggsCare.Domain.Models;

namespace PiggsCare.Core.Parameter
{
    public class InseminationDetailAnimalList( List<InseminationEvent> insemination )
    {
        public List<InseminationEvent> Inseminations { get; init; } = insemination;
    }
}
