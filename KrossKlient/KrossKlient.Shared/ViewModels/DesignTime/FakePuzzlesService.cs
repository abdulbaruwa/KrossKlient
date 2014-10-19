using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using KrossKlient.Common;
using KrossKlient.DataModel;
using KrossKlient.Services;

namespace KrossKlient.ViewModels.DesignTime
{
    public class FakePuzzlesService : IPuzzlesService
    {
        private IPuzzleRepository _repository = new FakePuzzleRepository();
        private Dictionary<string, string> _words;

        public ObservableCollection<WordViewModel> GetOrdereredWordsForPuzzle(int puzzleId, string user)
        {
            Dictionary<string, string> words = _words ?? new Dictionary<string, string>
            {
                {"Bamidele", "Adetoro's first name. Rashedat omo Abdulrahaman Adedayo Baruwa and Rasheedat Patience Binta OluwaFunmilayo Baruwa. Sister to Abdulrasheed Ademola Dabira Adedayo Baruwa "},
                {"station", "place where i fit get train"},
                {"india", "Origin of my favourite curry, spicy hot tropical country. With loads and loads of people. Probably the second most populated country Origin of my favourite curry, spicy hot tropical country. With loads and loads of people. Probably the second most populated country"},
                {"Adams", "Captain Arsenal"},
                {"fards", "show off"},
                {"novemb", "like november"},
                {"belt", "Tied around my waist"},
                {"train", "Mode of transportation"},
                {"adeola", "My sister"},
                {"amoeba", "Single cell organism"},
                {"moscow", "Cold city behind iron curtain"}
            };

            List<WordViewModel> wordviewmodels = GetWordsWordviewmodels(words);

            return SortWordsByPositionOnBoard(wordviewmodels);
        }

        public IList<WordViewModel> GetWordsInsertableIntoPuzzle(Dictionary<string, string> words)
        {
            throw new NotImplementedException();
        }

        public IObservable<List<PuzzleGroup>> GetPuzzlesForUser(string currentUser)
        {
            var puzzles = new List<PuzzleGroup>();
            var sciencegroup = new PuzzleGroup { Name = "Science", Puzzles = new List<PuzzleSubGroup>() };

            sciencegroup.Puzzles.Add(PuzzleBuilder("Human Skeleton Puzzles"));
            sciencegroup.Puzzles.Add(PuzzleBuilder("Resperatory System"));
            sciencegroup.Puzzles.Add(PuzzleBuilder("Muscle System"));

            puzzles.Add(sciencegroup);

            var englishgroup = new PuzzleGroup { Name = "English", Puzzles = new List<PuzzleSubGroup>() };

            englishgroup.Puzzles.Add(PuzzleBuilder("English Vocabs Puzzles"));
            englishgroup.Puzzles.Add(PuzzleBuilder("Grammer"));
            puzzles.Add(englishgroup);

            var geographygroup = new PuzzleGroup { Name = "Geography", Puzzles = new List<PuzzleSubGroup>() };
            geographygroup.Puzzles.Add(PuzzleBuilder("Rivers Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Tectonic Plates Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Polution Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Volcanoes Puzzles"));
            puzzles.Add(geographygroup);

            IObservable<List<PuzzleGroup>> observable = Observable.Create<List<PuzzleGroup>>(o =>
            {
                o.OnNext(puzzles);
                o.OnCompleted();
                return () => {}
                ;
            });

            return observable;
        }

        public PuzzleSubGroup PuzzleBuilder(string title)
        {
            var puzzleVm = new PuzzleSubGroup()
            {
                Title = title,
                Words = new Dictionary<string, string>
                {

                    {"First", "The first"},
                    {"Second", "The Second"},
                    {"Third", "The Third"},
                    {"Forth", "The Forth"},
                    {"Fifth", "The Fifth"},
                    {"Sixth", "The Sixth"}
                }
            };
            return puzzleVm;
        }

        public string[,] GetEmptyBoard()
        {
            throw new NotImplementedException();
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


        public void AddWords(Dictionary<string, string> words)
        {
            _words = words;
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
                var position = (word.StartCell.Item1 * 12) + word.StartCell.Item2;

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