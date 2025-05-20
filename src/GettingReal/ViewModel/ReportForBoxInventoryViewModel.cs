using System.Collections.ObjectModel;
using GettingReal.Model;

namespace GettingReal.ViewModel;

using GettingReal.Handler;

public class ReportForBoxInventoryViewModel : ViewModelBase
{
    #region Properties
    private readonly BoxRepository _boxRepository;
    private readonly MaterialRepository _materialRepository;
    #endregion

    #region Observable Collections properties
    public ObservableCollection<Box> Boxes { get; }
    private ObservableCollection<Material>? _materialsInBox;
    public ObservableCollection<Material>? MaterialsInBox
    {
        get => _materialsInBox;
        private set => SetProperty(ref _materialsInBox, value);
    }
    #endregion

    #region Selected properties
    private Box? _selectedBox;
    public Box? SelectedBox
    {
        get => _selectedBox;
        set
        {
            if (SetProperty(ref _selectedBox, value))
            {
                UpdateMaterialsInBox();
                OnPropertyChanged(nameof(SelectedBox));
            }
        }
    }
    #endregion

    #region Commands properties
    public RelayCommand ExportToPdfCommand { get; private set; }
    public RelayCommand PrintReportCommand { get; private set; }
    #endregion

    public ReportForBoxInventoryViewModel()
    {
        _boxRepository = new BoxRepository();
        _materialRepository = new MaterialRepository();

        Boxes = new ObservableCollection<Box>(_boxRepository.GetAll());

        ExportToPdfCommand = new RelayCommand(ExecuteExportToPdf, CanExecuteExportToPdf);
        PrintReportCommand = new RelayCommand(ExecutePrintReport, CanExecutePrintReport);
    }

    private void UpdateMaterialsInBox()
    {
        if (_selectedBox == null)
            return;

        var materials = new List<Material>();

        foreach (var guid in _selectedBox.MaterialGuids)
        {
            var material = _materialRepository.Get(guid);
            if (material != null)
                materials.Add(material);
        }

        MaterialsInBox = new ObservableCollection<Material>(materials);
    }

    #region Buttons action
    private void ExecuteExportToPdf()
    {
        string report = ReportGenerator.ReportBoxInventory(_selectedBox);
        ReportGenerator.Print(report);
    }

    private void ExecutePrintReport()
    {
    }
    #endregion

    #region Button condition
    private bool CanExecuteExportToPdf()
    {
        return SelectedBox != null;
    }

    private bool CanExecutePrintReport()
    {
        return SelectedBox != null;
    }
    #endregion
}
