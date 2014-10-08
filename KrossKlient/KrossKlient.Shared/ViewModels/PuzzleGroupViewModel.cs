using System.Collections.ObjectModel;

namespace KrossKlient.ViewModels
{
    public class PuzzleGroupViewModel
    {
        public string Category { get; set; }
        public ObservableCollection<PuzzleViewModel> Puzzles { get; set; }
    }
}