using System.Collections.ObjectModel;

namespace GettingReal.ViewModel;

using GettingReal.Model;

class ActivityViewModel : ViewModelBase
{
    private readonly ActivityRepository _activityRepository;
    public ObservableCollection<Activity> Activities { get; set; } = [];
    private Activity? _selectedActivity;

    public ActivityViewModel()
    {
        _activityRepository = new ActivityRepository();
        Activities = [.. _activityRepository.GetAll()];
    }
}
