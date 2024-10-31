using Asv.Drones.Gui.Api;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(VisibleSatellitesGbsRttViewModel))]
    public partial class VisibleSatellitesGbsRttView : UserControl
    {
        public VisibleSatellitesGbsRttView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
