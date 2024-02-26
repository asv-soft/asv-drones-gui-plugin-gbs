using Asv.Drones.Gui.Api;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(LinkQualityGbsRttViewModel))]
    
    public partial class LinkQualityGbsRttView : UserControl
    {
        public LinkQualityGbsRttView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}