using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.ApplicationModel;
using KrossKlient.DataModel;
using KrossKlient.ViewModels;
using KrossKlient.ViewModels.DesignTime;
using Splat;

namespace KrossKlient.Services
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            if (DesignMode.DesignModeEnabled) return;
            Resolver = Locator.CurrentMutable;

            //Resolver.RegisterConstant(UserService()); RegisterConstant(IUserService,UserService>());
            //Resolver.Default.Register<IPuzzleRepository, PuzzleRepository>();
            //SimpleIoc.Default.Register<IPuzzlesService, PuzzlesService>();
            //SimpleIoc.Default.Register<IPuzzleWebApiService, PuzzleWebApiService>();
            //SimpleIoc.Default.Register<IGameDataService, GameDataService.GameDataService>();
        }

        public HomePageViewModel HomePageViewModel
        {
            get { return GetHomePageViewModel(); }
        }

        public PuzzleBoardViewModel PuzzleBoardViewModel
        {
            get
            {
                return GetPuzzleBoardViewModel();
            }
        }

        public IMutableDependencyResolver Resolver { get; set; }

        private PuzzleBoardViewModel GetPuzzleBoardViewModel()
        {
            var vm = new PuzzleBoardViewModel(new FakePuzzlesService(), new FakeUserSevice());

            vm.CurrentUser = "Abdulrahaman";

            vm.PuzzleViewModel = new PuzzleViewModel() { Group = "Science", Title = "Level One" };
            vm.Words = new FakePuzzlesService().GetOrdereredWordsForPuzzle(0, vm.CurrentUser);

            vm.SelectedWord = (from word in vm.Words
                               where word.Word.Equals("india",StringComparison.OrdinalIgnoreCase)
                               select word).FirstOrDefault();

            vm.GameIsRunning = true;

            vm.GameCountDown = "00:00:00";

            vm.AcrossAndDownVisible = true;

            vm.AddWordsToBoard();
            return vm;
        }

        private HomePageViewModel GetHomePageViewModel()
        {
            var puzzles = new ObservableCollection<PuzzleGroup>();
            var sciencegroup = new PuzzleGroup { Name = "Science", Puzzles = new List<PuzzleSubGroup>()};

            sciencegroup.Puzzles.Add(PuzzleBuilder("Human Skeleton Puzzles"));
            sciencegroup.Puzzles.Add(PuzzleBuilder("Resperatory System"));
            sciencegroup.Puzzles.Add(PuzzleBuilder("Muscle System"));

            puzzles.Add(sciencegroup);


            var englishgroup = new PuzzleGroup {Name = "English", Puzzles = new List<PuzzleSubGroup>() };

            englishgroup.Puzzles.Add(PuzzleBuilder("English Vocabs Puzzles"));
            englishgroup.Puzzles.Add(PuzzleBuilder("Grammer"));
            puzzles.Add(englishgroup);

            var geographygroup = new PuzzleGroup { Name = "Geography", Puzzles = new List<PuzzleSubGroup>() };
            geographygroup.Puzzles.Add(PuzzleBuilder("Rivers Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Tectonic Plates Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Polution Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Volcanoes Puzzles"));
            puzzles.Add(geographygroup);
            var homePageViewModel = new HomePageViewModel {CurrentUser = "Ademola Baruwa", PuzzleGroups = puzzles};
            return homePageViewModel;
        }

        public PuzzleSubGroup PuzzleBuilder(string title)
        {
            var puzzleVm = new PuzzleSubGroup()
            {
                Title = title,
                Words = new Dictionary<string, string>
                {
                    
                        {"First","The first"},
                        {"Second", "The Second"},
                        {"Third", "The Third"},
                        {"Forth", "The Forth"},
                        {"Fifth", "The Fifth"},
                        {"Sixth", "The Sixth"}
                    }
            };
            return puzzleVm;
        }
    }
}