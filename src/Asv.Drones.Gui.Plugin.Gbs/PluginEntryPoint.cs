using System.Composition;
using Asv.Cfg;
using Asv.Drones.Gui.Api;
using NLog;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [PluginEntryPoint("GBS")]
    [Shared]
    public class PluginEntryPoint:IPluginEntryPoint
    {
        Logger _log = LogManager.GetCurrentClassLogger();
    
        [ImportingConstructor]
        public PluginEntryPoint(IConfiguration cfg, IApplicationHost host)
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