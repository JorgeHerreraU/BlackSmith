using BlackSmith.Presentation.ViewModels;
using System.Windows.Controls;

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