using Asv.Common;
using Asv.Mavlink;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public class VisibleSatellitesGbsRttViewModel : GbsRttItem
    {
        public VisibleSatellitesGbsRttViewModel()
        {
        }

        public VisibleSatellitesGbsRttViewModel(IGbsClientDevice baseStation) : base(baseStation, "sat")
        {
            Order = 1;

            BaseStation.Gbs.AllSatellites
                .Subscribe(_ => VisibleSatellites = _.ToString())
                .DisposeItWith(Disposable);
        }

        [Reactive] public string VisibleSatellites { get; set; } = RS.GbsRttItem_ValueNotAvailable;
    }
}