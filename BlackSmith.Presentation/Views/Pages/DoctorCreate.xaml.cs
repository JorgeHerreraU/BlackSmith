using BlackSmith.Presentation.ViewModels;
using Wpf.Ui.Common.Interfaces;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for DoctorCreate.xaml
/// </summary>
public partial class DoctorCreate : INavigableView<DoctorCreateViewModel>
{
    public DoctorCreate(DoctorCreateViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    public DoctorCreateViewModel ViewModel { get; }
}