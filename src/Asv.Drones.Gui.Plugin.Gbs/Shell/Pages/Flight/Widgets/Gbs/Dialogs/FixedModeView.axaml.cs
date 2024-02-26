using Asv.Drones.Gui.Api;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(FixedModeViewModel))]
    
    public partial class FixedModeView : ReactiveUserControl<FixedModeViewModel>
    {
        public FixedModeView()
        {
            InitializeComponent();
        }
    }
}