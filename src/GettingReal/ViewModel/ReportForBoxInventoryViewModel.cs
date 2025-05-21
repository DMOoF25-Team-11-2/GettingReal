using System.Collections.ObjectModel;
using GettingReal.Model;

namespace GettingReal.ViewModel;

using GettingReal.Handler;

/// <summary>
/// ViewModel for managing the report of boxes in inventory.
/// </summary>
public class ReportForBoxInventoryViewModel : ViewModelBase
{
    #region Properties
    private readonly BoxRepository _boxRepository;
    private readonly MaterialRepository _materialRepository;
    #endregion

    #region Observable Collections properties
    public ObservableCollection<Box> Boxes { get; }
    private ObservableCollection<Material> _materialsInBox;
    public ObservableCollection<Material> MaterialsInBox
    {
        get => _materialsInBox;
        private set => SetProperty(ref _materialsInBox, value);
    }
    #endregion

    #region Selected properties
    private Box? _selectedBox = null;
    public Box? SelectedBox
    {
        get => _selectedBox;
        set
        {
            if (SetProperty(ref _selectedBox, value))
            {
                UpdateMaterialsInBox();
                PrintReportCommand.RaiseCanExecuteChanged();
            }
        }
    }
    #endregion

    #region Commands properties
    public RelayCommand PrintReportCommand { get; private set; }
    #endregion

    public ReportForBoxInventoryViewModel()
    {
        _boxRepository = new BoxRepository();
        _materialRepository = new MaterialRepository();
        _materialsInBox = [];

        Boxes = [.. _boxRepository.GetAll()];

        PrintReportCommand = new RelayCommand(ExecutePrintReport, CanExecutePrintReport);
    }

    private void UpdateMaterialsInBox()
    {
        if (_selectedBox == null)
            return;

        var materials = new List<Material>();

        foreach (var guid in _selectedBox.MaterialGuids)
        {
            Material? material = _materialRepository.Get(guid);
            if (material != null)
                materials.Add(material);
        }
        MaterialsInBox = [.. materials];
    }

    #region Buttons action
    private void ExecutePrintReport()
    {
        string report = ReportGenerator.ReportBoxInventory(_selectedBox!);
        ReportGenerator.Print(report);
    }
    #endregion

    #region Button condition

    private bool CanExecutePrintReport()
    {
        return _selectedBox != null;
    }
    #endregion
}
