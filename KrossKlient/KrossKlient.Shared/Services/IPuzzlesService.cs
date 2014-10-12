using System.Collections.Generic;
using System.Collections.ObjectModel;
using KrossKlient.ViewModels;

namespace KrossKlient.Services
{
    public interface IPuzzlesService
    {
        ObservableCollection<WordViewModel> GetOrdereredWordsForPuzzle(int puzzleId, string user);
        IList<WordViewModel> GetWordsInsertableIntoPuzzle(Dictionary<string, string> words);
    }
}