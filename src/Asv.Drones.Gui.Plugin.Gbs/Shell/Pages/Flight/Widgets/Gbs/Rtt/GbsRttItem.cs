using Asv.Drones.Gui.Api;
using Asv.Mavlink;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public abstract class GbsRttItem : ViewModelBase, IGbsRttItem
    {
        protected IGbsClientDevice BaseStation { get; }

        public GbsRttItem()
            : base(WellKnownUri.UndefinedUri) { }

        protected GbsRttItem(IGbsClientDevice baseStation, string name)
            : base($"{WellKnownUri.ShellPageMapFlightWidget}.gbs.{name}")
        {
            IsVisible = true;
            BaseStation = baseStation;
        }

        [Reactive]
        public int Order { get; set; }

        [Reactive]
        public bool IsVisible { get; set; }

        [Reactive]
        public bool IsMinimizedVisible { get; set; }
    }
}
