using System;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace BlackSmith.Presentation.Services;

public class PageService : IPageService
{
    private readonly IServiceProvider _serviceProvider;

    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T? GetPage<T>() where T : class
    {
        return !typeof(FrameworkElement).IsAssignableFrom(typeof(T))
            ? throw new InvalidOperationException("The page should be a WPF control.")
            : (T?)_serviceProvider.GetService(typeof(T));
    }

    public FrameworkElement? GetPage(Type pageType)
    {
        return !typeof(FrameworkElement).IsAssignableFrom(pageType)
            ? throw new InvalidOperationException("The page should be a WPF control.")
            : _serviceProvider.GetService(pageType) as FrameworkElement;
    }
}