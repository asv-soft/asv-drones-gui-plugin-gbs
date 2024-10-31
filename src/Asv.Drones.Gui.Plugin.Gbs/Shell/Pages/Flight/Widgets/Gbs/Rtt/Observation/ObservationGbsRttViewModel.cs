using Asv.Common;
using Asv.Drones.Gui.Api;
using Asv.Mavlink;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public class ObservationGbsRttViewModel : GbsRttItem
    {
        public ObservationGbsRttViewModel()
        {
            Observation = "30%";
        }

        public ObservationGbsRttViewModel(IGbsClientDevice baseStation, ILocalizationService loc)
            : base(baseStation, "observation")
        {
            Order = 1;

            BaseStation
                .Gbs.ObservationSec.Subscribe(_ =>
                    Observation = loc.RelativeTime.ConvertToString(TimeSpan.FromSeconds(_))
                )
                .DisposeItWith(Disposable);
        }

        [Reactive]
        public string Observation { get; set; } = RS.GbsRttItem_ValueNotAvailable;
    }
}
