using Asv.Common;
using Asv.Mavlink;
using Asv.Mavlink.V2.AsvGbs;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public class BaseStationModeGbsRttViewModel : GbsRttItem
    {
        public BaseStationModeGbsRttViewModel()
        {
            BaseStationMode = "Idle";
        }

        public BaseStationModeGbsRttViewModel(IGbsClientDevice baseStation)
            : base(baseStation, "mode")
        {
            Order = 2;

            BaseStation
                .Gbs.CustomMode.Subscribe(_ =>
                    BaseStationMode = _.ToString().Replace(nameof(AsvGbsCustomMode), string.Empty)
                )
                .DisposeItWith(Disposable);
        }

        [Reactive]
        public string BaseStationMode { get; set; } = RS.GbsRttItem_ValueNotAvailable;
    }
}
