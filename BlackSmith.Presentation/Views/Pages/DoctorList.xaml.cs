using System.Windows.Controls;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for DoctorList.xaml
/// </summary>
public partial class DoctorList : Page
{
    public DoctorList(DoctorListViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}