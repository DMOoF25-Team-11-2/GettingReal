namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using GettingReal.Model;

class BoxViewModel : ViewModelBase
{
    private readonly BoxRepository _boxRepository;
    private readonly MaterialRepository _materialRepository;
    public ObservableCollection<Box>? Boxes { get; set; }
    public ObservableCollection<Material>? MaterialsAvailable { get; set; }
    public ObservableCollection<Material>? MaterialsInBox { get; set; }
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
                SetFormMaterialVisibility();
                MaterialsInBox = [.. SelectedBox?.Materials ?? []];
                RefreshMaterialInBox();
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
                AddMaterialInBoxCommand.RaiseCanExecuteChanged();
            }
        }
    }
    private Material? _selectedMaterialInBox;
    public Material? SelectedMaterialInBox
    {
        get => _selectedMaterialInBox;
        set
        {
            if (SetProperty(ref _selectedMaterialInBox, value))
            {
                OnPropertyChanged(nameof(SelectedMaterialInBox));
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
                UpdateBoxCommand.RaiseCanExecuteChanged();
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
                UpdateBoxCommand.RaiseCanExecuteChanged();
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

    private Visibility _removeButtonFromMaterialRepoVisibility = Visibility.Visible;
    public Visibility RemoveButtonFromMaterialInBoxVisibility
    {
        get => _removeButtonFromMaterialRepoVisibility;
        set => SetProperty(ref _removeButtonFromMaterialRepoVisibility, value);
    }
    private Visibility _addButtonToMaterialInBoxVisibility = Visibility.Visible;
    public Visibility AddButtonToMaterialInBoxVisibility
    {
        get => _addButtonToMaterialInBoxVisibility;
        set => SetProperty(ref _addButtonToMaterialInBoxVisibility, value);
    }

    private Visibility _formMaterialInBoxVisibility = Visibility.Hidden;
    public Visibility FormMaterialInBoxVisibility
    {
        get => _formMaterialInBoxVisibility;
        set => SetProperty(ref _formMaterialInBoxVisibility, value);
    }
    private Visibility _formMaterialVisibility = Visibility.Hidden;
    public Visibility FormMaterialVisibility
    {
        get => _formMaterialVisibility;
        set => SetProperty(ref _formMaterialVisibility, value);
    }

    public RelayCommand AddBoxCommand { get; private set; }
    public RelayCommand RemoveBoxCommand { get; private set; }
    public RelayCommand UpdateBoxCommand { get; private set; }
    public RelayCommand AddMaterialInBoxCommand { get; private set; }
    public RelayCommand RemoveMaterialInBoxCommand { get; private set; }
    public BoxViewModel()
    {
        _newBoxName = string.Empty;
        _newBoxDescription = string.Empty;
        _boxRepository = new BoxRepository();
        Boxes = new ObservableCollection<Box>(_boxRepository.GetAll());
        _materialRepository = new MaterialRepository();
        MaterialsAvailable = new ObservableCollection<Material>(_materialRepository.GetAll());
        AddBoxCommand = new RelayCommand(AddBox, CanAddBox);
        RemoveBoxCommand = new RelayCommand(RemoveBox, CanRemoveBox);
        UpdateBoxCommand = new RelayCommand(SaveBox, CanSaveBox);
        AddMaterialInBoxCommand = new RelayCommand(AddMaterialInBox, CanAddInFormMaterialInBox);
        RemoveMaterialInBoxCommand = new RelayCommand(RemoveMaterialInBox, CanRemoveInFormMaterialInBox);
        SetButtonVisibility();
    }

    #region Button action
    private void AddBox()
    {
        Box newBox = new(NewBoxName, NewBoxDescription);
        Boxes?.Add(newBox);
        _boxRepository.Add(newBox);
        ClearForm();
        AddBoxCommand.RaiseCanExecuteChanged();
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
    private void AddMaterialInBox()
    {
        if (SelectedMaterial != null && SelectedMaterial.GUID != Guid.Empty)
        {
            SelectedBox!.Materials.Add(SelectedMaterial);
            MaterialsInBox?.Add(SelectedMaterial);
            _boxRepository.Update(SelectedBox);
            RefreshMaterialInBox();
        }
    }
    private void RemoveMaterialInBox()
    {
        if (SelectedMaterialInBox != null)
        {
            MaterialsInBox?.Remove(SelectedMaterialInBox);
            SelectedBox!.Materials = [.. MaterialsInBox!];
            _boxRepository.Update(SelectedBox);
        }
    }
    #endregion
    #region Button can do
    private bool CanAddBox()
    {
        if (SelectedBox != null && SelectedBox.GUID != Guid.Empty)
            return false;
        return IsFormValid();
    }
    private bool CanRemoveBox()
    {
        return SelectedBox != null;
    }
    private bool CanSaveBox()
    {
        return IsFormValid();
    }
    private bool CanRemoveInFormMaterialInBox()
    {
        return SelectedMaterialInBox != null;
    }
    private bool CanAddInFormMaterialInBox()
    {
        return SelectedMaterial != null && SelectedMaterial.GUID != Guid.Empty;
    }
    #endregion
    #region  Button Visibility
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

    #endregion
    private void SetFormMaterialVisibility()
    {
        if (SelectedBox != null && SelectedBox.GUID != Guid.Empty)
        {
            FormMaterialInBoxVisibility = Visibility.Visible; // Show the form
            FormMaterialVisibility = Visibility.Visible; // Show the form
        }
        else
        {
            FormMaterialInBoxVisibility = Visibility.Hidden; // Hide the form
            FormMaterialVisibility = Visibility.Hidden; // Hide the form
        }
    }

    private bool IsFormValid()
    {
        return (!string.IsNullOrWhiteSpace(NewBoxName));
    }

    private void UpdateFormValue()
    {
        if (SelectedBox != null)
        {
            NewBoxName = SelectedBox.Name;
            NewBoxDescription = SelectedBox.Description;
            MaterialsInBox = new ObservableCollection<Material>(SelectedBox.Materials ?? new List<Material>());

            //OnPropertyChanged(nameof(MaterialsInBox));
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

    private void RefreshMaterialInBox()
    {
        if (SelectedBox != null)
        {
            MaterialsInBox = new ObservableCollection<Material>(SelectedBox.Materials ?? new List<Material>());
        }
        else
        {
            MaterialsInBox = new ObservableCollection<Material>();
        }
        OnPropertyChanged(nameof(MaterialsInBox));
        //workaround to refresh listview with new data
        //var temp = MaterialsInBox;
        //MaterialsInBox = null;
        //OnPropertyChanged(nameof(MaterialsInBox));
        //MaterialsInBox = temp;
        //OnPropertyChanged(nameof(MaterialsInBox));
    }
}
