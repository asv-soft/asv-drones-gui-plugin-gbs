using System.Composition;
using Asv.Cfg;
using Asv.Drones.Gui.Api;
using DynamicData;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [Export(WellKnownUri.ShellPageMapFlight, typeof(IViewModelProvider<IMapWidget>))]
    public class FlightMissionWidgetProvider:ViewModelProviderBase<IMapWidget>
    {
        [ImportingConstructor]
        public FlightMissionWidgetProvider(
            IMavlinkDevicesService devices,ILogService log,
            ILocalizationService localization,
            IConfiguration configuration,
            ISoundNotificationService soundNotification,
            [ImportMany]IEnumerable<IGbsRttItemProvider> rttItems)
        {
            devices.BaseStations
                .Transform(_ => (IMapWidget)new FlightGbsViewModel(_,log, soundNotification, localization,configuration,rttItems))
                .ChangeKey( ((_, v) => v.Id) )
                .PopulateInto(Source);
        }
    }
}