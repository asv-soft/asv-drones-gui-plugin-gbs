using Asv.Drones.Gui.Api;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(AutoModeViewModel))]
    public partial class AutoModeView : ReactiveUserControl<AutoModeViewModel>
    {
        public AutoModeView()
        {
            InitializeComponent();
        }
    }
}
