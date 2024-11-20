using System.Composition;
using Asv.Cfg;
using Asv.Drones.Gui.Api;
using NLog;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [PluginEntryPoint("GBS")]
    [Shared]
    public class GbsPlugin:IPluginEntryPoint
    {
        private Logger log = LogManager.GetLogger("GBS");
        [ImportingConstructor]
        public GbsPlugin(IConfiguration cfg, IApplicationHost host)
        {
         
        }
        public async void Initialize()
        {
       

        }

        public void Init()
        {
        
        }

        public void OnFrameworkInitializationCompleted()
        {
        
        }

        public void OnShutdownRequested()
        {
        
        }
    }
}