using PiggsCare.Domain.Models;
using PiggsCare.Domain.Services;

namespace PiggsCare.Core.Stores
{
    public class PregnancyStore:IPregnancyStore
    {
        #region Constructor

        public PregnancyStore( IPregnancyService pregnancyService )
        {
            _pregnancyService = pregnancyService;
        }

        #endregion

        #region Properties

        public IEnumerable<PregnancyScan> PregnancyScans => _pregnancyScans;

        #endregion

        #region CRUD Operations

        public async Task Create( PregnancyScan scan )
        {
            await _pregnancyService.CreatePregnancyScanAsync(scan);
            _pregnancyScans.Add(scan);
            OnSave?.Invoke(scan);
        }

        public async Task Modify( PregnancyScan scan )
        {
            await _pregnancyService.UpdatePregnancyScanAsync(scan);
            int currentIndex = _pregnancyScans.FindIndex(x => x.ScanId == scan.ScanId);

            if (currentIndex != -1)
            {
                _pregnancyScans[currentIndex] = scan;
            }
            else
            {
                _pregnancyScans.Add(scan);
            }
            OnUpdate?.Invoke(scan);
        }

        public async Task Remove( int id )
        {
            await _pregnancyService.DeleteHealthRecordAsync(id);
            _pregnancyScans.RemoveAll(x => x.ScanId == id);
            OnDelete?.Invoke(id);
        }

        #endregion

        #region Methods

        public async Task Load( int id )
        {
            try
            {
                await Init(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(PregnancyStore)} could not be loaded. Exception: {ex.Message}");
                throw;
            }
            finally
            {
                OnLoad?.Invoke();
            }
        }

        private async Task Init( int id )
        {
            _pregnancyScans.Clear();
            Console.WriteLine($"Loading pregnancy id {id}");
            IEnumerable<PregnancyScan> scans = await _pregnancyService.GetAllPregnancyScansAsync(id);
            Console.WriteLine($"Found {scans.Count()} pregnancy ids");
            _pregnancyScans.AddRange(scans);
        }

        #endregion

        #region Events

        public event Action? OnLoad;
        public event Action<PregnancyScan>? OnSave;
        public event Action<PregnancyScan>? OnUpdate;
        public event Action<int>? OnDelete;

        #endregion

        #region Fields

        private readonly IPregnancyService _pregnancyService;
        private readonly List<PregnancyScan> _pregnancyScans = [];

        #endregion
    }
}
