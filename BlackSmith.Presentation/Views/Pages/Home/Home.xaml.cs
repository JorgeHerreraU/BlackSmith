using BlackSmith.Presentation.ViewModels.Home;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Home;

/// <summary>
///     Interaction logic for Home.xaml
/// </summary>
public partial class Home : Page
{
    public Home(HomeViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
