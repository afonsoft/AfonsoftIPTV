using Xamarin.Forms;

namespace AfonsoftIPTV.Views
{
    public partial class PrismMasterDetailPageIPTV : MasterDetailPage
    {
        public PrismMasterDetailPageIPTV()
        {
            InitializeComponent();
            Detail = new NavigationPage(new PrismTabbedPageList());
            IsPresented = false;
        }
    }
}