using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using KrossKlient.DataModel;
using KrossKlient.Logging;
using KrossKlient.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Splat;

namespace KrossTests.ServicesTests
{
    [TestClass]
    public class PuzzlesServiceTests
    {
        [TestMethod]
        public async Task GetPuzzlesForUserShouldReturnValidDataForGivenUsername()
        {
            var blobCache = RegisterComponentsAndReturnInMemoryBlobCache();
            IPuzzlesService sut = new PuzzlesService(blobCache);
            var puzzleGroups = BuildTestPuzzleGroupData();
            blobCache.InsertObject("kross", puzzleGroups);
            var result = await sut.GetPuzzles();
            Assert.AreEqual(3, result.Count);
        }

        public List<PuzzleGroup> BuildTestPuzzleGroupData()
        {
            var puzzles = new List<PuzzleGroup>();
            var sciencegroup = new PuzzleGroup { Name = "Science", Puzzles = new List<PuzzleSubGroup>() };

            sciencegroup.Puzzles.Add(PuzzleBuilder("Human Skeleton Puzzles"));
            sciencegroup.Puzzles.Add(PuzzleBuilder("Resperatory System"));
            sciencegroup.Puzzles.Add(PuzzleBuilder("Muscle System"));

            puzzles.Add(sciencegroup);

            var englishgroup = new PuzzleGroup { Name = "English", Puzzles = new List<PuzzleSubGroup>() };

            englishgroup.Puzzles.Add(PuzzleBuilder("English Vocabs Puzzles"));
            englishgroup.Puzzles.Add(PuzzleBuilder("Grammer"));
            puzzles.Add(englishgroup);

            var geographygroup = new PuzzleGroup { Name = "Geography", Puzzles = new List<PuzzleSubGroup>() };
            geographygroup.Puzzles.Add(PuzzleBuilder("Rivers Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Tectonic Plates Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Polution Puzzles"));
            geographygroup.Puzzles.Add(PuzzleBuilder("Volcanoes Puzzles"));
            puzzles.Add(geographygroup);
            return puzzles;
        }

        public PuzzleSubGroup PuzzleBuilder(string title)
        {
            var puzzleVm = new PuzzleSubGroup()
            {
                Title = title,
                Words = new Dictionary<string, string>
                {

                    {"First", "The first"},
                    {"Second", "The Second"},
                    {"Third", "The Third"},
                    {"Forth", "The Forth"},
                    {"Fifth", "The Fifth"},
                    {"Sixth", "The Sixth"}
                }
            };
            return puzzleVm;
        }

        public static InMemoryBlobCache RegisterComponentsAndReturnInMemoryBlobCache()
        {
            EventListener verboseListener = new StorageFileEventListener("MyListenerVerbose1");
            EventListener informationListener = new StorageFileEventListener("MyListenerInformation1");

            verboseListener.EnableEvents(KrossEventSource.Log, EventLevel.Error);
            Locator.CurrentMutable.RegisterConstant(KrossEventSource.Log, typeof (KrossEventSource));

            var blobCache = new InMemoryBlobCache();
            Locator.CurrentMutable.RegisterConstant(blobCache, typeof (IBlobCache), "UserAccount");
            Locator.CurrentMutable.Register(() => new KrossLogger(), typeof (ILogger));

            return blobCache;
        }

        public object x { get; set; }
    }
}