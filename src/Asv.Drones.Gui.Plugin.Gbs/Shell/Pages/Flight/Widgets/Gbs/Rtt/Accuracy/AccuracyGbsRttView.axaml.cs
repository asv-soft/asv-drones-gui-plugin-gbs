using Asv.Drones.Gui.Api;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(AccuracyGbsRttViewModel))]
    public partial class AccuracyGbsRttView : UserControl
    {
        public AccuracyGbsRttView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
