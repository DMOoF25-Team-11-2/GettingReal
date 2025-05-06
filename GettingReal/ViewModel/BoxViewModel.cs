using System.Collections.ObjectModel;
namespace GettingReal.ViewModel;
using GettingReal.Model;

class BoxViewModel : ViewModelBase
{
    private readonly BoxRepository _boxRepository;

    public ObservableCollection<Box> Boxes { get; set; } = new ObservableCollection<Box>();

    public BoxViewModel()
    {
        _boxRepository = new BoxRepository();
        Boxes = new ObservableCollection<Box>(_boxRepository.GetAll());
    }

    private Box _selectedBox;
    public Box SelectedBox
    {
        get => _selectedBox;
        set
        {
            if (SetProperty(ref _selectedBox, value))
            {
                OnPropertyChanged(nameof(SelectedBox));
                RemoveBoxCommand.RaiseCanExecuteChanged();
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

    private RelayCommand _addBoxCommand;
    public RelayCommand AddBoxCommand
    {
        get
        {
            return _addBoxCommand ??= new RelayCommand(AddBox, CanAddBox);
        }
    }

    private void AddBox()
    {
        var newBox = new Box(NewBoxName, NewBoxDescription);
        Boxes.Add(newBox);
        _boxRepository.Add(newBox);
        NewBoxName = string.Empty;
        NewBoxDescription = string.Empty;
        AddBoxCommand.RaiseCanExecuteChanged();
    }

    private bool CanAddBox()
    {
        return !string.IsNullOrWhiteSpace(NewBoxName) && !string.IsNullOrWhiteSpace(NewBoxDescription);
    }

    private RelayCommand _removeBoxCommand;
    public RelayCommand RemoveBoxCommand
    {
        get
        {
            return _removeBoxCommand ??= new RelayCommand(RemoveBox, CanRemoveBox);
        }
    }

    private void RemoveBox()
    {
        if (SelectedBox != null)
        {
            Boxes.Remove(SelectedBox);
            _boxRepository.Remove(SelectedBox);
            SelectedBox = null;
            RemoveBoxCommand.RaiseCanExecuteChanged();
        }
    }

    private bool CanRemoveBox()
    {
        return SelectedBox != null;
    }
}
