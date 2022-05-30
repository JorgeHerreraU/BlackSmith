using BlackSmith.Presentation.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSmith.Presentation.ViewModels;
public class MainViewModel : BaseViewModel
{
    public INavigator Navigator { get; set; }

    public MainViewModel(INavigator navigator)
    {
        Navigator = navigator;
    }
}
