using System;
using System.Linq;
using KrossKlient.Services;

namespace KrossKlient.ViewModels.DesignTime
{
    public class DesignPuzzleBoardViewModel : PuzzleBoardViewModel
    {
        public DesignPuzzleBoardViewModel()
            : base(new FakePuzzlesService(), new FakeUserSevice())
        {
            CurrentUser = "Abdulrahaman";

            PuzzleViewModel = new PuzzleViewModel() {Group = "Science", Title = "Level One"};
            Words = base.PuzzlesService.GetOrdereredWordsForPuzzle(0,CurrentUser);

            SelectedWord = (from word in Words
                            where word.Word.Equals("india", StringComparison.OrdinalIgnoreCase)
                            select word).FirstOrDefault();

            GameIsRunning = true;

            GameCountDown = "00:00:00";

            AcrossAndDownVisible = true;
            
            AddWordsToBoard();
        }
    }
}