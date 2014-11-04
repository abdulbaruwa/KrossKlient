using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using KrossKlient.Common;
using KrossKlient.ViewModels;
using ReactiveUI;

namespace KrossKlient
{
    public sealed partial class PuzzleBoardView : Page, IViewFor<PuzzleBoardViewModel>
    {
        private NavigationHelper navigationHelper;

        public PuzzleBoardView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += this.NavigationHelper_SaveState;

            this.Bind(ViewModel, x => x.Cells, x => x.GameBoard.ItemsSource);
            this.Bind(ViewModel, x => x.EnteredWord, x => x.AnswerForSelectedWord.Text);

            this.Bind(ViewModel, x => x.CurrentSelectedCell, x => x.GameBoard.SelectedItem);

            this.OneWayBind(ViewModel, x => x.SelectedWord.WordHint, x => x.HintForSelectedWord.Text);
            this.OneWayBind(ViewModel, x => x.SelectedWordLength, x => x.AnswerForSelectedWord.MaxLength);

        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.ViewModel = new PuzzleBoardViewModel();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PuzzleBoardViewModel) value; }
        }

        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register("ViewModel", typeof(PuzzleBoardViewModel), typeof(PuzzleBoardView), new PropertyMetadata(default(PuzzleBoardView)));

        public PuzzleBoardViewModel ViewModel { get; set; }

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
