using System.Collections.ObjectModel;
using GettingReal.Model;

namespace GettingReal.ViewModel;

public class ReportForBoxesInWorkshopViewModel : ViewModelBase
{
    #region Properties
    private readonly WorkshopRepository _workshopRepository;
    private readonly BoxRepository _boxRepository;
    #endregion

    #region Observable Collections properties
    public ObservableCollection<Workshop> Workshops { get; }
    private ObservableCollection<Box>? _boxesForWorkshop;
    public ObservableCollection<Box>? BoxesForWorkshop
    {
        get => _boxesForWorkshop;
        private set => SetProperty(ref _boxesForWorkshop, value);
    }
    #endregion

    #region Selected properties
    private Workshop? _selectedWorkshop;
    public Workshop? SelectedWorkshop
    {
        get => _selectedWorkshop;
        set
        {
            if (SetProperty(ref _selectedWorkshop, value))
            {
                UpdateBoxesForWorkshop();
                OnPropertyChanged(nameof(SelectedWorkshop));
            }
        }
    }
    #endregion


    #region Commands properties
    public RelayCommand ExportToPdfCommand { get; private set; }
    public RelayCommand PrintReportCommand { get; private set; }
    #endregion
    public ReportForBoxesInWorkshopViewModel()
    {
        _workshopRepository = new WorkshopRepository();
        _boxRepository = new BoxRepository();

        Workshops = new ObservableCollection<Workshop>(_workshopRepository.GetAll());

        ExportToPdfCommand = new RelayCommand(ExecuteExportToPdf);
        PrintReportCommand = new RelayCommand(ExecutePrintReport);
    }

    private void UpdateBoxesForWorkshop()
    {
        if (SelectedWorkshop == null)
        {
            BoxesForWorkshop = null;
            return;
        }

        // If Workshop has a GetBoxesForWorkshop method, use it:
        var boxes = SelectedWorkshop.GetBoxesForWorkshop()?.ToList()
            ?? new List<Box>();

        BoxesForWorkshop = new ObservableCollection<Box>(boxes);
    }

    #region Buttons action
    private void ExecuteExportToPdf()
    {
    }

    private void ExecutePrintReport()
    {
    }
    #endregion
    private bool CanExecutePrintReport()
    {
        return true; // Added a return value to avoid compilation errors
    }
}
