namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using GettingReal.Model;

class ActivityViewModel : ViewModelBase
{
    // Repositories
    private readonly ActivityRepository _activityRepository;
    private readonly MaterialRepository _materialRepository;

    // Observable Collections
    public ObservableCollection<Activity>? Activities { get; set; }
    public ObservableCollection<Material>? MaterialsAvailable { get; set; }
    public ObservableCollection<Material>? MaterialsInActivity { get; set; }

    // Selected properties
    private Activity? _selectedActivity;
    public Activity? SelectedActivity
    {
        get => _selectedActivity;
        set
        {
            if (SetProperty(ref _selectedActivity, value))
            {
                if (_selectedActivity != null)
                    foreach (var guids in _selectedActivity!.MaterialGuids)
                    {
                        var material = MaterialsAvailable?.FirstOrDefault(m => m.GUID == guids);
                        if (material != null)
                        {
                            MaterialsInActivity?.Add(material);
                        }
                    }
                OnPropertyChanged(nameof(SelectedActivity));
                RemoveActivityCommand.RaiseCanExecuteChanged();
                SetButtonVisibility();
                SetFormMaterialVisibility();
                UpdateFormValue();
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
                AddMaterialInActivityCommand.RaiseCanExecuteChanged();
                SetButtonVisibility();
            }
        }
    }
    private Material? _selectedMaterialInActivity;
    public Material? SelectedMaterialInActivity
    {
        get => _selectedMaterialInActivity;
        set
        {
            if (SetProperty(ref _selectedMaterialInActivity, value))
            {
                OnPropertyChanged(nameof(SelectedMaterialInActivity));
                RemoveMaterialInActivityCommand.RaiseCanExecuteChanged();
            }
        }
    }

