using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using KrossKlient.DataModel;
using KrossKlient.Services;
using ReactiveUI;
using Splat;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class HomePageViewModel : ReactiveObject, ILoginMethods
    {
        private IPuzzleRepository _puzzleRepository;
        public IPuzzleRepository PuzzleRepository
        {
            get { return _puzzleRepository; }
            private set { _puzzleRepository = value; }
        }

        private IUserService _userService;        
        public IUserService UserService
        {
            get { return _userService; }
            private set { _userService = value; }
        }

        public HomePageViewModel(IMutableDependencyResolver testResolver = null)
        {
            if (testResolver == null)
            {
                PuzzleRepository = Locator.Current.GetService<IPuzzleRepository>();
                UserService = Locator.Current.GetService<IUserService>();
            }
        }

        public HomePageViewModel(IPuzzleRepository puzzleRepository, IUserService userService)
        {
            PuzzleRepository = puzzleRepository;
            UserService = userService;
        }

        [DataMember] private string _currentUser;
        public string CurrentUser
        {
            get { return _currentUser; }
            set { this.RaiseAndSetIfChanged(ref _currentUser, value); }
        }

        [DataMember] private ObservableCollection<PuzzleGroup> _puzzles;
        public ObservableCollection<PuzzleGroup> PuzzleGroupViewModels
        {
            get { return _puzzles; }
            set
            {
                this.RaiseAndSetIfChanged(ref _puzzles, value);
            }
        }

        [DataMember] private PuzzleViewModel _selectedPuzzleViewModel;
        public PuzzleViewModel SelectedPuzzleGroupViewModel
        {
            get { return _selectedPuzzleViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPuzzleViewModel, value);
            }
        }

        [DataMember] private List<PuzzleGroup> _puzzleGroupData;
        public List<PuzzleGroup> PuzzleGroupData
        {
            get { return _puzzleGroupData; }
            set { this.RaiseAndSetIfChanged(ref _puzzleGroupData, value); }
        }

        public void SaveCredentials(string userName = null)
        {
            throw new System.NotImplementedException();
        }

        private void FetchLatestGames()
        {
            var service = Locator.Current.GetService<IPuzzlesService>();
            service.GetPuzzlesForUser(CurrentUser);
        }
    }
}
