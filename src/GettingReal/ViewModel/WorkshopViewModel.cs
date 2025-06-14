﻿namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GettingReal.Model;

/// <summary>
/// ViewModel for managing workshops.
/// </summary>
class WorkshopViewModel : ViewModelBase
{
    #region Properties
    private readonly WorkshopRepository _workshopRepository;
    private readonly ActivityRepository _activityRepository;
    #endregion

    #region Observable Collections properties
    public ObservableCollection<Workshop>? Workshops { get; set; }
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
            SetProperty(ref _selectedWorkshop, value);
            if (_selectedWorkshop != null)
            {
                OnPropertyChanged(nameof(SelectedWorkshop));
                RemoveWorkshopCommand.RaiseCanExecuteChanged();
                SetButtonVisibility();
                SetFormActivityVisibility();
                UpdateFormValue();
            }
            else
            {
                ClearForm();
                SetButtonVisibility();
                FormActivityInWorkshopVisibility = Visibility.Hidden;
                FormActivityVisibility = Visibility.Hidden;
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
                if (_selectedWorkshop != null)
                {
                    // Find the workshop in the repository
                    var workshop = _workshopRepository.Get(_selectedWorkshop.GUID);
                    if (workshop != null)
                    {
                        // Get the activities for this workshop
                        ActivitiesInWorkshop = new ObservableCollection<Activity>(
                            workshop.ActivityGuids
                                .Select(guid => _activityRepository.Get(guid))
                                .Where(activity => activity != null)
                                .Cast<Activity>()
                        );
                        OnPropertyChanged(nameof(ActivitiesInWorkshop));
                    }
                }
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
                if (_selectedActivityInWorkshop != null)
                {
                    // Find the activity in the repository
                    var activity = _activityRepository.Get(_selectedActivityInWorkshop.GUID);
                    if (activity != null)
                    {
                        // Get the activities for this workshop
                        ActivitiesInWorkshop = new ObservableCollection<Activity>(
                            _selectedWorkshop!.ActivityGuids
                                .Select(guid => _activityRepository.Get(guid))
                                .Where(activity => activity != null)
                                .Cast<Activity>()
                        );
                        OnPropertyChanged(nameof(ActivitiesInWorkshop));
                    }
                }
                OnPropertyChanged(nameof(SelectedActivityInWorkshop));
                RemoveActivityFromWorkshopCommand.RaiseCanExecuteChanged();
            }
        }
    }

    #endregion
    #region  Form fields for new workshop
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
    //private Visibility _buttonRemoveMaterialFromWorkshopVisibility = Visibility.Visible;
    //public Visibility ButtonRemoveMaterialFromWorkshopVisibility
    //{
    //    get => _buttonRemoveMaterialFromWorkshopVisibility;
    //    set => SetProperty(ref _buttonRemoveMaterialFromWorkshopVisibility, value);
    //}
    //private Visibility _buttonAddToMaterialInWorkshopVisibility = Visibility.Visible;
    //public Visibility ButtonAddToMaterialInWorkshopVisibility
    //{
    //    get => _buttonAddToMaterialInWorkshopVisibility;
    //    set => SetProperty(ref _buttonAddToMaterialInWorkshopVisibility, value);
    //}
    private Visibility _buttonRemoveFromActivityInWorkshopVisibility = Visibility.Visible;
    public Visibility ButtonRemoveFromActivityInWorkshopVisibility
    {
        get => _buttonRemoveFromActivityInWorkshopVisibility;
        set => SetProperty(ref _buttonRemoveFromActivityInWorkshopVisibility, value);
    }
    private Visibility _buttonAddActivityInWorkshopVisibility = Visibility.Visible;
    public Visibility ButtonAddActivityInWorkshopVisibility
    {
        get => _buttonAddActivityInWorkshopVisibility;
        set => SetProperty(ref _buttonAddActivityInWorkshopVisibility, value);
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
    #endregion
    #region Commands properties
    public RelayCommand AddWorkshopCommand { get; private set; }
    public RelayCommand RemoveWorkshopCommand { get; private set; }
    public RelayCommand UpdateWorkshopCommand { get; private set; }
    public RelayCommand AddActivityToWorkshopCommand { get; private set; }
    public RelayCommand RemoveActivityFromWorkshopCommand { get; private set; }
    #endregion
    public WorkshopViewModel()
    {
        _workshopRepository = new WorkshopRepository();
        //_materialRepository = new MaterialRepository();
        _activityRepository = new ActivityRepository();

        Workshops = [.. _workshopRepository.GetAll()];
        //MaterialsAvailable = new ObservableCollection<Material>(_materialRepository.GetAll());
        ActivitiesAvailable = [.. _activityRepository.GetAll()];

        _newWorkshopName = string.Empty;
        _newWorkshopDescription = string.Empty;

        AddWorkshopCommand = new RelayCommand(AddWorkshop, CanAddWorkshop);
        RemoveWorkshopCommand = new RelayCommand(RemoveWorkshop, CanRemoveWorkshop);
        UpdateWorkshopCommand = new RelayCommand(SaveWorkshop, CanSaveWorkshop);
        //AddMaterialToWorkshopCommand = new RelayCommand(AddMaterialToWorkshop, CanAddMaterialToWorkshop);
        //RemoveMaterialFromWorkshopCommand = new RelayCommand(RemoveMaterialFromWorkshop, CanRemoveMaterialFromWorkshop);
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
    private void AddActivityToWorkshop()
    {
        if (SelectedActivity != null)
        {
            SelectedWorkshop?.ActivityGuids.Add(SelectedActivity.GUID);
            ActivitiesAvailable?.Remove(SelectedActivity);
            _workshopRepository.Update(SelectedWorkshop!);
        }
    }
    private void RemoveActivityFromWorkshop()
    {
        if (SelectedActivityInWorkshop != null)
        {
            SelectedWorkshop?.ActivityGuids.Remove(SelectedActivityInWorkshop.GUID);
            ActivitiesAvailable?.Add(SelectedActivityInWorkshop);
            ActivitiesInWorkshop?.Remove(SelectedActivityInWorkshop);
            _workshopRepository.Update(SelectedWorkshop!);
        }
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
    private bool CanAddActivityToWorkshop()
    {
        return SelectedActivity != null && SelectedActivity.GUID != Guid.Empty;
    }
    private bool CanRemoveActivityFromWorkshop()
    {
        return SelectedActivityInWorkshop != null && SelectedActivityInWorkshop.GUID != Guid.Empty;
    }
    #endregion
    #region Helper methods
    private void SetButtonVisibility()
    {
        ButtonAddVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Collapsed : Visibility.Visible;
        ButtonUpdateVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
        ButtonRemoveVisibility = (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
    }
    private void SetFormActivityVisibility()
    {
        var isActivitySelected = SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty;
        FormActivityInWorkshopVisibility = isActivitySelected ? Visibility.Visible : Visibility.Hidden;
        FormActivityVisibility = isActivitySelected ? Visibility.Visible : Visibility.Hidden;
        RefreshActivitiesInWorkshop();
        // Refresh the list of available activities
        ActivitiesAvailable = new ObservableCollection<Activity>(
            _activityRepository.GetAll()
        );
        foreach (var activity in ActivitiesInWorkshop!)
        {
            ActivitiesAvailable?.Remove(activity);
        }
        // Notify the UI that the list of available activities has changed
        OnPropertyChanged(nameof(ActivitiesAvailable));
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
    private void RefreshActivitiesInWorkshop()
    {
        ActivitiesInWorkshop = new ObservableCollection<Activity>(
                SelectedWorkshop!.ActivityGuids
                    .Select(guid => _activityRepository.Get(guid))
                    .Where(activity => activity != null)
                    .Cast<Activity>()
              );
        OnPropertyChanged(nameof(ActivitiesInWorkshop));
    }
    #endregion
}
