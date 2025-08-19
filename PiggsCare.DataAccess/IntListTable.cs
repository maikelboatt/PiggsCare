using PiggsCare.DataAccess.DTO;
using System.Collections;

namespace PiggsCare.DataAccess
{
    public class IntListTable:IEnumerable<IntListDto>
    {
        private readonly IEnumerable<int> _ids;

        public IntListTable( IEnumerable<int> ids )
        {
            _ids = ids;
        }

        public IEnumerator<IntListDto> GetEnumerator()
        {
            return _ids.Select(id => new IntListDto { AnimalId = id }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
