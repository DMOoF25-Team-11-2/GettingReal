using System.Collections.ObjectModel;
namespace GettingReal.ViewModel;

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
                UpdateButtonVisibility();
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

    private Visibility _saveButtonVisibility = Visibility.Collapsed;
    public Visibility SaveButtonVisibility
    {
        get => _saveButtonVisibility;
        set => SetProperty(ref _saveButtonVisibility, value);
    }

    public RelayCommand AddBoxCommand { get; private set; }
    public RelayCommand RemoveBoxCommand { get; private set; }

    public RelayCommand SaveBoxCommand { get; private set; }
    public BoxViewModel()
    {
        _boxRepository = new BoxRepository();
        Boxes = new ObservableCollection<Box>(_boxRepository.GetAll());
        _newBoxName = string.Empty;
        _newBoxDescription = string.Empty;
        AddBoxCommand = new RelayCommand(AddBox, CanAddBox);
        RemoveBoxCommand = new RelayCommand(RemoveBox, CanRemoveBox);
        SaveBoxCommand = new RelayCommand(SaveBox, CanSaveBox);
    }

    private void AddBox()
    {
        if (SelectedBox != null && string.IsNullOrEmpty(SelectedBox.Name) && string.IsNullOrEmpty(SelectedBox.Description))
        {
            // Replace the placeholder with a new item
            SelectedBox.Name = NewBoxName;
            SelectedBox.Description = NewBoxDescription;

            // Add a new placeholder item
            Boxes.Add(new Box { Name = string.Empty, Description = string.Empty });

            // Clear the input fields
            NewBoxName = string.Empty;
            NewBoxDescription = string.Empty;
        }
        else
        {
            var newBox = new Box(NewBoxName, NewBoxDescription);
            Boxes.Add(newBox);
            _boxRepository.Add(newBox);
            NewBoxName = string.Empty;
            NewBoxDescription = string.Empty;
        }

        AddBoxCommand.RaiseCanExecuteChanged();
    }

    private bool CanAddBox()
    {
        if (SelectedBox != null && SelectedBox.GUID != Guid.Empty)
        {
            return false;
        }
        // Check if the box name and description are not empty and if the selected box is not already in the list
        return IsNotFormFieldNullOrSpace();
    }

    private void RemoveBox()
    {
        if (SelectedBox != null)
        {
            _boxRepository.Remove(SelectedBox.GUID);
            Boxes.Remove(SelectedBox);
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
        return IsNotFormFieldNullOrSpace();
    }

    private bool IsNotFormFieldNullOrSpace()
    {
        return !(string.IsNullOrWhiteSpace(NewBoxName) && !string.IsNullOrWhiteSpace(NewBoxDescription));
    }

    private void UpdateButtonVisibility()
    {
        if (SelectedBox != null && SelectedBox.GUID != Guid.Empty)
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
        if (SelectedBox != null)
        {
            NewBoxName = SelectedBox.Name;
            NewBoxDescription = SelectedBox.Description;
        }
        else
        {
            NewBoxName = string.Empty;
            NewBoxDescription = string.Empty;
        }
    }
}
