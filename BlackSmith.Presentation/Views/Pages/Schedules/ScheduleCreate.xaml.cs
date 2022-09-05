using BlackSmith.Presentation.ViewModels.Schedules;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Schedules;

/// <summary>
///     Interaction logic for ScheduleCreate.xaml
/// </summary>
public partial class ScheduleCreate : Page
{
    public ScheduleCreate(ScheduleCreateViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
