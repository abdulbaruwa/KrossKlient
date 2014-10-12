using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using KrossKlient.Common;
using KrossKlient.ViewModels;

namespace KrossKlient.Services
{
    public class PuzzlesService : IPuzzlesService
    {
        private IPuzzleRepository puzzlesRepository;
        private IUserService _userService;

        public PuzzlesService(IPuzzleRepository puzzlesRepository, IUserService userService)
        {
            this.puzzlesRepository = puzzlesRepository;
            _userService = userService;
        }

        public ObservableCollection<WordViewModel> GetOrdereredWordsForPuzzle(int puzzleId, string user)
        {
            var words = puzzlesRepository.GetPuzzleWithId(puzzleId, user);

            var wordviewmodels = GetWordsWordviewmodels(words);

            return SortWordsByPositionOnBoard(wordviewmodels);

        }

        public IList<WordViewModel> GetWordsInsertableIntoPuzzle(Dictionary<string, string> words)
        {
            try
            {
                var wordviewmodels = GetWordsWordviewmodels(words);

                return SortWordsByPositionOnBoard(wordviewmodels);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private ObservableCollection<WordViewModel> SortWordsByPositionOnBoard(List<WordViewModel> wordviewmodels)
        {
            var orderedWords = wordviewmodels.OrderBy(x => x.Index).ToList();
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
            var result = board.InsertWordResults;
            var wordsInserted = result.Where(x => x.Inserted);
            var wordviewmodels = new List<WordViewModel>();
            foreach (var word in wordsInserted)
            {
                Debug.WriteLine(word);
                var position = (word.StartCell.Item1*12) + word.StartCell.Item2;

                var wordViewModel = new WordViewModel()
                {
                    Cells = new ObservableCollection<EmptyCellViewModel>(),
                    Direction = word.Direction,
                    Word = word.Word.ToString(),
                    WordHint = words.First(x => x.Key == word.Word.ToString()).Value,
                    WordLength = "(" + word.Word.Length + ")",
                    Index = position
                };

                var row = word.StartCell.Item1;
                var col = word.StartCell.Item2;
                foreach (var character in word.Word)
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