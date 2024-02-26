using Asv.Drones.Gui.Api;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public class SetCoordsNameViewModel : DisposableReactiveObjectWithValidation
    {
        [Reactive] public string Name { get; set; }

        public SetCoordsNameViewModel() 
        {
            this.ValidationRule(x => x.Name, _ => !string.IsNullOrWhiteSpace(_), RS.SetCoordsNameViewModel_Name_ValidValue);
        }
    }
}