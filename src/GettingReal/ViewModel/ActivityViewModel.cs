using System.Collections.ObjectModel;
namespace GettingReal.ViewModel;

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
                AddActivityCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private TimeSpan _newActivityExpectedTime;
    public TimeSpan NewActivityDescription
    {
        get => _newActivityExpectedTime;
        set
        {
            if (SetProperty(ref _newActivityExpectedTime, value))
            {
                AddActivityCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private Visibility _addButtonVisibility = Visibility.Visible;
    public Visibility AddButtonVisibility
    {
        get => _addButtonVisibility;
        set => SetProperty(ref _addButtonVisibility, value);
    }

    private Visibility _removeButtonVisibility = Visibility.Visible;
    public Visibility RemoveButtonVisibility
    {
        get => _removeButtonVisibility;
        set => SetProperty(ref _removeButtonVisibility, value);
    }

    private Visibility _saveButtonVisibility = Visibility.Collapsed;
    public Visibility SaveButtonVisibility
    {
        get => _saveButtonVisibility;
        set => SetProperty(ref _saveButtonVisibility, value);
    }

    public RelayCommand AddActivityCommand { get; private set; }
    public RelayCommand RemoveActivityCommand { get; private set; }

    public RelayCommand UpdateActivityCommand { get; private set; }
    public ActivityViewModel()
    {
        _activityRepository = new ActivityRepository();
        Activities = new ObservableCollection<Activity>(_activityRepository.GetAll());
        _newActivityName = string.Empty;
        _newActivityExpectedTime = TimeSpan.Zero;
        AddActivityCommand = new RelayCommand(AddActivity, CanAddActivity);
        RemoveActivityCommand = new RelayCommand(RemoveActivity, CanRemoveActivity);
        UpdateActivityCommand = new RelayCommand(UpdateActivity, CanUpdateActivity);
    }

    private void AddActivity()
    {
        if (SelectedActivity != null)
            throw new InvalidOperationException("Cannot add a new activity while one is selected.");
        if (string.IsNullOrEmpty(NewActivityName))
            throw new InvalidOperationException("Cannot add a new activity with an empty name.");

        Activity newActivity = new Activity(NewActivityName, NewActivityDescription);
        Activities?.Add(newActivity);
        _activityRepository.Add(newActivity);
        ClearForm();
        AddActivityCommand.RaiseCanExecuteChanged();
    }

    private bool CanAddActivity()
    {
        if (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty)
        {
            return false;
        }
        // Check if the activity name and description are not empty and if the selected activity is not already in the list
        return IsFormValid();
    }

    private void RemoveActivity()
    {
        if (SelectedActivity != null)
        {
            _activityRepository.Remove(SelectedActivity.GUID);
            Activities.Remove(SelectedActivity);
            ClearForm();
            RemoveActivityCommand.RaiseCanExecuteChanged();
        }
    }

    private bool CanRemoveActivity()
    {
        return SelectedActivity != null && SelectedActivity.GUID != Guid.Empty;
    }

    private void UpdateActivity()
    {
        if (SelectedActivity == null)
            throw new InvalidOperationException("Der er ikke valgt nogen aktiviteter i listen!");
        if (string.IsNullOrEmpty(NewActivityName))
            throw new InvalidOperationException("Aktivitetsnavn må ikke være tomt!");
        SelectedActivity!.Name = NewActivityName;
        SelectedActivity.ExpectedTime = NewActivityDescription;
        _activityRepository.Update(SelectedActivity);
        //workaround to refresh listview with new data
        var temp = Activities;
        Activities = null;
        OnPropertyChanged(nameof(Activities));
        Activities = temp;
        OnPropertyChanged(nameof(Activities));
        //UpdateActivityCommand.RaiseCanExecuteChanged();

    }

    private bool CanUpdateActivity()
    {
        return IsFormValid();
    }

    private bool IsFormValid()
    {
        return (!string.IsNullOrWhiteSpace(NewActivityName));
    }

    private void SetButtonVisibility()
    {
        if (SelectedActivity != null && SelectedActivity.GUID != Guid.Empty)
        {
            AddButtonVisibility = Visibility.Collapsed; // Hide the button
            SaveButtonVisibility = Visibility.Visible; // Show the button
        }
        else
        {
            AddButtonVisibility = Visibility.Visible; // Show the button
            SaveButtonVisibility = Visibility.Collapsed; // Hide the button
        }
    }
    private void UpdateFormValue()
    {
        if (SelectedActivity != null)
        {
            NewActivityName = SelectedActivity.Name;
            NewActivityDescription = SelectedActivity.ExpectedTime;
        }
        else
        {
            ClearForm();
        }
    }
    private void ClearForm()
    {
        NewActivityName = string.Empty;
        NewActivityDescription = TimeSpan.Zero;
    }
}
