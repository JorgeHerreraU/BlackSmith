using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BlackSmith.Presentation.Controls.AttachedProperties;

public class DataGridAttachedProperties : DependencyObject
{
    public static readonly DependencyProperty CommandRefreshOnScrollingProperty =
        DependencyProperty.RegisterAttached(
            "CommandRefreshOnScrolling",
            typeof(bool),
            typeof(DataGridAttachedProperties),
            new FrameworkPropertyMetadata(false, OnCommandRefreshOnScrollingChanged));

    public bool CommandRefreshOnScrolling
    {
        set => SetValue(CommandRefreshOnScrollingProperty, value);
    }

    private static void OnCommandRefreshOnScrollingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DataGrid dataGrid) return;
        if ((bool)e.NewValue) dataGrid.PreviewMouseWheel += DataGridPreviewMouseWheel;
    }
    private static void DataGridPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
