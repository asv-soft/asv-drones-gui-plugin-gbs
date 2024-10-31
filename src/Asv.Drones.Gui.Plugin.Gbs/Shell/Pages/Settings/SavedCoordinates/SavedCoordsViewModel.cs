using System.Collections.ObjectModel;
using System.Composition;
using System.Windows.Input;
using Asv.Cfg;
using Asv.Common;
using Asv.Drones.Gui.Api;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.Plugin.Gbs;

public class FixedModeConfig
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double Altitude { get; set; }
    public double Accuracy { get; set; } = 0.01;
    public string? Name { get; set; }
}

public class FixedModeSavedCoords
{
    public ObservableCollection<FixedModeConfig> Coords { get; init; } = [];
}

public class SavedCoordsViewModel : TreePageViewModel
{
    private readonly ILocalizationService _loc;
    private readonly IConfiguration _cfg;

    public SavedCoordsViewModel()
        : base(WellKnownUri.UndefinedUri)
    {
        DesignTime.ThrowIfNotDesignMode();
    }

    [ImportingConstructor]
    public SavedCoordsViewModel(IConfiguration configuration, ILocalizationService loc)
        : base($"{WellKnownUri.ShellPageSettings}.gbs.coordinates")
    {
        _loc = loc;
        _cfg = configuration;

        UpdateValues(configuration);

        AddNewItemCommand = ReactiveCommand.CreateFromTask(AddNewItem).DisposeItWith(Disposable);

        var canExecuteRemoveCommand = this.WhenAnyValue(
            m => m.SelectedCoordsItem,
            (selected) => selected != null && SavedCoordinates.Contains(selected)
        );

        RemoveItemCommand = ReactiveCommand
            .CreateFromTask(RemoveItem, canExecuteRemoveCommand)
            .DisposeItWith(Disposable);
    }

    private void UpdateValues(IConfiguration configuration)
    {
        var savedCoordsConfig = configuration.Get<FixedModeSavedCoords>();

        SavedCoordinates = savedCoordsConfig.Coords;
    }

    private async Task AddNewItem()
    {
        var dialog = new ContentDialog()
        {
            Title = RS.SavedCoordsViewModel_AddNewItem_Title,
            PrimaryButtonText = RS.SavedCoordsViewModel_AddNewItem_PrimaryButtonText,
            IsSecondaryButtonEnabled = true,
            CloseButtonText = RS.SavedCoordsViewModel_AddNewItem_CloseButtonText,
        };

        var itemToAdd = SelectedCoordsItem ?? new FixedModeConfig();
        var vm = new AddNewMapPointViewModel(itemToAdd, _loc, _cfg);
        vm.ApplyDialog(dialog);
        dialog.Content = vm;
        var result = await dialog.ShowAsync();

        UpdateValues(_cfg);
    }

    private async Task RemoveItem()
    {
        var dialog = new ContentDialog()
        {
            Title = RS.SavedCoordsViewModel_RemoveItem_Title,
            PrimaryButtonText = RS.SavedCoordsViewModel_RemoveItem_PrimaryButtonText,
            IsSecondaryButtonEnabled = true,
            CloseButtonText = RS.SavedCoordsViewModel_RemoveItem_CloseButtonText,
        };

        if (SelectedCoordsItem != null)
        {
            var vm = new RemoveMapPointViewModel(
                SelectedCoordsItem,
                SavedCoordinates.IndexOf(SelectedCoordsItem),
                _loc,
                _cfg
            );
            vm.ApplyDialog(dialog);
            dialog.Content = vm;
        }

        var result = await dialog.ShowAsync();
        UpdateValues(_cfg);
    }

    public ICommand AddNewItemCommand { get; set; }
    public ICommand? RemoveItemCommand { get; set; }

    [Reactive]
    public ObservableCollection<FixedModeConfig> SavedCoordinates { get; set; } = new();

    [Reactive]
    public FixedModeConfig? SelectedCoordsItem { get; set; } = new();
}
