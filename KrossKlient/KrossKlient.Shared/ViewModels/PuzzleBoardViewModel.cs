using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using KrossKlient.Common;
using KrossKlient.Services;
using ReactiveUI;
using Splat;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class PuzzleBoardViewModel : ReactiveObject
    {
        private readonly IUserService UserService;
        private readonly ObservableCollection<EmptyCellViewModel> _cells;
        protected IPuzzlesService PuzzlesService;
        [DataMember] private bool _acrossAndDownVisible;
        [DataMember] private EmptyCellViewModel _currentSelectedCell;
        [DataMember] private string _currentUser;
        [DataMember] private bool _gameIsRunning;
        [DataMember] private string _gameScoreDisplay;
        [DataMember] private ObservableCollection<WordViewModel> _words;
        [DataMember] private PuzzleViewModel _puzzleViewModel;
        [DataMember] private bool _wordSelectedVisibility;
        [DataMember] private string _enteredWord;

        public PuzzleBoardViewModel(IPuzzlesService puzzlesService = null, IUserService userService = null)
        {
            _cells = new ObservableCollection<EmptyCellViewModel>();
            CreateCellsForBoard();
            PuzzlesService = puzzlesService ?? Locator.Current.GetService<IPuzzlesService>();
            UserService = userService ?? Locator.Current.GetService<IUserService>();

            _selectedWord = this.WhenAny(vm => vm.CurrentSelectedCell, x => x.Value)
                .Where(x => x != null)
                .Select(x => SetLikelyWordMatchOnBoardForSelectedCell(x))
                .ToProperty(this, x => x.SelectedWord);

            _selectedWordLength = this.WhenAny(vm => vm._selectedWord, x => x.Value)
                .Select(x => x.Value.Cells.Count)
                .ToProperty(this, x => x.SelectedWordLength);
        }

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

        public string GameScoreDisplay
        {
            get { return _gameScoreDisplay; }
            set { this.RaiseAndSetIfChanged(ref _gameScoreDisplay, value); }
        }

        public ObservableCollection<WordViewModel> Words
        {
            get { return _words; }
            set { this.RaiseAndSetIfChanged(ref _words, value); }
        }

        public bool AcrossAndDownVisible
        {
            get { return _acrossAndDownVisible; }
            set { this.RaiseAndSetIfChanged(ref _acrossAndDownVisible, value); }
        }

        public EmptyCellViewModel CurrentSelectedCell
        {
            get { return _currentSelectedCell; }
            set
            {
                this.RaiseAndSetIfChanged(ref _currentSelectedCell, value);
                //SetLikelyWordMatchOnBoardForSelectedCell(value);
            }
        }

        public PuzzleViewModel PuzzleViewModel 
        {
            get { return _puzzleViewModel; }
            set { this.RaiseAndSetIfChanged(ref _puzzleViewModel, value); }
        }

        public bool WordSelectedVisibility
        {
            get { return _wordSelectedVisibility; }
            set { this.RaiseAndSetIfChanged(ref _wordSelectedVisibility, value); }
        }

        private readonly ObservableAsPropertyHelper<WordViewModel> _selectedWord;
        public WordViewModel SelectedWord
        {
            get { return _selectedWord.Value; }
        }

        public string EnteredWord
        {
            get { return _enteredWord; }
            set { this.RaiseAndSetIfChanged(ref _enteredWord, value); }
        }

        readonly ObservableAsPropertyHelper<int> _selectedWordLength;
        public int SelectedWordLength
        {
            get { return _selectedWordLength.Value; }
        } 
        
        readonly ObservableAsPropertyHelper<string> _gameCountDown;
        public string GameCountDown
        {
            get { return _gameCountDown.Value; }
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

                    int cellPositionOnBoard = (cell.Row*12) + cell.Col;
                    if (!firstCellVisited) startPositionForWordOnBoard = wordViewModel.Index.ToString();
                    firstCellVisited = true;

                    Cells[cellPositionOnBoard] = new CellViewModel(cell.Col, cell.Row, cell.Value, wordViewModel, startPositionForWordOnBoard);
                }
            }
        }

        private WordViewModel SetLikelyWordMatchOnBoardForSelectedCell(EmptyCellViewModel value)
        {
            var words = Words.Where(x => x.Cells.Count(y => y.Col == value.Col && y.Row == value.Row) > 0);
            var word = words.FirstOrDefault();
            return word;
        }
    }
}