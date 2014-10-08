using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using KrossKlient.ViewModels;
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

        public IMutableDependencyResolver Resolver { get; set; }

        private HomePageViewModel GetHomePageViewModel()
        {
            var puzzles = new ObservableCollection<PuzzleGroupViewModel>();
            var sciencegroup = new PuzzleGroupViewModel {Category = "Science", Puzzles = new ObservableCollection<PuzzleViewModel>()};

            sciencegroup.Puzzles.Add(PuzzleBuilder("Human Skeleton Puzzles"));
            sciencegroup.Puzzles.Add(PuzzleBuilder("Resperatory System"));
            sciencegroup.Puzzles.Add(PuzzleBuilder("Muscle System"));

            puzzles.Add(sciencegroup);


            var englishgroup = new PuzzleGroupViewModel {Category = "English", Puzzles = new ObservableCollection<PuzzleViewModel>()};
            englishgroup.Puzzles.Add(PuzzleBuilder("English Vocabs Puzzles"));
            englishgroup.Puzzles.Add(PuzzleBuilder("Grammer"));
            puzzles.Add(englishgroup);
            var geographygroup = new PuzzleGroupViewModel {Category = "Geography", Puzzles = new ObservableCollection<PuzzleViewModel>()};
            geographygroup.Puzzles.Add(PuzzleBuilder("Rivers Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Tectonic Plates Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Polution Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Volcanoes Puzzles"));
            puzzles.Add(geographygroup);
            var homePageViewModel = new HomePageViewModel {CurrentUser = "Ademola Baruwa", PuzzleGroupViewModels = puzzles};
            return homePageViewModel;
        }

        public PuzzleViewModel PuzzleBuilder(string title)
        {
            var puzzleVm = new PuzzleViewModel
            {
                Title = title,
                Group = "My Group",
                Words =
                    new List<string>
                    {
                        "First",
                        "Second",
                        "Third",
                        "Forth",
                        "Fifth",
                        "Sixth"
                    }
            };
            return puzzleVm;
        }
    }
}