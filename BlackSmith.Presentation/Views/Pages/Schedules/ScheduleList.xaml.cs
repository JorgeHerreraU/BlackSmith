using BlackSmith.Presentation.ViewModels.Schedules;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Schedules;

/// <summary>
///     Interaction logic for ScheduleList.xaml
/// </summary>
public partial class ScheduleList : Page
{
    public ScheduleList(ScheduleListViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
