using Asv.Common;
using Asv.Mavlink;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public class LinkQualityGbsRttViewModel : GbsRttItem
    {
        public LinkQualityGbsRttViewModel()
        {
            LinkQuality = 0.3;
            LinkQualityString = "30%";
        }

        public LinkQualityGbsRttViewModel(IGbsClientDevice baseStation) : base(baseStation, "quality")
        {
            Order = 1;
        
            BaseStation.Heartbeat.LinkQuality
                .Subscribe(_ => LinkQuality = _)
                .DisposeItWith(Disposable);

            BaseStation.Heartbeat.LinkQuality
                .Subscribe(_ => LinkQualityString = _.ToString("P0"))
                .DisposeItWith(Disposable);

            IsMinimizedVisible = true;
        }
    
        [Reactive]
        public double LinkQuality { get; set; }

        [Reactive]
        public string LinkQualityString { get; set; } = RS.GbsRttItem_ValueNotAvailable;
    }
}