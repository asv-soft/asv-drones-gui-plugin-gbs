using System.Composition;
using Asv.Cfg;
using Asv.Drones.Gui.Api;
using Material.Icons;

namespace Asv.Drones.Gui.Plugin.Gbs;

[Export(WellKnownUri.ShellPageSettings, typeof(ITreePageMenuItem))]
[method: ImportingConstructor]
public class PluginsMarketTreeMenuItem(IConfiguration cfg, ILocalizationService loc)
    : TreePageMenuItem($"{WellKnownUri.ShellPageSettings}.gbs")
{
    public override Uri ParentId => WellKnownUri.UndefinedUri;
    public override string? Name => "Ground base station";
    public override string? Description => "Ground base station settings";
    public override MaterialIconKind Icon => GbsIconHelper.DefaultIcon;
    public override int Order => 600;

    public override ITreePage? CreatePage(ITreePageContext context)
    {
        return new SavedCoordsViewModel(cfg, loc);
    }
}
