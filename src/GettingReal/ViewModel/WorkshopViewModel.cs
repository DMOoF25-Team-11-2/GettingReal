namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using GettingReal.Model;

class WorkshopViewModel : ViewModelBase
{
    #region Properties
    private readonly WorkshopRepository _workshopRepository;
    private readonly MaterialRepository _materialRepository;
    private readonly ActivityRepository _activityRepository;
    #endregion
    #region Observable Collections properties
    public ObservableCollection<Workshop>? Workshops { get; set; } = new ObservableCollection<Workshop>();
    public ObservableCollection<Material>? MaterialsAvailable { get; set; }
    public ObservableCollection<Material>? MaterialsInWorkshop { get; set; }
    public ObservableCollection<Activity>? ActivitiesAvailable { get; set; }
    public ObservableCollection<Activity>? ActivitiesInWorkshop { get; set; }
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
                OnPropertyChanged(nameof(SelectedWorkshop));
                RemoveWorkshopCommand.RaiseCanExecuteChanged();
                SetButtonVisibility();
                UpdateFormValue();
            }
        }
    }
    #endregion
    #region Form properties
    private string _newWorkshopName;
    public string NewWorkshopName
    {
        get => _newWorkshopName;
        set
        {
            if (SetProperty(ref _newWorkshopName, value))
            {
                UpdateWorkshopCommand.RaiseCanExecuteChanged();
                AddWorkshopCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private string _newWorkshopDescription;
    public string NewWorkshopDescription
    {
        get => _newWorkshopDescription;
        set
        {
            if (SetProperty(ref _newWorkshopDescription, value))
            {
                UpdateWorkshopCommand.RaiseCanExecuteChanged();
                AddWorkshopCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private TimeSpan _newWorkshopExpectedTime;
    public TimeSpan NewWorkshopExpectedTime
    {
        get => _newWorkshopExpectedTime;
        set
        {
            if (SetProperty(ref _newWorkshopExpectedTime, value))
            {
                UpdateWorkshopCommand.RaiseCanExecuteChanged();
                AddWorkshopCommand.RaiseCanExecuteChanged();
            }
        }
    }
    #endregion
    #region Button visability properties
    // Button visibility properties
    private Visibility _addButtonVisibility = Visibility.Collapsed;
    public Visibility AddButtonVisibility
    {
        get => _addButtonVisibility;
        set => SetProperty(ref _addButtonVisibility, value);
    }
    private Visibility _removeButtonVisibility = Visibility.Collapsed;
    public Visibility RemoveButtonVisibility
    {
        get => _removeButtonVisibility;
        set => SetProperty(ref _removeButtonVisibility, value);
    }
    private Visibility _updateButtonVisibility = Visibility.Collapsed;
    public Visibility UpdateButtonVisibility
    {
        get => _updateButtonVisibility;
        set => SetProperty(ref _updateButtonVisibility, value);
    }
    private Visibility _removeButtonFromMaterialInWorkshopVisibility = Visibility.Visible;
    public Visibility RemoveButtonFromMaterialInWorkshopVisibility
    {
        get => _removeButtonFromMaterialInWorkshopVisibility;
        set => SetProperty(ref _removeButtonFromMaterialInWorkshopVisibility, value);
    }
    private Visibility _addButtonToMaterialInWorkshopVisibility = Visibility.Visible;
    public Visibility AddButtonToMaterialInWorkshopVisibility
    {
        get => _addButtonToMaterialInWorkshopVisibility;
        set => SetProperty(ref _addButtonToMaterialInWorkshopVisibility, value);
    }
    private Visibility _removeButtonFromActivityInWorkshopVisibility = Visibility.Visible;
    public Visibility RemoveButtonFromActivityInWorkshopVisibility
    {
        get => _removeButtonFromActivityInWorkshopVisibility;
        set => SetProperty(ref _removeButtonFromActivityInWorkshopVisibility, value);
    }
    private Visibility _addButtonToActivityInWorkshopVisibility = Visibility.Visible;
    public Visibility AddButtonToActivityInWorkshopVisibility
    {
        get => _addButtonToActivityInWorkshopVisibility;
        set => SetProperty(ref _addButtonToActivityInWorkshopVisibility, value);
    }
    #endregion
    #region Commands properties
    public RelayCommand AddWorkshopCommand { get; private set; }
    public RelayCommand RemoveWorkshopCommand { get; private set; }
    public RelayCommand UpdateWorkshopCommand { get; private set; }
    #endregion
    public WorkshopViewModel()
    {
        _workshopRepository = new WorkshopRepository();
        _materialRepository = new MaterialRepository();
        _activityRepository = new ActivityRepository();

        Workshops = new ObservableCollection<Workshop>(_workshopRepository.GetAll());
        MaterialsAvailable = new ObservableCollection<Material>(_materialRepository.GetAll());
        ActivitiesAvailable = new ObservableCollection<Activity>(_activityRepository.GetAll());

        _newWorkshopName = string.Empty;
        _newWorkshopDescription = string.Empty;

        AddWorkshopCommand = new RelayCommand(AddWorkshop, CanAddWorkshop);
        RemoveWorkshopCommand = new RelayCommand(RemoveWorkshop, CanRemoveWorkshop);
        UpdateWorkshopCommand = new RelayCommand(SaveWorkshop, CanSaveWorkshop);

        SetButtonVisibility();
    }

    #region Buttons action
    private void AddWorkshop()
    {
        Workshop newWorkshop = new(NewWorkshopName, NewWorkshopDescription);
        _workshopRepository.Add(newWorkshop);
        Workshops?.Add(newWorkshop);
        ClearForm();
        AddWorkshopCommand.RaiseCanExecuteChanged();
    }
    private void RemoveWorkshop()
    {
        if (SelectedWorkshop != null)
        {
            _workshopRepository.Remove(SelectedWorkshop.GUID);
            Workshops?.Remove(SelectedWorkshop);
            RemoveWorkshopCommand.RaiseCanExecuteChanged();
        }
    }
    private void SaveWorkshop()
    {
        //MessageBox.Show("Test", "Test", MessageBoxButton.OK);
        SelectedWorkshop!.Name = NewWorkshopName;
        SelectedWorkshop.Description = NewWorkshopDescription;
        _workshopRepository.Update(SelectedWorkshop);
        //workaround to refresh listview with new data
        var temp = Workshops;
        Workshops = null;
        OnPropertyChanged(nameof(Workshops));
        Workshops = temp;
        OnPropertyChanged(nameof(Workshops));
    }
    #endregion
    #region Buttons conditions

    private bool CanAddWorkshop()
    {
        if (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty)
            return false;
        return IsFormValid();
    }


    private bool CanRemoveWorkshop()
    {
        return SelectedWorkshop != null;
    }


    private bool CanSaveWorkshop()
    {
        return IsFormValid();
    }
    #endregion
    #region Helper methods
    private bool IsFormValid()
    {
        if (string.IsNullOrWhiteSpace(NewWorkshopName))
            return false;
        return true;

    }

    private void SetButtonVisibility()
    {
        AddButtonVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Collapsed : Visibility.Visible;
        UpdateButtonVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
        RemoveButtonVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
    }
    private void UpdateFormValue()
    {
        if (SelectedWorkshop != null)
        {
            NewWorkshopName = SelectedWorkshop.Name;
            NewWorkshopDescription = SelectedWorkshop.Description;
        }
        else
        {
            ClearForm();
        }
    }

    private void ClearForm()
    {
        NewWorkshopName = string.Empty;
        NewWorkshopDescription = string.Empty;
    }
    #endregion
}
