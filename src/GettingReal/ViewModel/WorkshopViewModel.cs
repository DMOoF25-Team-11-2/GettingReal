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
    public ObservableCollection<Activity>? ActivitiesAvailable { 
        get; // Burde frasotere alle aktiviteter i workshop
        set;
    }
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
                SetFormMaterialVisibility();
                UpdateFormValue();
            }
        }
    }
    private Activity? _selectedActivity;
    public Activity? SelectedActivity
    {
        get => _selectedActivity;
        set
        {
            if (SetProperty(ref _selectedActivity, value))
            {
                OnPropertyChanged(nameof(SelectedActivity));
                RemoveActivityFromWorkshopCommand.RaiseCanExecuteChanged();
                AddActivityToWorkshopCommand.RaiseCanExecuteChanged();
            }
        }
    }
    private Activity? _selectedActivityInWorkshop;
    public Activity? SelectedActivityInWorkshop
    {
        get => _selectedActivityInWorkshop;
        set
        {
            if (SetProperty(ref _selectedActivityInWorkshop, value))
            {
                OnPropertyChanged(nameof(SelectedActivityInWorkshop));
                RemoveActivityFromWorkshopCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private Material? _selectedMaterial;
    public Material? SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            if (SetProperty(ref _selectedMaterial, value))
            {
                OnPropertyChanged(nameof(SelectedMaterial));
                RemoveMaterialFromWorkshopCommand.RaiseCanExecuteChanged();
            }
        }
    }
    private Material? _selectedMaterialInWorkshop;
    public Material? SelectedMaterialInWorkshop
    {
        get => _selectedMaterialInWorkshop;
        set
        {
            if (SetProperty(ref _selectedMaterialInWorkshop, value))
            {
                OnPropertyChanged(nameof(SelectedMaterialInWorkshop));
                RemoveMaterialFromWorkshopCommand.RaiseCanExecuteChanged();
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
    #region Button visibility properties
    // Button visibility properties
    private Visibility _buttonAddVisibility = Visibility.Collapsed;
    public Visibility ButtonAddVisibility
    {
        get => _buttonAddVisibility;
        set => SetProperty(ref _buttonAddVisibility, value);
    }
    private Visibility _buttonRemoveVisibility = Visibility.Collapsed;
    public Visibility ButtonRemoveVisibility
    {
        get => _buttonRemoveVisibility;
        set => SetProperty(ref _buttonRemoveVisibility, value);
    }
    private Visibility _buttonUpdateVisibility = Visibility.Collapsed;
    public Visibility ButtonUpdateVisibility
    {
        get => _buttonUpdateVisibility;
        set => SetProperty(ref _buttonUpdateVisibility, value);
    }
    private Visibility _buttonRemoveMaterialFromWorkshopVisibility = Visibility.Visible;
    public Visibility ButtonRemoveMaterialFromWorkshopVisibility
    {
        get => _buttonRemoveMaterialFromWorkshopVisibility;
        set => SetProperty(ref _buttonRemoveMaterialFromWorkshopVisibility, value);
    }
    private Visibility _buttonAddToMaterialInWorkshopVisibility = Visibility.Visible;
    public Visibility ButtonAddToMaterialInWorkshopVisibility
    {
        get => _buttonAddToMaterialInWorkshopVisibility;
        set => SetProperty(ref _buttonAddToMaterialInWorkshopVisibility, value);
    }
    private Visibility _buttonRemoveFromActivityInWorkshopVisibility = Visibility.Visible;
    public Visibility ButtonRemoveToMaterialInWorkshopVisibility
    {
        get => _buttonRemoveFromActivityInWorkshopVisibility;
        set => SetProperty(ref _buttonRemoveFromActivityInWorkshopVisibility, value);
    }
    private Visibility _buttonAddActivityToWorkshopVisibility = Visibility.Visible;
    public Visibility ButtonAddActivityToWorkshopVisibility
    {
        get => _buttonAddActivityToWorkshopVisibility;
        set => SetProperty(ref _buttonAddActivityToWorkshopVisibility, value);
    }
    #endregion
    #region Form visibility properties
    private Visibility _formActivityInWorkshopVisibility = Visibility.Hidden;
    public Visibility FormActivityInWorkshopVisibility
    {
        get => _formActivityInWorkshopVisibility;
        set => SetProperty(ref _formActivityInWorkshopVisibility, value);
    }
    private Visibility _formActivityVisibility = Visibility.Hidden;
    public Visibility FormActivityVisibility
    {
        get => _formActivityVisibility;
        set => SetProperty(ref _formActivityVisibility, value);
    }
    private Visibility _formMaterialInWorkshopVisibility = Visibility.Hidden;
    public Visibility FormMaterialInWorkshopVisibility
    {
        get => _formMaterialInWorkshopVisibility;
        set => SetProperty(ref _formMaterialInWorkshopVisibility, value);
    }

    private Visibility _formMaterialVisibility = Visibility.Hidden;
    public Visibility FormMaterialVisibility
    {
        get => _formMaterialVisibility;
        set => SetProperty(ref _formMaterialVisibility, value);
    }
    #endregion
    #region Commands properties
    public RelayCommand AddWorkshopCommand { get; private set; }
    public RelayCommand RemoveWorkshopCommand { get; private set; }
    public RelayCommand UpdateWorkshopCommand { get; private set; }
    public RelayCommand AddMaterialToWorkshopCommand { get; private set; }
    public RelayCommand RemoveMaterialFromWorkshopCommand { get; private set; }
    public RelayCommand AddActivityToWorkshopCommand { get; private set; }
    public RelayCommand RemoveActivityFromWorkshopCommand { get; private set; }
    #endregion
    public WorkshopViewModel()
    {
        _workshopRepository = new WorkshopRepository();
        _materialRepository = new MaterialRepository();
        _activityRepository = new ActivityRepository();

        Workshops = new ObservableCollection<Workshop>(_workshopRepository.GetAll());
        MaterialsAvailable = new ObservableCollection<Material>(_materialRepository.GetAll());
        ActivitiesAvailable = new ObservableCollection<Activity>(_activityRepository.GetAll()); // skal have ActivitiesInWorkshop inhold fjernet
        ActivitiesInWorkshop = new ObservableCollection<Activity>();

        _newWorkshopName = string.Empty;
        _newWorkshopDescription = string.Empty;

        AddWorkshopCommand = new RelayCommand(AddWorkshop, CanAddWorkshop);
        RemoveWorkshopCommand = new RelayCommand(RemoveWorkshop, CanRemoveWorkshop);
        UpdateWorkshopCommand = new RelayCommand(SaveWorkshop, CanSaveWorkshop);
        AddMaterialToWorkshopCommand = new RelayCommand(AddMaterialToWorkshop, CanAddMaterialToWorkshop);
        RemoveMaterialFromWorkshopCommand = new RelayCommand(RemoveMaterialFromWorkshop, CanRemoveMaterialFromWorkshop);
        AddActivityToWorkshopCommand = new RelayCommand(AddActivityToWorkshop, CanAddActivityToWorkshop);
        RemoveActivityFromWorkshopCommand = new RelayCommand(RemoveActivityFromWorkshop, CanRemoveActivityFromWorkshop);

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
    private void AddMaterialToWorkshop()
    {
        if (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty)
        {
            //Material material = new(NewWorkshopName, NewWorkshopDescription, 0);
            //_materialRepository.Add(material);
            //MaterialsInWorkshop?.Add(material);
            //ClearForm();
            //AddMaterialToWorkshopCommand.RaiseCanExecuteChanged();
        }
    }
    private void RemoveMaterialFromWorkshop()
    {
        //if (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty)
        //{
        //    _materialRepository.Remove(SelectedWorkshop.GUID);
        //    MaterialsInWorkshop?.Remove(SelectedWorkshop);
        //    RemoveMaterialFromWorkshopCommand.RaiseCanExecuteChanged();
        //}
    }
    private void AddActivityToWorkshop()
    {
        this.ActivitiesInWorkshop.Add(this.SelectedActivity!);
        //if (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty)
        //{
        //Activity activity = new(NewWorkshopName, NewWorkshopDescription, TimeSpan.Zero);
        //_activityRepository.Add(activity);
        //ActivitiesInWorkshop?.Add(activity);
        //ClearForm();
        //AddActivityToWorkshopCommand.RaiseCanExecuteChanged();
        //}
    }
    private void RemoveActivityFromWorkshop()
    {
        //if (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty)
        //{
        //    _activityRepository.Remove(SelectedWorkshop.GUID);
        //    ActivitiesInWorkshop?.Remove(SelectedWorkshop);
        //    RemoveActivityFromWorkshopCommand.RaiseCanExecuteChanged();
        //}
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
    private bool CanAddMaterialToWorkshop()
    {
        return SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty;
    }
    private bool CanRemoveMaterialFromWorkshop()
    {
        return SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty;
    }
    private bool CanAddActivityToWorkshop()
    {
        return SelectedActivity != null && SelectedActivity.GUID != Guid.Empty;
    }
    private bool CanRemoveActivityFromWorkshop()
    {
        return SelectedActivity != null && SelectedActivity.GUID != Guid.Empty;
    }
    #endregion
    #region Helper methods

    private void SetButtonVisibility()
    {
        ButtonAddVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Collapsed : Visibility.Visible;
        ButtonUpdateVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
        ButtonRemoveVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
    }
    private void SetFormMaterialVisibility()
    {
        var isActivitySelected = SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty;
        MaterialsInWorkshop = isActivitySelected
            ? new ObservableCollection<Material>(
                SelectedWorkshop!.MaterialGuids
                    .Select(guid => _materialRepository.Get(guid))
                    .Where(material => material != null)
                    .Cast<Material>()
              )
            : null;
        FormActivityInWorkshopVisibility = isActivitySelected ? Visibility.Visible : Visibility.Hidden;
        FormActivityVisibility = isActivitySelected ? Visibility.Visible : Visibility.Hidden;
        FormMaterialInWorkshopVisibility = isActivitySelected ? Visibility.Visible : Visibility.Hidden;
        FormMaterialVisibility = isActivitySelected ? Visibility.Visible : Visibility.Hidden;
        //RefreshMaterialInActivity();
    }

    private bool IsFormValid()
    {
        if (string.IsNullOrWhiteSpace(NewWorkshopName))
            return false;
        return true;

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
