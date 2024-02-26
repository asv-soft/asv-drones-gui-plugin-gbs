using Asv.Drones.Gui.Api;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [ExportView(typeof(SetCoordsNameViewModel))]
    
    public partial class SetCoordsNameView : ReactiveUserControl<SetCoordsNameViewModel>
    {
        public SetCoordsNameView()
        {
            InitializeComponent();
        }
    }
}