using System;
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
        private NavigationHelper navigationHelper;
        public HomePage()
        {
            InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += this.NavigationHelper_SaveState;

            //Bindings
            //Hub Section 0
            this.Bind(ViewModel, x => x.PuzzleGroups[0], x => x.HubSection1.DataContext);
            this.Bind(ViewModel, x => x.PuzzleGroups[0].Name, x => x.HubSection1.Header);

            //Hub Section 1
            this.OneWayBind(ViewModel, x => x.PuzzleGroups[1], x => x.HubSection2.DataContext);
            this.Bind(ViewModel, x => x.PuzzleGroups[1].Name, x => x.HubSection2.Header);

            //Hub Section 2
            this.OneWayBind(ViewModel, x => x.PuzzleGroups[2], x => x.HubSection3.DataContext);
            this.Bind(ViewModel, x => x.PuzzleGroups[2].Name, x => x.HubSection3.Header);

        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.ViewModel = new HomePageViewModel();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (HomePageViewModel) value; }
        }

        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register("ViewModel", typeof(HomePageViewModel), typeof(HomePage), new PropertyMetadata(default(HomePage)));

        public HomePageViewModel ViewModel { get; set; }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}