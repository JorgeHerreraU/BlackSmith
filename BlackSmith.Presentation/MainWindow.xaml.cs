using System.Windows;
using BlackSmith.Presentation.Interfaces;
using BlackSmith.Presentation.Services;
using BlackSmith.Presentation.Views.Pages;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly IPageService _pageService;
    private readonly IThemeService _themeService;

    public MainWindow(INavService navService,
        IPageService pageService,
        IThemeService themeService)
    {
        _pageService = pageService;
        _themeService = themeService;
        navService.NavigationTriggered += OnNavigationTriggered;
        InitializeComponent();
        RootNavigation.PageService = pageService;
        navService.Navigate(new NavigationTriggeredEventArgs { Page = typeof(Home) });
    }

    private void OnNavigationTriggered(object? sender,
        NavigationTriggeredEventArgs e)
    {
        var page = _pageService.GetPage(e.Page);
        RootFrame.NavigationService.Navigate(page);
    }

    private void NavigationButtonTheme_OnClick(object sender,
        RoutedEventArgs e)
    {
        _themeService.SetTheme(_themeService.GetTheme() == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark);
    }
}