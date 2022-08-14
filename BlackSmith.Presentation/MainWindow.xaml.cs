using System.Threading.Tasks;
using System.Windows;
using BlackSmith.Presentation.Views.Pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly IThemeService _themeService;

    public MainWindow(INavigationService navigationService,
        IPageService pageService,
        IThemeService themeService)
    {
        _themeService = themeService;

        InitializeComponent();

        navigationService.SetNavigationControl(RootNavigation);
        RootNavigation.PageService = pageService;

        Task.Run(async () =>
        {
            await Task.Delay(400);
            await Dispatcher.InvokeAsync(() => { RootNavigation.Navigate(typeof(Home)); });
            return true;
        });
    }


    private void NavigationButtonTheme_OnClick(object sender,
        RoutedEventArgs e)
    {
        _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
    }

    private void RootNavigation_OnNavigated(INavigation sender,
        RoutedNavigationEventArgs e)
    {
        sender.Current.IsActive = true;
    }
}