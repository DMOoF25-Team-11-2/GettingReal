namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using GettingReal.Model;

class BoxViewModel : ViewModelBase
{
    private readonly BoxRepository _boxRepository;
    public ObservableCollection<Box>? Boxes { get; set; } = new ObservableCollection<Box>();
    private Box? _selectedBox;
    public Box? SelectedBox
    {
        get => _selectedBox;
        set
        {
            if (SetProperty(ref _selectedBox, value))
            {
                OnPropertyChanged(nameof(SelectedBox));
                RemoveBoxCommand.RaiseCanExecuteChanged();
                SetButtonVisibility();
                UpdateFormValue();
            }
        }
    }

    private string _newBoxName;
    public string NewBoxName
    {
        get => _newBoxName;
        set
        {
            if (SetProperty(ref _newBoxName, value))
            {
                AddBoxCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private string _newBoxDescription;
    public string NewBoxDescription
    {
        get => _newBoxDescription;
        set
        {
            if (SetProperty(ref _newBoxDescription, value))
            {
                AddBoxCommand.RaiseCanExecuteChanged();
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

    private Visibility _updateButtonVisibility = Visibility.Collapsed;
    public Visibility UpdateButtonVisibility
    {
        get => _updateButtonVisibility;
        set => SetProperty(ref _updateButtonVisibility, value);
    }

    public RelayCommand AddBoxCommand { get; private set; }
    public RelayCommand RemoveBoxCommand { get; private set; }

    public RelayCommand SaveBoxCommand { get; private set; }
    public BoxViewModel()
    {
        _newBoxName = string.Empty;
        _newBoxDescription = string.Empty;
        _boxRepository = new BoxRepository();
        Boxes = new ObservableCollection<Box>(_boxRepository.GetAll());
        AddBoxCommand = new RelayCommand(AddBox, CanAddBox);
        RemoveBoxCommand = new RelayCommand(RemoveBox, CanRemoveBox);
        SaveBoxCommand = new RelayCommand(SaveBox, CanSaveBox);
        SetButtonVisibility();
    }

    private void AddBox()
    {
        Box newBox = new(NewBoxName, NewBoxDescription);
        Boxes?.Add(newBox);
        _boxRepository.Add(newBox);
        ClearForm();
        AddBoxCommand.RaiseCanExecuteChanged();
    }

    private bool CanAddBox()
    {
        if (SelectedBox != null && SelectedBox.GUID != Guid.Empty)
            return false;
        return IsFormValid();
    }

    private void RemoveBox()
    {
        if (SelectedBox != null)
        {
            _boxRepository.Remove(SelectedBox.GUID);
            Boxes?.Remove(SelectedBox);
            RemoveBoxCommand.RaiseCanExecuteChanged();
        }
    }

    private bool CanRemoveBox()
    {
        return SelectedBox != null;
    }

    private void SaveBox()
    {
        //MessageBox.Show("Test", "Test", MessageBoxButton.OK);
        SelectedBox!.Name = NewBoxName;
        SelectedBox.Description = NewBoxDescription;
        _boxRepository.Update(SelectedBox);
        //workaround to refresh listview with new data
        var temp = Boxes;
        Boxes = null;
        OnPropertyChanged(nameof(Boxes));
        Boxes = temp;
        OnPropertyChanged(nameof(Boxes));
    }

    private bool CanSaveBox()
    {
        return IsFormValid();
    }

    private bool IsFormValid()
    {
        return (!string.IsNullOrWhiteSpace(NewBoxName));
    }

    private void SetButtonVisibility()
    {
        //if (SelectedBox != null && SelectedBox.GUID != Guid.Empty)
        //{
        //    AddButtonVisibility = Visibility.Collapsed; // Hide the button
        //    UpdateButtonVisibility = Visibility.Visible; // Show the button
        //    RemoveButtonVisibility = Visibility.Visible; // Show the button
        //}
        //else
        //{
        //    AddButtonVisibility = Visibility.Visible; // Show the button
        //    UpdateButtonVisibility = Visibility.Collapsed; // Hide the button
        //    RemoveButtonVisibility = Visibility.Collapsed; // Hide the button
        //}
        AddButtonVisibility = (SelectedBox != null && SelectedBox.GUID != Guid.Empty) ? Visibility.Collapsed : Visibility.Visible;
        UpdateButtonVisibility = (SelectedBox != null && SelectedBox.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;
        RemoveButtonVisibility = (SelectedBox != null && SelectedBox.GUID != Guid.Empty) ? Visibility.Visible : Visibility.Collapsed;

    }
    private void UpdateFormValue()
    {
        if (SelectedBox != null)
        {
            NewBoxName = SelectedBox.Name;
            NewBoxDescription = SelectedBox.Description;
        }
        else
        {
            ClearForm();
        }
    }

    private void ClearForm()
    {
        NewBoxName = string.Empty;
        NewBoxDescription = string.Empty;
    }
}
