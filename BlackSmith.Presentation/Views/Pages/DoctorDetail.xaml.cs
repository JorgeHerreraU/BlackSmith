using System.Windows.Controls;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for DoctorDetail.xaml
/// </summary>
public partial class DoctorDetail : Page
{
    public DoctorDetail(DoctorDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}