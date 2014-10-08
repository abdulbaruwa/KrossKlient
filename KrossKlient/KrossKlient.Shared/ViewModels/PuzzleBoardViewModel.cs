using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using KrossKlient.Services;
using ReactiveUI;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class PuzzleBoardViewModel : ReactiveObject
    {
        private readonly ObservableCollection<EmptyCellViewModel> _cells;
        [DataMember] private string _currentUser;
        [DataMember] private string _gameCountDown;
        [DataMember] private bool _gameIsRunning;
        [DataMember] private string _gameScoreDisplay;

        private readonly IUserService UserService;
        protected IPuzzlesService PuzzlesService;

        public PuzzleBoardViewModel()
        {
            CreateCellsForBoard();
        }

        public PuzzleBoardViewModel(IPuzzlesService puzzlesService, IUserService userService)
        {
            CreateCellsForBoard();
            PuzzlesService = puzzlesService;
            UserService = userService;
        }

        public PuzzleViewModel PuzzleViewModel { get; set; }

        public ObservableCollection<EmptyCellViewModel> Cells
        {
            get { return _cells; }
        }

        public string CurrentUser
        {
            get { return _currentUser; }
            set { this.RaiseAndSetIfChanged(ref _currentUser, value); }
        }


        public bool GameIsRunning
        {
            get { return _gameIsRunning; }
            set { this.RaiseAndSetIfChanged(ref _gameIsRunning, value); }
        }

        public string GameCountDown
        {
            get { return _gameCountDown; }
            set { this.RaiseAndSetIfChanged(ref _gameCountDown, value); }
        }

        public string GameScoreDisplay
        {
            get { return _gameScoreDisplay; }
            set { this.RaiseAndSetIfChanged(ref _gameScoreDisplay, value); }
        }

        [DataMember] private ObservableCollection<WordViewModel> _words;
        public ObservableCollection<WordViewModel> Words
        {
            get { return _words; }
            set { this.RaiseAndSetIfChanged(ref _words, value); }
        }

        private void CreateCellsForBoard()
        {
            var cells = new List<EmptyCellViewModel>();
            cells.AddRange(
                from row in Enumerable.Range(0, 12)
                from col in Enumerable.Range(0, 12)
                select new EmptyCellViewModel(col, row, string.Empty));

            foreach (EmptyCellViewModel cellViewModel in cells)
            {
                _cells.Add(cellViewModel);
            }
        }

        public void AddWordsToBoard()
        {
            foreach (WordViewModel wordViewModel in Words)
            {
                bool firstCellVisited = false;
                foreach (EmptyCellViewModel cell in wordViewModel.Cells)
                {
                    string startPositionForWordOnBoard = string.Empty;

                    int cellPositionOnBoard = (cell.Row * 12) + cell.Col;
                    if (!firstCellVisited) startPositionForWordOnBoard = wordViewModel.Index.ToString();
                    firstCellVisited = true;

                    Cells[cellPositionOnBoard] = new CellViewModel(cell.Col, cell.Row, cell.Value, wordViewModel, startPositionForWordOnBoard);
                }
            }
        }

    }
}