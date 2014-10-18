using KrossKlient.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace KrossTests.ViewModels
{
    [TestClass]
    public class MatchesPlayedViewModelTests
    {
        [TestMethod]
        public void ShouldRetrieveGamesOnVmLoad()
        {
            var fixture = new HomePageViewModel();
            Assert.IsNotNull(fixture.PuzzleGroupData, "Puzzle data not initialised");
        }
    }
}