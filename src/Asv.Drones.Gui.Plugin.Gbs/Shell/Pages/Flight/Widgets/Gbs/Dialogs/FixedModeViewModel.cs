using System.Collections.ObjectModel;
using System.Composition;
using System.Reactive.Linq;
using System.Windows.Input;
using Asv.Cfg;
using Asv.Common;
using Asv.Drones.Gui.Api;
using Asv.Mavlink;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    [Export]
    public class FixedModeViewModel : DisposableReactiveObjectWithValidation
    {
        private readonly IGbsClientDevice _gbsDevice;
        private readonly ILogService _logService;
        private readonly ILocalizationService _loc;
        private readonly IConfiguration _configuration;

        private const double MinimumAccuracyDistance = 0.01;
        private const int MinimumLatitudeValue = -90;
        private const int MaximumLatitudeValue = 90;
        private const int MinimumLongitudeValue = -180;
        private const int MaximumLongitudeValue = 180;

        public FixedModeViewModel() { }

        [ImportingConstructor]
        public FixedModeViewModel(
            IGbsClientDevice gbsDevice,
            ILogService logService,
            ILocalizationService loc,
            IConfiguration configuration,
            CancellationToken ctx
        )
            : this()
        {
            _gbsDevice = gbsDevice;
            _logService = logService;
            _configuration = configuration;
            _loc = loc;

            var fixedModeSavedCoords = _configuration.Get<FixedModeSavedCoords>();

            if (fixedModeSavedCoords.Coords != null)
            {
                MapCoords = fixedModeSavedCoords.Coords;
            }

            SaveCurrentValuesCommand = ReactiveCommand
                .Create(AddNewSavedCoords)
                .DisposeItWith(Disposable);

            this.WhenAnyValue(_ => _.SelectedConfigItem)
                .Subscribe(SetValues)
                .DisposeItWith(Disposable);
            SelectedConfigItem = _configuration.Get<FixedModeConfig>();
            SetValues(SelectedConfigItem);

            #region Validation Rules

            this.ValidationRule(
                    x => x.Accuracy,
                    property =>
                        property is not null
                        && _loc.Accuracy.IsValid(
                            MinimumAccuracyDistance,
                            double.MaxValue,
                            property
                        ),
                    string.Format(
                        RS.AutoModeViewModel_Accuracy_ValidValue,
                        _loc.Accuracy.FromSiToString(MinimumAccuracyDistance)
                    )
                )
                .DisposeItWith(Disposable);

            this.ValidationRule(
                    x => x.Latitude,
                    property =>
                        property is not null
                        && _loc.Latitude.IsValid(
                            MinimumLatitudeValue,
                            MaximumLatitudeValue,
                            property
                        ),
                    _ => _loc.Latitude.GetErrorMessage(_)
                )
                .DisposeItWith(Disposable);

            this.ValidationRule(
                    x => x.Longitude,
                    property =>
                        property is not null
                        && _loc.Longitude.IsValid(
                            MinimumLongitudeValue,
                            MaximumLongitudeValue,
                            property
                        ),
                    _ => _loc.Longitude.GetErrorMessage(_)
                )
                .DisposeItWith(Disposable);

            this.ValidationRule(
                    x => x.Altitude,
                    _ => _loc.Altitude.IsValid(_),
                    _ => _loc.Altitude.GetErrorMessage(_)
                )
                .DisposeItWith(Disposable);

            #endregion
        }

        private void SetValues(FixedModeConfig cfg)
        {
            Latitude = _loc.Latitude.FromSiToString(cfg.Latitude);
            Longitude = _loc.Longitude.FromSiToString(cfg.Longitude);
            Altitude = _loc.Altitude.FromSiToString(cfg.Altitude);
            Accuracy = _loc.Accuracy.FromSiToString(cfg.Accuracy);
        }

        public void ApplyDialog(ContentDialog dialog)
        {
            ArgumentNullException.ThrowIfNull(dialog);

            dialog.PrimaryButtonCommand = ReactiveCommand
                .CreateFromTask(
                    SetUpFixedMode,
                    this.IsValid().Do(_ => dialog.IsPrimaryButtonEnabled = _)
                )
                .DisposeItWith(Disposable);
        }

        private async Task SetUpFixedMode(CancellationToken cancel)
        {
            if (_gbsDevice == null)
            {
                return;
            }

            var lat = _loc.Latitude.ConvertToSi(Latitude);
            var lon = _loc.Longitude.ConvertToSi(Longitude);
            var alt = _loc.Altitude.ConvertToSi(Altitude);
            var acc = _loc.Accuracy.ConvertToSi(Accuracy);

            _configuration.Set(
                new FixedModeConfig
                {
                    Longitude = lon,
                    Latitude = lat,
                    Altitude = alt,
                    Accuracy = acc,
                }
            );

            _configuration.Set(new FixedModeSavedCoords { Coords = MapCoords });

            try
            {
                await _gbsDevice.Gbs.StartFixedMode(
                    new GeoPoint(lat, lon, alt),
                    (float)_loc.Accuracy.ConvertToSi(Accuracy),
                    cancel
                );
            }
            catch (Exception e)
            {
                _logService.Error(
                    string.Empty,
                    string.Format(RS.FixedModeViewModel_StartFailed, e.Message),
                    e
                );
            }
        }

        private async void AddNewSavedCoords()
        {
            var dialog = new ContentDialog()
            {
                Title = RS.FixedModeViewModel_SetCoordsName_Title,
                PrimaryButtonText = RS.FixedModeViewModel_SetCoordsName_PrimaryButtonText,
                IsSecondaryButtonEnabled = true,
                CloseButtonText = RS.FixedModeViewModel_SetCoordsName_SecondaryButtonText,
            };

            using var vm = new SetCoordsNameViewModel();
            dialog.Content = vm;
            var result = await dialog.ShowAsync();

            var lat = _loc.Latitude.ConvertToSi(Latitude);
            var lon = _loc.Longitude.ConvertToSi(Longitude);
            var alt = _loc.Altitude.ConvertToSi(Altitude);
            var acc = _loc.Accuracy.ConvertToSi(Accuracy);
            var name = vm.Name;

            MapCoords.Add(
                new FixedModeConfig
                {
                    Name = name,
                    Latitude = lat,
                    Longitude = lon,
                    Altitude = alt,
                    Accuracy = acc,
                }
            );
        }

        [Reactive]
        public string Latitude { get; set; } = "0";

        [Reactive]
        public string Longitude { get; set; } = "0";

        [Reactive]
        public string Altitude { get; set; } = "0";

        [Reactive]
        public string Accuracy { get; set; } = "0";

        [Reactive]
        public ObservableCollection<FixedModeConfig> MapCoords { get; set; } = new();

        [Reactive]
        public FixedModeConfig SelectedConfigItem { get; set; } = new();

        public string AccuracyUnits => _loc.Accuracy.CurrentUnit.Value.Unit;
        public string LatitudeUnits => _loc.Latitude.CurrentUnit.Value.Unit;
        public string LongitudeUnits => _loc.Longitude.CurrentUnit.Value.Unit;
        public string AltitudeUnits => _loc.Altitude.CurrentUnit.Value.Unit;
        public ICommand SaveCurrentValuesCommand { get; set; }
    }
}
