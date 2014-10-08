using System;
using System.Collections.Generic;
using KrossKlient.DataModel;
using KrossKlient.Services;

namespace KrossKlient.ViewModels.DesignTime
{
    public class FakePuzzleRepository : IPuzzleRepository
    {
        private Dictionary<string,string> _words = null;

        public Dictionary<string,string> GetPuzzleWithId(int puzzleId,string userName)
        {
            return _words ?? new Dictionary<string, string>
                                 {
                                     {"Bamidele", "Adetoro's first name"},
                                     {"station", "place where i fit get train"},
                                     {"india", "Origin of my favourite curry"},
                                     {"Adams", "Captain Arsenal"},
                                     {"fards", "show off"},
                                     {"novemb", "like november"},
                                     {"belt", "Tied around my waist"},
                                     {"train", "Mode of transportation"},
                                     {"adeola", "My sister"},
                                     {"amoeba", "Single cell organism"},
                                     {"moscow", "Cold city behind iron curtain"}
                                 };
        }

        public void AddWord(Dictionary<string,string> words)
        {
            _words = words;
        }

        public void AddPuzzleRepositoryPath(string repositoryDbPath)
        {
        }

        public List<PuzzleGroup> GetPuzzles(string userName)
        {
            var puzzleGroups = new List<PuzzleGroup>();
            return puzzleGroups;
        }

        public void UpdateGameData(List<PuzzleGroup> puzzleGroupData, string userName)
        {
            throw new NotImplementedException();
        }
    }
}