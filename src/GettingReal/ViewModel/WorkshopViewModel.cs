namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using GettingReal.Model;

class WorkshopViewModel : ViewModelBase
{
    private readonly WorkshopRepository _workshopRepository;
    public ObservableCollection<Workshop>? Workshops { get; set; } = new ObservableCollection<Workshop>();
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

    public RelayCommand AddWorkshopCommand { get; private set; }
    public RelayCommand RemoveWorkshopCommand { get; private set; }

    public RelayCommand UpdateWorkshopCommand { get; private set; }
    public WorkshopViewModel()
    {
        _workshopRepository = new WorkshopRepository();
        Workshops = new ObservableCollection<Workshop>(_workshopRepository.GetAll());
        _newWorkshopName = string.Empty;
        _newWorkshopDescription = string.Empty;
        AddWorkshopCommand = new RelayCommand(AddWorkshop, CanAddWorkshop);
        RemoveWorkshopCommand = new RelayCommand(RemoveWorkshop, CanRemoveWorkshop);
        UpdateWorkshopCommand = new RelayCommand(SaveWorkshop, CanSaveWorkshop);
        SetButtonVisibility();
    }

    private void AddWorkshop()
    {
        Workshop newWorkshop = new(NewWorkshopName, NewWorkshopDescription);
        _workshopRepository.Add(newWorkshop);
        Workshops?.Add(newWorkshop);
        ClearForm();
        AddWorkshopCommand.RaiseCanExecuteChanged();
    }

    private bool CanAddWorkshop()
    {
        if (SelectedWorkshop != null && SelectedWorkshop.GUID != Guid.Empty)
            return false;
        return IsFormValid();
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

    private bool CanRemoveWorkshop()
    {
        return SelectedWorkshop != null;
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

    private bool CanSaveWorkshop()
    {
        return IsFormValid();
    }

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
}
