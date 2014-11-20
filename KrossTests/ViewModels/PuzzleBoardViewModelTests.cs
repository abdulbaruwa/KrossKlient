using KrossKlient.Services;
using KrossKlient.ViewModels;
using KrossKlient.ViewModels.DesignTime;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Splat;

namespace KrossTests.ViewModels
{
    [TestClass]
    public class PuzzleBoardViewModelTests
    {
        [TestInitialize]
        public void InstantiateTest()
        {
            GetResolver();
        }


        [TestMethod]
        public void WhenInstatiateBoardForAGameShouldLoadGameFromService()
        {
        }

        private void GetResolver()
        {
            if (this.Resolver != null) return;
            Resolver = Locator.CurrentMutable;
            Resolver.Register(() => new FakePuzzlesService(), typeof(IPuzzlesService));
            Resolver.Register(() => new UserService(), typeof(IUserService));
            Resolver.Register(() => new FakePuzzleRepository(), typeof(IPuzzleRepository));
        }

        public IMutableDependencyResolver Resolver { get; set; }
    }
}
