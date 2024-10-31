using Asv.Drones.Gui.Api;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(ObservationGbsRttViewModel))]
    public partial class ObservationGbsRttView : UserControl
    {
        public ObservationGbsRttView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
