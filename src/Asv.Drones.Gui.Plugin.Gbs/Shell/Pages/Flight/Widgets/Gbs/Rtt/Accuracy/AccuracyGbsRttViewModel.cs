using Asv.Common;
using Asv.Drones.Gui.Api;
using Asv.Mavlink;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public class AccuracyGbsRttViewModel : GbsRttItem
    {
        public AccuracyGbsRttViewModel()
        {
            Accuracy = "5 m";
        }

        public AccuracyGbsRttViewModel(
            IGbsClientDevice baseStation,
            ILocalizationService localizationService
        )
            : base(baseStation, "accuracy")
        {
            Order = 1;
            BaseStation
                .Gbs.AccuracyMeter.Subscribe(v =>
                    Accuracy = localizationService.Accuracy.FromSiToStringWithUnits(v)
                )
                .DisposeItWith(Disposable);
            IsMinimizedVisible = true;
        }

        [Reactive]
        public string Accuracy { get; set; } = RS.GbsRttItem_ValueNotAvailable;
    }
}
