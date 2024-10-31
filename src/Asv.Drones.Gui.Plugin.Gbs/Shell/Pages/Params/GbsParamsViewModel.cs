using System.Composition;
using Asv.Cfg;
using Asv.Common;
using Asv.Drones.Gui.Api;
using Asv.Mavlink;

namespace Asv.Drones.Gui.Plugin.Gbs;

[ExportShellPage(Uri)]
public class GbsParamsViewModel : ParamPageViewModel
{
    public const string Uri = $"{WellKnownUri.ShellPage}.params-gbs";

    [ImportingConstructor]
    public GbsParamsViewModel(IMavlinkDevicesService svc, ILogService log, IConfiguration cfg)
        : base(Uri, svc, log, cfg) { }

    public override IParamsClientEx? GetParamsClient(
        IMavlinkDevicesService svc,
        ushort fullId,
        DeviceClass @class
    )
    {
        var dev = svc.GetGbsByFullId(fullId);
        if (dev is null)
        {
            return null;
        }

        dev.Name.Subscribe(n => Title = n).DisposeItWith(Disposable);
        return dev.Params;
    }
}
