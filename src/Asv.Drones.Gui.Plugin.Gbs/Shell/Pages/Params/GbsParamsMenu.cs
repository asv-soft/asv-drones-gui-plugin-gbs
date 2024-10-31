using System.Composition;
using Asv.Drones.Gui.Api;
using Asv.Mavlink;
using Material.Icons;

namespace Asv.Drones.Gui.Plugin.Gbs;

[Export(nameof(DeviceClass.GbsRtk), typeof(IShellMenuItem<IClientDevice>))]
public class GbsParamsMenu : ShellMenuItem, IShellMenuItem<IClientDevice>
{
    public GbsParamsMenu()
        : base(GbsParamsViewModel.Uri)
    {
        Icon = MaterialIconDataProvider.GetData(MaterialIconKind.WrenchCog);
        Position = ShellMenuPosition.Top;
        Type = ShellMenuItemType.PageNavigation;
        Order = 100;
        Name = RS.GbsParamsMenu_GbsParamsMenu_Settings;
    }

    public IShellMenuItem Init(IClientDevice target)
    {
        NavigateTo = ParamPageViewModel.GenerateUri(
            GbsParamsViewModel.Uri,
            target.FullId,
            target.Class
        );
        return this;
    }
}
