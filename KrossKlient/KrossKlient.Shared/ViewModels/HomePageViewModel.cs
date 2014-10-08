﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using KrossKlient.DataModel;
using KrossKlient.Services;
using ReactiveUI;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class HomePageViewModel : ReactiveObject
    {
        private IPuzzleRepository _puzzleRepository;
        public IPuzzleRepository PuzzleRepository
        {
            get { return _puzzleRepository; }
            private set { _puzzleRepository = value; }
        }

        private IUserService _userService;        
        public IUserService UserService
        {
            get { return _userService; }
            private set { _userService = value; }
        }

        public HomePageViewModel()
        {
        }

        public HomePageViewModel(IPuzzleRepository puzzleRepository, IUserService userService)
        {
            PuzzleRepository = puzzleRepository;
            UserService = userService;
        }

        [DataMember] private string _currentUser;
        public string CurrentUser
        {
            get { return _currentUser; }
            set { this.RaiseAndSetIfChanged(ref _currentUser, value); }
        }

        [DataMember] private ObservableCollection<PuzzleGroupViewModel> _puzzles;
        public ObservableCollection<PuzzleGroupViewModel> PuzzleGroupViewModels
        {
            get { return _puzzles; }
            set
            {
                this.RaiseAndSetIfChanged(ref _puzzles, value);
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

    }
}
