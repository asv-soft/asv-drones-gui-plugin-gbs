using Asv.Avalonia.Map;
using Asv.Common;
using Asv.Drones.Gui.Api;
using Asv.Mavlink;
using Asv.Mavlink.V2.AsvGbs;
using Avalonia.Media;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public class GbsAnchor : MapAnchorBase
    {
        private readonly IGbsClientDevice _device;
        private readonly ILocalizationService _loc;

        public GbsAnchor(IGbsClientDevice device, ILocalizationService loc)
            : base($"{WellKnownUri.ShellPageMapFlightAnchor}.gbs.{device.FullId}")
        {
            _device = device;
            _loc = loc;
            Size = 48;
            OffsetX = OffsetXEnum.Center;
            OffsetY = OffsetYEnum.Center;
            StrokeThickness = 1;
            Stroke = Brushes.Indigo;
            IconBrush = Brushes.Teal;
            IsVisible = true;
            Title = RS.GbsAnchor_Title;

            device.Gbs.Position.Subscribe(_ => Location = _).DisposeItWith(Disposable);
            device.Gbs.Position.Subscribe(_ => UpdateDescription()).DisposeItWith(Disposable);
            device.Gbs.CustomMode.Subscribe(SetIcon).DisposeItWith(Disposable);
        }

        public IGbsClientDevice Device => _device;

        private void SetIcon(AsvGbsCustomMode mode)
        {
            Icon =
                mode == AsvGbsCustomMode.AsvGbsCustomModeError
                    ? GbsIconHelper.DefaultIcon
                    : GbsIconHelper.DefaultIconDisconnected;
        }

        private void UpdateDescription()
        {
            Description =
                string.Format(
                    RS.GbsAnchor_Latitude,
                    _loc.Latitude.FromSiToStringWithUnits(_device.Gbs.Position.Value.Latitude)
                )
                + "\n"
                + string.Format(
                    RS.GbsAnchor_Longitude,
                    _loc.Longitude.FromSiToStringWithUnits(_device.Gbs.Position.Value.Longitude)
                )
                + "\n"
                + string.Format(
                    RS.GbsAnchor_GNSS_Altitude,
                    _loc.Altitude.FromSiToStringWithUnits(_device.Gbs.Position.Value.Altitude)
                )
                + "\n";
        }
    }
}
