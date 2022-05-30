using BlackSmith.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackSmith.Presentation.State.Navigators;

public enum ViewType
{
    Home,
    Appointment
}
public interface INavigator
{
    BaseViewModel CurrentViewModel { get; set; }
    ICommand UpdateCurrentViewModelCommand { get; }
}
