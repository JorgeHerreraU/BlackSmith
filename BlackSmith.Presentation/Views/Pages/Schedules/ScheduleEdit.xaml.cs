using BlackSmith.Presentation.ViewModels.Schedules;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Schedules;

/// <summary>
///     Interaction logic for ScheduleEdit.xaml
/// </summary>
public partial class ScheduleEdit : Page
{
    public ScheduleEdit(ScheduleEditViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
