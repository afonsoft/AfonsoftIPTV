using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AfonsoftIPTV.ViewModels
{
    public class PrismTabbedPageListViewModel : ViewModelBase
    {
        public PrismTabbedPageListViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
