using System.Composition;
using Asv.Drones.Gui.Api;
using Asv.Mavlink;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [Export(typeof(IGbsRttItemProvider))]
    
    public class DefaultGbsRttItemProvider : IGbsRttItemProvider
    {
        private readonly ILocalizationService _localizationService;
    
        [ImportingConstructor]
        public DefaultGbsRttItemProvider(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
    
        public IEnumerable<IGbsRttItem> Create(IGbsClientDevice device)
        {
            yield return new LinkQualityGbsRttViewModel(device);
            yield return new VisibleSatellitesGbsRttViewModel(device);
            yield return new BaseStationModeGbsRttViewModel(device);
            yield return new AccuracyGbsRttViewModel(device, _localizationService);
            yield return new ObservationGbsRttViewModel(device,_localizationService);
            yield return new DGpsRateGbsRttViewModel(device, _localizationService);
        }
    }
}