using Asv.Drones.Gui.Api;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(FlightGbsViewModel))]
    public partial class FlightGbsView : ReactiveUserControl<FlightGbsViewModel>
    {
        public FlightGbsView()
        {
            InitializeComponent();
        }
    }
}
