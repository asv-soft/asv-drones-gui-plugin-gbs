﻿using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using Asv.Cfg;
using Asv.Common;
using Asv.Drones.Gui.Api;
using Asv.Mavlink;
using Asv.Mavlink.V2.AsvGbs;
using Avalonia.Controls;
using DynamicData;
using FluentAvalonia.UI.Controls;
using Material.Icons;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.Gbs
{
    public class FlightGbsViewModel : MapWidgetBase
    {
        private readonly ReadOnlyObservableCollection<IGbsRttItem> _rttItems;
        private readonly IGbsClientDevice _baseStationDevice;
        private readonly ILogService _logService;
        private readonly ILocalizationService _loc;
        private readonly IConfiguration _configuration;
        private readonly ISoundNotificationService _soundNotification;

        private Subject<bool> _canExecuteAutoCommand = new();
        private Subject<bool> _canExecuteFixedCommand = new();
        private Subject<bool> _canExecuteIdleCommand = new();
        private Subject<bool> _canExecuteCancelCommand = new();

        public FlightGbsViewModel(
            IGbsClientDevice baseStationDevice,
            ILogService log,
            ISoundNotificationService soundNotification,
            ILocalizationService loc,
            IConfiguration configuration,
            IEnumerable<IGbsRttItemProvider> rttItems
        )
            : base($"{WellKnownUri.ShellPageMapFlightWidget}.gbs.{baseStationDevice.FullId}")
        {
            _baseStationDevice = baseStationDevice;
            _logService = log;
            _soundNotification = soundNotification;
            _loc = loc;
            _configuration = configuration;
            Order = 200;
            Icon = MaterialIconKind.RouterWireless;
            Title = RS.FlightGbsViewModel_Title;

            rttItems
                .SelectMany(_ => _.Create(baseStationDevice))
                .OrderBy(_ => _.Order)
                .AsObservableChangeSet()
                .AutoRefresh(_ => _.IsVisible)
                .Filter(_ => _.IsVisible)
                .Bind(out _rttItems)
                .DisposeMany()
                .Subscribe()
                .DisposeItWith(Disposable);

            MinimizedRttItems = _rttItems.Where(_ => _.IsMinimizedVisible);

            EnableAutoCommand = ReactiveCommand
                .CreateFromTask(EnableAutoMode, _canExecuteAutoCommand)
                .DisposeItWith(Disposable);
            EnableFixedCommand = ReactiveCommand
                .CreateFromTask(EnableFixedMode, _canExecuteFixedCommand)
                .DisposeItWith(Disposable);
            EnableIdleCommand = ReactiveCommand
                .CreateFromTask(EnableIdleMode, _canExecuteIdleCommand)
                .DisposeItWith(Disposable);
            CancelCommand = ReactiveCommand
                .CreateFromTask(EnableIdleMode, _canExecuteCancelCommand)
                .DisposeItWith(Disposable);
            ChangeStateCommand = ReactiveCommand.Create(() =>
            {
                IsMinimized = !IsMinimized;
            });

            // Subscribe only after creating commands
            _baseStationDevice
                .Gbs.CustomMode.DistinctUntilChanged()
                .Subscribe(SwitchMode)
                .DisposeItWith(Disposable);

            _baseStationDevice
                .Gbs.BeidouSatellites.Subscribe(_ =>
                    BeidouSats = new GridLength(_, GridUnitType.Star)
                )
                .DisposeItWith(Disposable);
            _baseStationDevice
                .Gbs.GalSatellites.Subscribe(_ => GalSats = new GridLength(_, GridUnitType.Star))
                .DisposeItWith(Disposable);
            _baseStationDevice
                .Gbs.GlonassSatellites.Subscribe(_ =>
                    GlonassSats = new GridLength(_, GridUnitType.Star)
                )
                .DisposeItWith(Disposable);
            _baseStationDevice
                .Gbs.GpsSatellites.Subscribe(_ => GpsSats = new GridLength(_, GridUnitType.Star))
                .DisposeItWith(Disposable);
            _baseStationDevice
                .Gbs.ImesSatellites.Subscribe(_ => ImesSats = new GridLength(_, GridUnitType.Star))
                .DisposeItWith(Disposable);
            _baseStationDevice
                .Gbs.QzssSatellites.Subscribe(_ => QzssSats = new GridLength(_, GridUnitType.Star))
                .DisposeItWith(Disposable);
            _baseStationDevice
                .Gbs.SbasSatellites.Subscribe(_ => SbasSats = new GridLength(_, GridUnitType.Star))
                .DisposeItWith(Disposable);
        }

        private void SwitchMode(AsvGbsCustomMode mode)
        {
            IsProgressShown = false;
            IsDisableShown = false;

            switch (mode)
            {
                case AsvGbsCustomMode.AsvGbsCustomModeLoading:
                    _canExecuteAutoCommand.OnNext(false);
                    _canExecuteFixedCommand.OnNext(false);
                    _canExecuteIdleCommand.OnNext(false);
                    _canExecuteCancelCommand.OnNext(false);

                    IsProgressShown = true;
                    break;
                case AsvGbsCustomMode.AsvGbsCustomModeIdle:
                    _canExecuteAutoCommand.OnNext(true);
                    _canExecuteFixedCommand.OnNext(true);
                    _canExecuteIdleCommand.OnNext(false);
                    _canExecuteCancelCommand.OnNext(false);

                    _soundNotification.Notify();
                    break;
                case AsvGbsCustomMode.AsvGbsCustomModeError:
                    //TODO: Implement error state
                    break;
                case AsvGbsCustomMode.AsvGbsCustomModeAutoInProgress:
                    _canExecuteAutoCommand.OnNext(false);
                    _canExecuteFixedCommand.OnNext(false);
                    _canExecuteIdleCommand.OnNext(false);
                    _canExecuteCancelCommand.OnNext(true);

                    IsProgressShown = true;
                    break;
                case AsvGbsCustomMode.AsvGbsCustomModeAuto:
                    _canExecuteAutoCommand.OnNext(false);
                    _canExecuteFixedCommand.OnNext(false);
                    _canExecuteIdleCommand.OnNext(true);
                    _canExecuteCancelCommand.OnNext(false);

                    IsDisableShown = true;
                    _soundNotification.Notify();
                    break;
                case AsvGbsCustomMode.AsvGbsCustomModeFixedInProgress:
                    _canExecuteAutoCommand.OnNext(false);
                    _canExecuteFixedCommand.OnNext(false);
                    _canExecuteIdleCommand.OnNext(false);
                    _canExecuteCancelCommand.OnNext(true);

                    IsProgressShown = true;
                    break;
                case AsvGbsCustomMode.AsvGbsCustomModeFixed:
                    _canExecuteAutoCommand.OnNext(false);
                    _canExecuteFixedCommand.OnNext(false);
                    _canExecuteIdleCommand.OnNext(true);
                    _canExecuteCancelCommand.OnNext(false);

                    IsDisableShown = true;
                    _soundNotification.Notify();
                    break;
            }
        }

        private async Task EnableAutoMode(CancellationToken ctx)
        {
            var dialog = new ContentDialog()
            {
                Title = RS.FlightGbsViewModel_AutoMode_Title,
                PrimaryButtonText = RS.FlightGbsViewModel_AutoMode_PrimaryButtonText,
                IsSecondaryButtonEnabled = true,
                CloseButtonText = RS.FlightGbsViewModel_AutoMode_CloseButtonText,
            };

            using var viewModel = new AutoModeViewModel(
                _baseStationDevice,
                _logService,
                _loc,
                _configuration,
                ctx
            );
            viewModel.ApplyDialog(dialog);
            dialog.Content = viewModel;
            await dialog.ShowAsync();
        }

        private async Task EnableFixedMode(CancellationToken ctx)
        {
            var dialog = new ContentDialog()
            {
                Title = RS.FlightGbsViewModel_FixedMode_Title,
                PrimaryButtonText = RS.FlightGbsViewModel_FixedMode_PrimaryButtonText,
                IsSecondaryButtonEnabled = true,
                CloseButtonText = RS.FlightGbsViewModel_FixedMode_CloseButtonText,
            };

            using var viewModel = new FixedModeViewModel(
                _baseStationDevice,
                _logService,
                _loc,
                _configuration,
                ctx
            );
            viewModel.ApplyDialog(dialog);
            dialog.Content = viewModel;
            await dialog.ShowAsync();
        }

        private async Task EnableIdleMode(CancellationToken ctx)
        {
            await _baseStationDevice.Gbs.StartIdleMode(ctx);
        }

        protected override void InternalAfterMapInit(IMap context)
        {
            LocateBaseStationCommand = ReactiveCommand
                .Create(() =>
                {
                    Map.Center = _baseStationDevice.Gbs.Position.Value;
                    var selectedGbs = Map
                        .Markers.Where(_ => _ is GbsAnchor)
                        .Cast<GbsAnchor>()
                        .FirstOrDefault(_ => _.Device.FullId == _baseStationDevice.FullId);
                    if (selectedGbs != null)
                    {
                        selectedGbs.IsSelected = true;
                    }
                })
                .DisposeItWith(Disposable);
        }

        public ICommand LocateBaseStationCommand { get; set; }
        public ICommand EnableAutoCommand { get; set; }
        public ICommand EnableFixedCommand { get; set; }
        public ICommand EnableIdleCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ChangeStateCommand { get; set; }

        public ReadOnlyObservableCollection<IGbsRttItem> RttItems => _rttItems;
        public IEnumerable<IGbsRttItem> MinimizedRttItems { get; set; }

        [Reactive]
        public bool IsProgressShown { get; set; }

        [Reactive]
        public bool IsDisableShown { get; set; }

        [Reactive]
        public bool IsMinimized { get; set; } = false;

        [Reactive]
        public GridLength BeidouSats { get; set; }

        [Reactive]
        public GridLength GalSats { get; set; }

        [Reactive]
        public GridLength GlonassSats { get; set; }

        [Reactive]
        public GridLength GpsSats { get; set; }

        [Reactive]
        public GridLength ImesSats { get; set; }

        [Reactive]
        public GridLength QzssSats { get; set; }

        [Reactive]
        public GridLength SbasSats { get; set; }
    }
}
