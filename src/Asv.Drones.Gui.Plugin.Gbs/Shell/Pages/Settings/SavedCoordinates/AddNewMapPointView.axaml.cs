using Asv.Drones.Gui.Api;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.Gbs;

[ExportView(typeof(AddNewMapPointViewModel))]
public partial class AddNewMapPointView : ReactiveUserControl<AddNewMapPointViewModel>
{
    public AddNewMapPointView()
    {
        InitializeComponent();
    }
}
