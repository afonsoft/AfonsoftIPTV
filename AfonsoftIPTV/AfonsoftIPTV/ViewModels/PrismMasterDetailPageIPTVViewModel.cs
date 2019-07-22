using AfonsoftIPTV.Models;
using AfonsoftIPTV.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AfonsoftIPTV.ViewModels
{
    public class PrismMasterDetailPageIPTVViewModel : ViewModelBase
    {

        private ObservableCollection<MasterMenuItem> MenuItems { get; set; }

        private MasterMenuItem selectedMenuItem;
        private MasterMenuItem SelectedMenuItem
        {
            get => selectedMenuItem;
            set => SetProperty(ref selectedMenuItem, value);
        }
        public DelegateCommand NavigateCommand { get; private set; }

        public PrismMasterDetailPageIPTVViewModel(INavigationService navigationService) : base(navigationService)
        {

            MenuItems = new ObservableCollection<MasterMenuItem>();

            MenuItems.Add(new MasterMenuItem()
            {
                Icon = "ic_viewa",
                PageName = nameof(PrismContentPageSettings),
                Title = "Settings"
            });

            NavigateCommand = new DelegateCommand(Navigate);
        }
        async void Navigate()
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + SelectedMenuItem.PageName);
        }
    }
}
