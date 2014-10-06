using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using KrossKlient.Common;
using KrossKlient.ViewModels;
using ReactiveUI;

namespace KrossKlient
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page, IViewFor<HomePageViewModel>
    {
        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            "ViewModel", typeof (HomePageViewModel), typeof (HomePage), new PropertyMetadata(default(HomePage)));

        private readonly NavigationHelper _navigationHelper;


        public HomePage()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
        }

        /// <summary>
        ///     Gets the NavigationHelper used to aid navigation and process life management:
        ///     Note: We are not using ReactiveUI navigation stuff - WP8 is opinionated as regard Routing (shame ;-( ).
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public HomePageViewModel ViewModel
        {
            get { return (HomePageViewModel) GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (HomePageViewModel) value; }
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            ViewModel = new HomePageViewModel();
            DataContext = this;
        }

        #region NavigationHelper registration

        /// <summary>
        ///     The methods provided in this section are simply used to allow
        ///     NavigationHelper to respond to the page's navigation methods.
        ///     Page specific logic should be placed in event handlers for the
        ///     <see cref="Common.NavigationHelper.LoadState" />
        ///     and <see cref="Common.NavigationHelper.SaveState" />.
        ///     The navigation parameter is available in the LoadState method
        ///     in addition to page state preserved during an earlier session.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}