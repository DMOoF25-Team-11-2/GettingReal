namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using GettingReal.Model;

class ActivityViewModel : ViewModelBase
{
    private readonly ActivityRepository _activityRepository;
    public ObservableCollection<Activity>? Activities { get; set; } = new ObservableCollection<Activity>();
    private Activity? _selectedActivity;
    public Activity? SelectedActivity
    {
        get => _selectedActivity;
        set
        {
            if (SetProperty(ref _selectedActivity, value))
            {
                OnPropertyChanged(nameof(SelectedActivity));
                RemoveActivityCommand.RaiseCanExecuteChanged();
                SetButtonVisibility();
                UpdateFormValue();
            }
        }
    }

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

    public RelayCommand AddActivityCommand { get; private set; }
    public RelayCommand RemoveActivityCommand { get; private set; }

    public RelayCommand UpdateActivityCommand { get; private set; }
    public ActivityViewModel()
    {
        _activityRepository = new ActivityRepository();
        Activities = new ObservableCollection<Activity>(_activityRepository.GetAll());
        _newActivityName = string.Empty;
        _newActivityDescription = string.Empty;
        AddActivityCommand = new RelayCommand(AddActivity, CanAddActivity);
        RemoveActivityCommand = new RelayCommand(RemoveActivity, CanRemoveActivity);
        UpdateActivityCommand = new RelayCommand(SaveActivity, CanSaveActivity);
        SetButtonVisibility();
    }

    private void AddActivity()
    {
        Activity newActivity = new(NewActivityName, NewActivityExpectedTime, NewActivityDescription);
        _activityRepository.Add(newActivity);
        Activities?.Add(newActivity);
        ClearForm();
        AddActivityCommand.RaiseCanExecuteChanged();
    }

    private bool CanAddActivity()
    {
        if (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty)
            return false;
        return IsFormValid();
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

    private bool CanRemoveActivity()
    {
        return SelectedActivity != null;
    }

    private void SaveActivity()
    {
        //MessageBox.Show("Test", "Test", MessageBoxButton.OK);
        SelectedActivity!.Name = NewActivityName;
        SelectedActivity.Description = NewActivityDescription;
        _activityRepository.Update(SelectedActivity);
        //workaround to refresh listview with new data
        var temp = Activities;
        Activities = null;
        OnPropertyChanged(nameof(Activities));
        Activities = temp;
        OnPropertyChanged(nameof(Activities));
    }

    private bool CanSaveActivity()
    {
        return IsFormValid();
    }

    private bool IsFormValid()
    {
        if (string.IsNullOrWhiteSpace(NewActivityName))
            return false;
        return true;

    }

    private void SetButtonVisibility()
    {
        AddButtonVisibility = (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty) ? Visibility.Collapsed : Visibility.Visible;
        UpdateButtonVisibility = (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
        RemoveButtonVisibility = (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
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
    }
}
