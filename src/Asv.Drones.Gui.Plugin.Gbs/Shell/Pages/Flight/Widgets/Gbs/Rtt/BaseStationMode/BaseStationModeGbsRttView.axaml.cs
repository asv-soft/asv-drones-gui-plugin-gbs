using Asv.Drones.Gui.Api;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(BaseStationModeGbsRttViewModel))]
    public partial class BaseStationModeGbsRttView : UserControl
    {
        public BaseStationModeGbsRttView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
