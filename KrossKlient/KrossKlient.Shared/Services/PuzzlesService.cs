using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Akavache;
using KrossKlient.Common;
using KrossKlient.DataModel;
using KrossKlient.ViewModels;
using Splat;

namespace KrossKlient.Services
{
    public class PuzzlesService : IPuzzlesService
    {
        private readonly IBlobCache _blobCache;
        private readonly IPuzzleRepository puzzlesRepository;
        private IUserService _userService;

        public PuzzlesService(IBlobCache blobCache = null)
        {
            puzzlesRepository = Locator.Current.GetService<IPuzzleRepository>();
            _userService = Locator.Current.GetService<IUserService>();
            _blobCache = blobCache ?? Locator.Current.GetService<IBlobCache>();
        }

        public ObservableCollection<WordViewModel> GetOrdereredWordsForPuzzle(int puzzleId, string user)
        {
            Dictionary<string, string> words = puzzlesRepository.GetPuzzleWithId(puzzleId, user);

            List<WordViewModel> wordviewmodels = GetWordsWordviewmodels(words);

            return SortWordsByPositionOnBoard(wordviewmodels);
        }

        public IList<WordViewModel> GetWordsInsertableIntoPuzzle(Dictionary<string, string> words)
        {
            try
            {
                List<WordViewModel> wordviewmodels = GetWordsWordviewmodels(words);

                return SortWordsByPositionOnBoard(wordviewmodels);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public IObservable<List<PuzzleGroup>> GetPuzzles()
        {
            IObservable<List<PuzzleGroup>> observableResult = _blobCache.GetObject<List<PuzzleGroup>>("kross");
            return observableResult;
        }

        private ObservableCollection<WordViewModel> SortWordsByPositionOnBoard(List<WordViewModel> wordviewmodels)
        {
            List<WordViewModel> orderedWords = wordviewmodels.OrderBy(x => x.Index).ToList();
            int lastindex = orderedWords[0].Index;
            var sortedWordViewModel = new ObservableCollection<WordViewModel>();

            for (int i = 0; i < orderedWords.Count; i++)
            {
                if (i > 0)
                {
                    if (orderedWords[i].Index == lastindex)
                    {
                        lastindex = orderedWords[i].Index;
                        orderedWords[i].Index = orderedWords[i - 1].Index;
                    }
                    else
                    {
                        lastindex = orderedWords[i].Index;
                        orderedWords[i].Index = orderedWords[i - 1].Index + 1;
                    }
                }
                else
                {
                    orderedWords[i].Index = 1;
                }
                sortedWordViewModel.Add(orderedWords[i]);
            }
            return sortedWordViewModel;
        }

        public List<WordViewModel> GetWordsWordviewmodels(Dictionary<string, string> words)
        {
            ////var words = new List<string>();
            //words.Add("Bamidele");
            //words.Add("station");
            //words.Add("india");
            //words.Add("Adams");
            //words.Add("fards");
            //words.Add("novemb");
            //words.Add("belt");
            //words.Add("train");
            //words.Add("adeola");
            //words.Add("amoeba");
            //words.Add("moscow");
            var board = new Board(12);
            board.ProcessWords(words.Keys.ToArray());
            List<InsertWordResult> result = board.InsertWordResults;
            IEnumerable<InsertWordResult> wordsInserted = result.Where(x => x.Inserted);
            var wordviewmodels = new List<WordViewModel>();
            foreach (InsertWordResult word in wordsInserted)
            {
                Debug.WriteLine(word);
                int position = (word.StartCell.Item1*12) + word.StartCell.Item2;

                var wordViewModel = new WordViewModel
                {
                    Cells = new ObservableCollection<EmptyCellViewModel>(),
                    Direction = word.Direction,
                    Word = word.Word,
                    WordHint = words.First(x => x.Key == word.Word.ToString()).Value,
                    WordLength = "(" + word.Word.Length + ")",
                    Index = position
                };

                int row = word.StartCell.Item1;
                int col = word.StartCell.Item2;
                foreach (char character in word.Word)
                {
                    var cell = new CellViewModel(col, row, character.ToString(), wordViewModel, string.Empty);
                    if (word.Direction == Direction.Across)
                        col += 1;
                    else
                        row += 1;

                    wordViewModel.Cells.Add(cell);
                }
                wordviewmodels.Add((wordViewModel));
            }
            return wordviewmodels;
        }
    }
}