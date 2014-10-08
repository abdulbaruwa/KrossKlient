using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using KrossKlient.Services;
using ReactiveUI;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class HomePageViewModel : ReactiveObject
    {
        [DataMember]private IPuzzleRepository _puzzleRepository;
        public IPuzzleRepository PuzzleRepository
        {
            get { return _puzzleRepository; }
            private set { _puzzleRepository = value; }
        }

        [DataMember]private IUserService _userService;        
        public IUserService UserService
        {
            get { return _userService; }
            private set { _userService = value; }
        }

        public HomePageViewModel(IPuzzleRepository puzzleRepository, IUserService userService)
        {
            PuzzleRepository = puzzleRepository;
            UserService = userService;
        }


        public string CurrentUser
        {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        public ObservableCollection<PuzzleGroupViewModel> PuzzleGroupViewModels
        {
            get { return _puzzles; }
            set
            {
                SetProperty(ref _puzzles, value);
            }
        }

        public RelayCommand<PuzzleViewModel> StartPuzzleCommand { get; private set; }

        public PuzzleViewModel SelectedPuzzleGroupViewModel
        {
            get { return _selectedPuzzleViewModel; }
            set
            {
                SetProperty(ref _selectedPuzzleViewModel, value);
            }
        }

        public object SelectedValueBinding
        {
            get { return _selectedValueBinding; }
        }

        public List<PuzzleGroup> PuzzleGroupData
        {
            get { return _puzzleGroupData; }
            set { SetProperty(ref _puzzleGroupData, value); }
        }

    }
}
