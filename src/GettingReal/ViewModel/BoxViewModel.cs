namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using GettingReal.Model;

class BoxViewModel : ViewModelBase
{
    #region Properties
    private readonly BoxRepository _boxRepository;
    private readonly MaterialRepository _materialRepository;
    #endregion
    #region Observable Collections properties
    public ObservableCollection<Box>? Boxes { get; set; }
    public ObservableCollection<Material>? MaterialsAvailable { get; set; }
    public ObservableCollection<Material>? MaterialsInBox { get; set; }
    #endregion
    #region Selected properties
    private Box? _selectedBox;
    public Box? SelectedBox
    {
        get => _selectedBox;
        set
        {
            if (SetProperty(ref _selectedBox, value))
            {
                if (_selectedBox != null)
                    foreach (var guids in SelectedBox!.MaterialGuids)
                    {
                        var material = MaterialsAvailable?.FirstOrDefault(m => m.GUID == guids);
                        if (material != null)
                        {
                            MaterialsInBox?.Add(material);
                        }
                    }
                OnPropertyChanged(nameof(SelectedBox));
                RemoveBoxCommand.RaiseCanExecuteChanged();
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
                RemoveMaterialInBoxCommand.RaiseCanExecuteChanged();
            }
        }
    }
    #endregion
    #region Form properties
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
    #endregion
    #region Button visibility properties
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

    private Visibility _removeButtonFromMaterialInBoxVisibility = Visibility.Visible;
    public Visibility RemoveButtonFromMaterialInBoxVisibility
    {
        get => _removeButtonFromMaterialInBoxVisibility;
        set => SetProperty(ref _removeButtonFromMaterialInBoxVisibility, value);
    }

    private Visibility _addButtonToMaterialInBoxVisibility = Visibility.Visible;
    public Visibility AddButtonToMaterialInBoxVisibility
    {
        get => _addButtonToMaterialInBoxVisibility;
        set => SetProperty(ref _addButtonToMaterialInBoxVisibility, value);
    }
    #endregion
    #region Form visibility properties
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
    #endregion
    #region Command properties
    public RelayCommand AddBoxCommand { get; private set; }
    public RelayCommand RemoveBoxCommand { get; private set; }
    public RelayCommand UpdateBoxCommand { get; private set; }
    public RelayCommand AddMaterialInBoxCommand { get; private set; }
    public RelayCommand RemoveMaterialInBoxCommand { get; private set; }
    #endregion
    public BoxViewModel()
    {
        _boxRepository = new BoxRepository();
        _materialRepository = new MaterialRepository();

        Boxes = new ObservableCollection<Box>(_boxRepository.GetAll());
        MaterialsAvailable = new ObservableCollection<Material>(_materialRepository.GetAll());
        // In the materials available list, remove materials that are already in boxes
        if (Boxes != null && MaterialsAvailable != null)
        {
            var guidsToRemove = Boxes
                .Where(box => box.MaterialGuids != null)
                .SelectMany(box => box.MaterialGuids)
                .ToHashSet();

            var materialsToRemove = MaterialsAvailable
                .Where(material => guidsToRemove.Contains(material.GUID))
                .ToList();

            foreach (var material in materialsToRemove)
                MaterialsAvailable.Remove(material);
        }

        _newBoxName = string.Empty;
        _newBoxDescription = string.Empty;

        AddBoxCommand = new RelayCommand(AddBox, CanAddBox);
        RemoveBoxCommand = new RelayCommand(RemoveBox, CanRemoveBox);
        UpdateBoxCommand = new RelayCommand(SaveBox, CanSaveBox);
        AddMaterialInBoxCommand = new RelayCommand(AddMaterialInBox, CanAddInFormMaterialInBox);
        RemoveMaterialInBoxCommand = new RelayCommand(RemoveMaterialInBox, CanRemoveInFormMaterialInBox);

        SetButtonVisibility();
    }

