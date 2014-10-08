using System.Collections.Generic;
using KrossKlient.DataModel;

namespace KrossKlient.Services
{
    public interface IPuzzleRepository
    {
        void AddPuzzleRepositoryPath(string repositoryDbPath);
        Dictionary<string,string> GetPuzzleWithId(int puzzleId, string userName);
        List<PuzzleGroup> GetPuzzles(string userName);
        void UpdateGameData(List<PuzzleGroup> puzzleGroupData, string userName);
    }
}