using System.Windows.Controls;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Views.Pages;

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