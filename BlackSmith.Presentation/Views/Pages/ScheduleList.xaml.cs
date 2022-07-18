using System.Windows.Controls;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Views.Pages;

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