using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using KrossKlient.DataModel;
using KrossKlient.Services;
using KrossKlient.ViewModels.DesignTime;
using ReactiveUI;
using Splat;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class HomePageViewModel : ReactiveObject, ILoginMethods
    {
        public HomePageViewModel(IMutableDependencyResolver testResolver = null)
        {
            Akavache.BlobCache.ApplicationName = "Kross";
            RegisterResolver(testResolver);
            this.Log().Debug("Kross Klient starting ");
            // Get credentials
            FetchLatestGames();
        }

        private void RegisterResolver(IMutableDependencyResolver testResolver)
        {
            if (testResolver == null)
            {
                Resolver = Locator.CurrentMutable;
                Resolver.Register(() => new FakePuzzlesService(), typeof(IPuzzlesService));
                Resolver.Register(() => new UserService(), typeof(IUserService)); 
                Resolver.Register(() => new FakePuzzleRepository(), typeof(IPuzzleRepository));
            }
            else
            {
                Resolver = testResolver;
            }
        }

        public IMutableDependencyResolver Resolver { get; protected set; }

        private IPuzzlesService _puzzlesService;
        public IPuzzlesService PuzzlesService
        {
            get { return _puzzlesService; }
            private set { _puzzlesService = value; }
        }

        private IUserService _userService;        
        public IUserService UserService
        {
            get { return _userService; }
            private set { _userService = value; }
        }


        [DataMember] private string _currentUser;
        public string CurrentUser
        {
            get { return _currentUser; }
            set { this.RaiseAndSetIfChanged(ref _currentUser, value); }
        }

        [DataMember] private List<PuzzleGroup> _puzzleGroups;
        public List<PuzzleGroup> PuzzleGroups
        {
            get { return _puzzleGroups; }
            set
            {
                this.RaiseAndSetIfChanged(ref _puzzleGroups, value);
            }
        }

        [DataMember] private PuzzleViewModel _selectedPuzzleViewModel;
        public PuzzleViewModel SelectedPuzzleGroupViewModel
        {
            get { return _selectedPuzzleViewModel; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPuzzleViewModel, value);
            }
        }

        [DataMember] private List<PuzzleGroup> _puzzleGroupData;
        public List<PuzzleGroup> PuzzleGroupData
        {
            get { return _puzzleGroupData; }
            set { this.RaiseAndSetIfChanged(ref _puzzleGroupData, value); }
        }

        public void SaveCredentials(string userName = null)
        {
            throw new System.NotImplementedException();
        }

        private void FetchLatestGames()
        {
            var service = Locator.Current.GetService<IPuzzlesService>();
            service.GetPuzzles().ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x =>
            {
                if (x != null)
                {
                    if (x.Count > 0)
                    {
                        var result = new List<PuzzleGroup>();
                        result.AddRange(x);
                        PuzzleGroups = result;
                    }
                }
            },
                ex => this.Log().Info("No game stats"));
        }
    }
}
