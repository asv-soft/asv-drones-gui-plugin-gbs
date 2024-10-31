using System.Composition;
using Asv.Drones.Gui.Api;
using DynamicData;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [Export(WellKnownUri.ShellPageMapFlight, typeof(IViewModelProvider<IMapAnchor>))]
    public class FlightGbsMapLayerProvider : IViewModelProvider<IMapAnchor>
    {
        [ImportingConstructor]
        public FlightGbsMapLayerProvider(IMavlinkDevicesService devices, ILocalizationService loc)
        {
            Items = devices
                .BaseStations.Transform(x => new GbsAnchor(x, loc))
                .ChangeKey((k, a) => a.Id)
                .Transform(a => (IMapAnchor)a)
                .DisposeMany();
        }

        public IObservable<IChangeSet<IMapAnchor, Uri>> Items { get; }

        public void Dispose() { }
    }
}