    // Form fields for new activity
    private string _newActivityName;
    public string NewActivityName
    {
        get => _newActivityName;
        set
        {
            if (SetProperty(ref _newActivityName, value))
            {
                UpdateActivityCommand.RaiseCanExecuteChanged();
                AddActivityCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private TimeSpan _newActivityExpectedTime;
    public TimeSpan NewActivityExpectedTime
    {
        get => _newActivityExpectedTime;
        set
        {
            if (SetProperty(ref _newActivityExpectedTime, value))
            {
                UpdateActivityCommand.RaiseCanExecuteChanged();
                AddActivityCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private string _newActivityDescription;
    public string NewActivityDescription
    {
        get => _newActivityDescription;
        set
        {
            if (SetProperty(ref _newActivityDescription, value))
            {
                UpdateActivityCommand.RaiseCanExecuteChanged();
                AddActivityCommand.RaiseCanExecuteChanged();
            }
        }
    }

    // Visibility properties for buttons
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

    private Visibility _removeButtonFromMaterialRepoVisibility = Visibility.Visible;
    public Visibility RemoveButtonFromMaterialInActivityVisibility
    {
        get => _removeButtonFromMaterialRepoVisibility;
        set => SetProperty(ref _removeButtonFromMaterialRepoVisibility, value);
    }

    private Visibility _addButtonToMaterialInActivityVisibility = Visibility.Visible;
    public Visibility AddButtonToMaterialInActivityVisibility
    {
        get => _addButtonToMaterialInActivityVisibility;
        set => SetProperty(ref _addButtonToMaterialInActivityVisibility, value);
    }

    // Visibility properties for material form
    private Visibility _formMaterialInActivityVisibility = Visibility.Hidden;
    public Visibility FormMaterialInActivityVisibility
    {
        get => _formMaterialInActivityVisibility;
        set => SetProperty(ref _formMaterialInActivityVisibility, value);
    }

    private Visibility _formMaterialVisibility = Visibility.Hidden;
    public Visibility FormMaterialVisibility
    {
        get => _formMaterialVisibility;
        set => SetProperty(ref _formMaterialVisibility, value);
    }

    public RelayCommand AddActivityCommand { get; private set; }
    public RelayCommand RemoveActivityCommand { get; private set; }

    public RelayCommand UpdateActivityCommand { get; private set; }
    public RelayCommand AddMaterialInActivityCommand { get; private set; }
    public RelayCommand RemoveMaterialInActivityCommand { get; private set; }

    public ActivityViewModel()
    {
        _activityRepository = new ActivityRepository();
        _materialRepository = new MaterialRepository();

        Activities = new ObservableCollection<Activity>(_activityRepository.GetAll());
        MaterialsAvailable = new ObservableCollection<Material>(_materialRepository.GetAll());

        _newActivityName = string.Empty;
        _newActivityDescription = string.Empty;

        AddActivityCommand = new RelayCommand(AddActivity, CanAddActivity);
        RemoveActivityCommand = new RelayCommand(RemoveActivity, CanRemoveActivity);
        UpdateActivityCommand = new RelayCommand(SaveActivity, CanSaveActivity);
        AddMaterialInActivityCommand = new RelayCommand(AddMaterialInActivity, CanAddInFormMaterialInActivity);
        RemoveMaterialInActivityCommand = new RelayCommand(RemoveMaterialInActivity, CanRemoveInFormMaterialInActivity);

        SetButtonVisibility();
    }

    #region Button Actions
    private void AddActivity()
    {
        Activity newActivity = new(NewActivityName, NewActivityExpectedTime, NewActivityDescription);
        _activityRepository.Add(newActivity);
        Activities?.Add(newActivity);
        ClearForm();
        AddActivityCommand.RaiseCanExecuteChanged();
    }
    private void SaveActivity()
    {
        //MessageBox.Show("Test", "Test", MessageBoxButton.OK);
        SelectedActivity!.Name = NewActivityName;
        SelectedActivity.Description = NewActivityDescription;
        SelectedActivity.ExpectedTime = NewActivityExpectedTime;
        _activityRepository.Update(SelectedActivity);
        //workaround to refresh listview with new data
        var temp = Activities;
        Activities = null;
        OnPropertyChanged(nameof(Activities));
        Activities = temp;
        OnPropertyChanged(nameof(Activities));
    }

    private void RemoveActivity()
    {
        if (SelectedActivity != null)
        {
            _activityRepository.Remove(SelectedActivity.GUID);
            Activities?.Remove(SelectedActivity);
            RemoveActivityCommand.RaiseCanExecuteChanged();
        }
    }

    private void AddMaterialInActivity()
    {
        if (SelectedMaterial != null && SelectedActivity != null)
        {
            SelectedActivity.MaterialGuids.Add(SelectedMaterial.GUID);
            MaterialsInActivity?.Add(SelectedMaterial);
            _activityRepository.Update(SelectedActivity);
            RefreshMaterialInActivity();
        }
    }

    private void RemoveMaterialInActivity()
    {
        if (SelectedMaterialInActivity != null && SelectedActivity != null)
        {
            SelectedActivity.MaterialGuids.Remove(SelectedMaterialInActivity.GUID);
            MaterialsInActivity?.Remove(SelectedMaterialInActivity);
            _activityRepository.Update(SelectedActivity);
        }
    }
    #endregion
    #region Button Conditions
    private bool CanAddActivity()
    {
        if (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty)
            return false;
        return IsFormValid();
    }
    private bool CanRemoveActivity()
    {
        return SelectedActivity != null;
    }

    private bool CanSaveActivity()
    {
        return IsFormValid();
    }
    private bool CanRemoveInFormMaterialInActivity() => SelectedMaterialInActivity != null;
    private bool CanAddInFormMaterialInActivity() => SelectedMaterial != null && SelectedMaterial.GUID != Guid.Empty;
    #endregion
    #region Helpers

    private void SetButtonVisibility()
    {
        AddButtonVisibility = (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty) ? Visibility.Collapsed : Visibility.Visible;
        UpdateButtonVisibility = (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
        RemoveButtonVisibility = (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void SetFormMaterialVisibility()
    {
        var isActivitySelected = SelectedActivity != null && SelectedActivity.GUID != Guid.Empty;
        MaterialsInActivity = isActivitySelected
            ? new ObservableCollection<Material>(
                SelectedActivity!.MaterialGuids
                    .Select(guid => _materialRepository.Get(guid))
                    .Where(material => material != null)
                    .Cast<Material>()
              )
            : null;
        FormMaterialInActivityVisibility = isActivitySelected ? Visibility.Visible : Visibility.Hidden;
        FormMaterialVisibility = isActivitySelected ? Visibility.Visible : Visibility.Hidden;
        RefreshMaterialInActivity();
    }

    private bool IsFormValid()
    {
        if (string.IsNullOrWhiteSpace(NewActivityName))
            return false;
        return true;

    }
    private void UpdateFormValue()
    {
        if (SelectedActivity != null)
        {
            NewActivityName = SelectedActivity.Name;
            NewActivityDescription = SelectedActivity.Description;
        }
        else
        {
            ClearForm();
        }
    }
    private void ClearForm()
    {
        NewActivityName = string.Empty;
        NewActivityDescription = string.Empty;
        NewActivityExpectedTime = TimeSpan.Zero;
    }

    private void RefreshMaterialInActivity()
    {
        var temp = MaterialsInActivity;
        MaterialsInActivity = null;
        OnPropertyChanged(nameof(MaterialsInActivity));
        MaterialsInActivity = temp;
        OnPropertyChanged(nameof(MaterialsInActivity));
    }

    private void RefreshActivities()
    {
        var temp = Activities;
        Activities = null;
        OnPropertyChanged(nameof(Activities));
        Activities = temp;
        OnPropertyChanged(nameof(Activities));
    }
}
#endregion
