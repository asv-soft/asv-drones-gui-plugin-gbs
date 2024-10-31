using System.Composition;
using Asv.Cfg;
using Asv.Drones.Gui.Api;
using Microsoft.Extensions.Logging;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [PluginEntryPoint("GBS")]
    [Shared]
    public class GbsPlugin : IPluginEntryPoint
    {
        private ILogger _log;

        [ImportingConstructor]
        public GbsPlugin(ILoggerFactory factory, IConfiguration cfg, IApplicationHost host)
        {
            _log = factory.CreateLogger<GbsPlugin>();
        }

        public async void Initialize() { }

        public void Init() { }

        public void OnFrameworkInitializationCompleted() { }

        public void OnShutdownRequested() { }
    }
}
