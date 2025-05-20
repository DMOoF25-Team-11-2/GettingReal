using System.Collections.ObjectModel;
using GettingReal.Model;

namespace GettingReal.ViewModel;

using GettingReal.Handler;
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
                PrintReportCommand.RaiseCanExecuteChanged();
            }
        }
    }
    private object _selectedBox;

    public object SelectedBox { get => _selectedBox; set => SetProperty(ref _selectedBox, value); }
    #endregion

    #region Commands properties
    public RelayCommand PrintReportCommand { get; private set; }
    #endregion

    public ReportForBoxesInWorkshopViewModel()
    {
        _workshopRepository = new WorkshopRepository();
        _boxRepository = new BoxRepository();

        Workshops = new ObservableCollection<Workshop>(_workshopRepository.GetAll());

        PrintReportCommand = new RelayCommand(ExecutePrintReport, CanExecutePrintReport);
    }

    private void UpdateBoxesForWorkshop()
    {
        if (_selectedWorkshop == null)
            return;

        var boxes = _selectedWorkshop.GetBoxesForWorkshop()?.ToList()
            ?? [];

        BoxesForWorkshop = [.. boxes];
    }

    #region Buttons action
    private void ExecutePrintReport()
    {
        string report = ReportGenerator.ReportBoxesNeededForWorkshop(_selectedWorkshop!);
        ReportGenerator.Print(report);
    }
    #endregion


    #region Button condition
    private bool CanExecutePrintReport()
    {
        return SelectedWorkshop != null;
    }
    #endregion
}
