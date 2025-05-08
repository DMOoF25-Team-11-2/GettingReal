namespace GettingReal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using GettingReal.Model;

class MaterialViewModel : ViewModelBase
{
    private readonly MaterialRepository _materialRepository;
    public ObservableCollection<Material>? Materials { get; set; } = new ObservableCollection<Material>();
    private Material? _selectedMaterial;
    public Material? SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            if (SetProperty(ref _selectedMaterial, value))
            {
                OnPropertyChanged(nameof(SelectedMaterial));
                RemoveMaterialCommand.RaiseCanExecuteChanged();
                SetButtonVisibility();
                UpdateFormValue();
            }
        }
    }

    private string _newMaterialName;
    public string NewMaterialName
    {
        get => _newMaterialName;
        set
        {
            if (SetProperty(ref _newMaterialName, value))
            {
                AddMaterialCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private string _newMaterialDescription;
    public string NewMaterialDescription
    {
        get => _newMaterialDescription;
        set
        {
            if (SetProperty(ref _newMaterialDescription, value))
            {
                AddMaterialCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private int _newMaterialQuantity;
    public int NewMaterialQuantity
    {
        get => _newMaterialQuantity;
        set
        {
            if (SetProperty(ref _newMaterialQuantity, value))
            {
                AddMaterialCommand.RaiseCanExecuteChanged();
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

    public RelayCommand AddMaterialCommand { get; private set; }
    public RelayCommand RemoveMaterialCommand { get; private set; }

    public RelayCommand SaveMaterialCommand { get; private set; }
    public MaterialViewModel()
    {
        _materialRepository = new MaterialRepository();
        Materials = new ObservableCollection<Material>(_materialRepository.GetAll());
        _newMaterialName = string.Empty;
        _newMaterialDescription = string.Empty;
        AddMaterialCommand = new RelayCommand(AddMaterial, CanAddMaterial);
        RemoveMaterialCommand = new RelayCommand(RemoveMaterial, CanRemoveMaterial);
        SaveMaterialCommand = new RelayCommand(SaveMaterial, CanSaveMaterial);
        SetButtonVisibility();
    }

    private void AddMaterial()
    {
        Material newMaterial = new(NewMaterialName, NewMaterialDescription, NewMaterialQuantity);
        Materials?.Add(newMaterial);
        _materialRepository.Add(newMaterial);
        ClearForm();
        AddMaterialCommand.RaiseCanExecuteChanged();
    }

    private bool CanAddMaterial()
    {
        if (SelectedMaterial != null && SelectedMaterial.GUID != Guid.Empty)
            return false;
        return IsFormValid();
    }

    private void RemoveMaterial()
    {
        if (SelectedMaterial != null)
        {
            _materialRepository.Remove(SelectedMaterial.GUID);
            Materials?.Remove(SelectedMaterial);
            RemoveMaterialCommand.RaiseCanExecuteChanged();
        }
    }

    private bool CanRemoveMaterial()
    {
        return SelectedMaterial != null;
    }

    private void SaveMaterial()
    {
        //MessageBox.Show("Test", "Test", MessageBoxButton.OK);
        SelectedMaterial!.Name = NewMaterialName;
        SelectedMaterial.Description = NewMaterialDescription;
        _materialRepository.Update(SelectedMaterial);
        //workaround to refresh listview with new data
        var temp = Materials;
        Materials = null;
        OnPropertyChanged(nameof(Materials));
        Materials = temp;
        OnPropertyChanged(nameof(Materials));
    }

    private bool CanSaveMaterial()
    {
        return IsFormValid();
    }

    private bool IsFormValid()
    {
        return (!string.IsNullOrWhiteSpace(NewMaterialName) && !(NewMaterialQuantity < 0));
    }

    private void SetButtonVisibility()
    {
        if (SelectedMaterial != null && SelectedMaterial.GUID != Guid.Empty)
        {
            AddButtonVisibility = Visibility.Collapsed; // Hide the button
            UpdateButtonVisibility = Visibility.Visible; // Show the button
            RemoveButtonVisibility = Visibility.Visible; // Show the button
        }
        else
        {
            AddButtonVisibility = Visibility.Visible; // Show the button
            UpdateButtonVisibility = Visibility.Collapsed; // Hide the button
            RemoveButtonVisibility = Visibility.Collapsed; // Hide the button
        }
    }
    private void UpdateFormValue()
    {
        if (SelectedMaterial != null)
        {
            NewMaterialName = SelectedMaterial.Name;
            NewMaterialDescription = SelectedMaterial.Description;
            NewMaterialQuantity = SelectedMaterial.Quantity;
        }
        else
        {
            ClearForm();
        }
    }

    private void ClearForm()
    {
        NewMaterialName = string.Empty;
        NewMaterialDescription = string.Empty;
        NewMaterialQuantity = 0;
    }
}