    #region Button Actions
    private void AddBox()
    {
        var newBox = new Box(NewBoxName, NewBoxDescription);
        Boxes?.Add(newBox);
        _boxRepository.Add(newBox);
        ClearForm();
    }

    private void RemoveBox()
    {
        if (SelectedBox != null)
        {
            _boxRepository.Remove(SelectedBox.GUID);
            Boxes?.Remove(SelectedBox);
        }
    }

    private void SaveBox()
    {
        if (SelectedBox != null)
        {
            SelectedBox.Name = NewBoxName;
            SelectedBox.Description = NewBoxDescription;
            _boxRepository.Update(SelectedBox);
            RefreshBoxes();
        }
    }

    private void AddMaterialInBox()
    {
        if (SelectedMaterial != null && SelectedBox != null)
        {
            SelectedBox.MaterialGuids.Add(SelectedMaterial.GUID);
            MaterialsInBox?.Add(SelectedMaterial);
            MaterialsAvailable?.Remove(SelectedMaterial);
            _boxRepository.Update(SelectedBox);
            RefreshMaterialInBox();
        }
    }

    private void RemoveMaterialInBox()
    {
        if (SelectedMaterialInBox != null && SelectedBox != null)
        {
            SelectedBox.MaterialGuids.Remove(SelectedMaterialInBox.GUID);
            MaterialsAvailable?.Add(SelectedMaterialInBox);
            MaterialsInBox?.Remove(SelectedMaterialInBox);
            _boxRepository.Update(SelectedBox);
        }
    }
    #endregion

    #region Button Conditions
    private bool CanAddBox() => IsFormValid();
    private bool CanRemoveBox() => SelectedBox != null;
    private bool CanSaveBox() => IsFormValid();
    private bool CanRemoveInFormMaterialInBox() => SelectedMaterialInBox != null;
    private bool CanAddInFormMaterialInBox() => SelectedMaterial != null && SelectedMaterial.GUID != Guid.Empty;
    #endregion

    #region Helpers
    private void SetButtonVisibility()
    {
        bool isBoxSelected = SelectedBox != null && SelectedBox.GUID != Guid.Empty;
        AddButtonVisibility = isBoxSelected ? Visibility.Collapsed : Visibility.Visible;
        UpdateButtonVisibility = isBoxSelected ? Visibility.Visible : Visibility.Collapsed;
        RemoveButtonVisibility = isBoxSelected ? Visibility.Visible : Visibility.Collapsed;
    }

    private void SetFormMaterialVisibility()
    {
        var isBoxSelected = SelectedBox != null && SelectedBox.GUID != Guid.Empty;
        MaterialsInBox = isBoxSelected
            ? new ObservableCollection<Material>(
                SelectedBox!.MaterialGuids
                    .Select(guid => _materialRepository.Get(guid))
                    .Where(material => material != null)
                    .Cast<Material>()
              )
            : null;
        FormMaterialInBoxVisibility = isBoxSelected ? Visibility.Visible : Visibility.Hidden;
        FormMaterialVisibility = isBoxSelected ? Visibility.Visible : Visibility.Hidden;
        RefreshMaterialInBox();
    }

    private bool IsFormValid() => !string.IsNullOrWhiteSpace(NewBoxName);
    private void UpdateFormValue()
    {
        if (SelectedBox != null)
        {
            NewBoxName = SelectedBox.Name;
            NewBoxDescription = SelectedBox.Description;
            RefreshMaterialInBox();
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
        var temp = MaterialsInBox;
        MaterialsInBox = null;
        OnPropertyChanged(nameof(MaterialsInBox));
        MaterialsInBox = temp;
        OnPropertyChanged(nameof(MaterialsInBox));
    }

    private void RefreshBoxes()
    {
        var temp = Boxes;
        Boxes = null;
        OnPropertyChanged(nameof(Boxes));
        Boxes = temp;
        OnPropertyChanged(nameof(Boxes));
    }
    #endregion
}
