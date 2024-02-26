using Asv.Drones.Gui.Api;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.Gbs;

[ExportView(typeof(RemoveMapPointViewModel))]

public partial class RemoveMapPointView : ReactiveUserControl<RemoveMapPointViewModel>
{
    public RemoveMapPointView()
    {
        InitializeComponent();
    }
}