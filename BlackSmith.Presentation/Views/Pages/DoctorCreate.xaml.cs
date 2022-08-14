using System.Windows.Controls;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for DoctorCreate.xaml
/// </summary>
public partial class DoctorCreate : Page
{
    public DoctorCreate(DoctorCreateViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}