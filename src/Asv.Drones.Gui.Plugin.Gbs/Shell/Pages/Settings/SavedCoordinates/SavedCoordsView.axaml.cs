using Asv.Drones.Gui.Api;
using Avalonia.ReactiveUI;

namespace Asv.Drones.Gui.Plugin.Gbs;

[ExportView(typeof(SavedCoordsViewModel))]
public partial class SavedCoordsView : ReactiveUserControl<SavedCoordsViewModel>
{
    public SavedCoordsView()
    {
        InitializeComponent();
    }
}
