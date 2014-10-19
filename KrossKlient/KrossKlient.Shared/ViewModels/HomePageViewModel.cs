﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using KrossKlient.DataModel;
using KrossKlient.Services;
using ReactiveUI;
using Splat;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class HomePageViewModel : ReactiveObject, ILoginMethods
    {
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

        public HomePageViewModel(IMutableDependencyResolver testResolver = null)
        {
            if (testResolver == null)
            {
                PuzzlesService = Locator.Current.GetService<IPuzzlesService>();
                UserService = Locator.Current.GetService<IUserService>();
            }
        }


        [DataMember] private string _currentUser;
        public string CurrentUser
        {
            get { return _currentUser; }
            set { this.RaiseAndSetIfChanged(ref _currentUser, value); }
        }

        [DataMember] private ReactiveList<PuzzleGroup> _puzzleGroups;
        public ReactiveList<PuzzleGroup> PuzzleGroups
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
            service.GetPuzzlesForUser(CurrentUser).ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x =>
            {
                if (x != null)
                {
                    if(x.Count > 0)PuzzleGroups.AddRange(x);
                }
            },
                ex => this.Log().Info("No game stats"));
        }
    }
}
