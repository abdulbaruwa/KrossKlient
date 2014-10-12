using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using KrossKlient.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace KrossTests.BuilderTests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void InitiatedBoard()
        {
            var board = new Board(12);
            Assert.IsNotNull(board.Grids);
            Assert.AreEqual(board.Grids.Length, 144);
        }

        [TestMethod]
        public void QuickTest()
        {
            var x = new ViewModelLocator();
            var y = x.PuzzleBoardViewModel;
        }

        [TestMethod]
        public void AddNewWordToBoard()
        {
            var board = new Board(12);
            board.AddWord("First");
            Assert.AreEqual(board.Grids[0, 0], "F");
            Assert.AreEqual(board.Grids[0, 1], "i");
            Assert.AreEqual(board.Grids[0, 2], "r");
            Assert.AreEqual(board.Grids[0, 3], "s");
            Assert.AreEqual(board.Grids[0, 4], "t");

            PrintBoard(board);
        }


        [TestMethod]
        public void AddASecondWordVerticalIfFirstLetterMatches()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            Assert.AreEqual("r", board.Grids[0, 2]);
            Assert.AreEqual("e", board.Grids[1, 2]);
            Assert.AreEqual("s", board.Grids[2, 2]);
            Assert.AreEqual("t", board.Grids[3, 2]);
            Assert.AreEqual("o", board.Grids[4, 2]);
            Assert.AreEqual("r", board.Grids[5, 2]);
            Assert.AreEqual("e", board.Grids[6, 2]);
            PrintBoard(board);
        }

        [TestMethod]
        public void ShouldReturnAllSetCellsWhenRequested()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");
            IEnumerable<Cell> cellsWithVals = board.GetLoadedCells();
            Assert.AreEqual(11, cellsWithVals.Count());
        }

        [TestMethod]
        public void ShouldReturnAllSetCellsWithStartCellsIdentified()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");
            IEnumerable<Cell> cellsWithVals = board.GetLoadedCells();
            Assert.AreEqual(11, cellsWithVals.Count());
            Assert.AreEqual(2, cellsWithVals.Count(x => x.IsFirstLetter));
        }

        [TestMethod, Ignore]
        public void AddWordVerticalShouldFailIfThereIsCharInSuffixCell()
        {
            var board = new Board(12);
            board.Grids[7, 2] = "x";
            board.AddWord("first");
            bool wordAdded = board.AddWord("restore");
            Assert.IsFalse(wordAdded);
        }

        [TestMethod]
        public void AddWordVerticalShouldFailIfThereIsCharInPrefixCell()
        {
            var board = new Board(12);
            board.Grids[7, 2] = "x";
            board.AddWord("first");
            bool wordAdded = board.AddWord("restore");
            Assert.IsFalse(wordAdded);
        }

        //Test to determine cells with letters with match the current word to be inserted.
        //// If we know the direction of the existing word, say vertical, we can only then add new word horizonally
        [TestMethod]
        public void UnitTest_ShouldReturnCellsWithMatchesWithCurrentWordToBeInserted()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            Assert.AreEqual(4, board.GetLetterMatchesFor("race").Count());
        }

        // Given a cell that is vertically occupied
        // That is Not in the first letter of a word
        // That is Not a junction letter
        // Test should assert a new word can only be inserted Horizonally

        [TestMethod]
        public void UnitTest_ShouldReturnHorizonalIfCellIsVerticallyUsedOnBoard()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            Assert.IsTrue(board.IsCellVerticallyOccupied(board.CellBoard[2, 2]));
        }

        [TestMethod]
        public void ShouldInsertWordHorizontallyIfMatchExistVertically()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            //Act; word to add horizontally
            board.AddHorizontally("brace");
            Assert.IsNotNull(board.CellBoard[5, 1]);
            Assert.IsNotNull(board.CellBoard[5, 1].WordH);
        }

        [TestMethod]
        public void AddWordHorizontallyShouldFailIfThereIsCharInPrefixCell()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            board.Grids[5, 0] = "x";
            var cell = new Cell
            {
                Character = "x",
                Col = 0,
                Row = 5
            };
            board.CellBoard[5, 0] = cell;

            //Act; word to add horizontally
            board.AddHorizontally("brace");
            Assert.IsNull(board.CellBoard[5, 1]);
        }

        [TestMethod]
        public void AddWordHorizontallyShouldFailIfThereIsCharInSuffixCell()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            board.Grids[5, 6] = "x";
            var cell = new Cell
            {
                Character = "x",
                Col = 6,
                Row = 5
            };
            board.CellBoard[5, 6] = cell;

            //Act; word to add horizontally
            board.AddHorizontally("brace");
            Assert.IsNull(board.CellBoard[5, 1]);
        }

        [TestMethod]
        public void AddWordHorizontallyShouldFailIfThereIsCharInACellBelow()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            board.Grids[6, 5] = "x";
            var cell = new Cell
            {
                Character = "x",
                Row = 6,
                Col = 5,
            };
            board.CellBoard[6, 5] = cell;

            //Act; word to add horizontally
            board.AddHorizontally("brace");
            Assert.IsNull(board.CellBoard[5, 1]);
        }

        [TestMethod]
        public void AddWordHorizontallyShouldFailIfThereIsCharInACellAbove()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            board.Grids[4, 5] = "x";
            var cell = new Cell
            {
                Character = "x",
                Row = 4,
                Col = 5,
            };
            board.CellBoard[4, 5] = cell;

            //Act; word to add horizontally
            board.AddHorizontally("brace");
            Assert.IsNull(board.CellBoard[5, 1]);
        }

        [TestMethod]
        public void ShouldInsertWordVerticallyIfMatchExistHorizontally()
        {
            var board = new Board(12);
            board.AddWord("first");
            board.AddWord("restore");

            //Act; word to add horizontally
            board.AddHorizontally("brace");
            PrintBoard(board);

            board.AddVertically("lack");
            Assert.IsNotNull(board.CellBoard[5, 1]);
            Assert.IsNotNull(board.CellBoard[5, 1].WordH);
            PrintBoard(board);
        }

        [TestMethod]
        public void AddFirstAndSecond_with_new_call()
        {
            var board = new Board(12);
            board.AddWord2("first");
            board.AddWord2("restore");

            PrintBoard(board);
        }

        [TestMethod]
        public void ShouldProcessAGroupOfWordsSucccessfully()
        {
            var board = new Board(12);
            var wordlist = new List<string>();
            wordlist.Add("first");
            wordlist.Add("restore");
            wordlist.Add("brace");
            wordlist.Add("beebs");
            wordlist.Add("skateboard");
            wordlist.Add("ShouldProcessAGroupOfWordsSucccessfully");
            wordlist.Add("broke");
            wordlist.Add("eko");
            wordlist.Add("straped");
            //wordlist.Add("others");

            List<string> result = board.ProcessWords(wordlist.ToArray());
            PrintBoard(board);
        }        
        
        [TestMethod]
        public void ShouldNotAddWordVerticallyIfMatchPositionOverunsGrid()
        {
            var board = new Board(12);
            var wordlist = new List<string>();
            wordlist.Add("kettledrum");
            wordlist.Add("embezzler");
            wordlist.Add("gladioli");

            // Board after first 3 words are added
            //k e t t l e d r u m - -
            // - m - - - - - - - - - -
            // - b - - - - - - - - - -
            // - e - - - - - - - - - -
            // - z - - - - - - - - - -
            // - z - - - - - - - - - -
            // g l a d i o l i - - - -   <- Attempt to add 'platted' here on 'L', will overun the grid
            // - e - - - - - - - - - -
            // - r - - - - - - - - - -
            // - - - - - - - - - - - -
            // - - - - - - - - - - - -
            // - - - - - - - - - - - -
            
            wordlist.Add("platted");

            List<string> result = board.ProcessWords(wordlist.ToArray());
            PrintBoard(board);
        }

        [TestMethod]
        public void ShouldAddWordHorizontallyOnTheLastRow()
        {
            var board = new Board(12);
            var wordlist = new List<string>();
            wordlist.Add("embezzler");
            wordlist.Add("zodiaclights");    
            wordlist.Add("szub");
            wordlist.Add("grrrrrrk");
            wordlist.Add("pook");

            List<string> result = board.ProcessWords(wordlist.ToArray());
            PrintBoard(board);
        }

        [TestMethod]
        public void ShouldAddWordVerticallyOnTheLastCol()
        {
            var board = new Board(12);
            var wordlist = new List<string>();
            wordlist.Add("embezzler");
            wordlist.Add("zodiaclights");    
            wordlist.Add("szub");

            List<string> result = board.ProcessWords(wordlist.ToArray());
            PrintBoard(board);
        }


        [TestMethod]
        public void ShouldProcessWordsAddWordHorizontallyOnTheLastRowTest()
        {
            var board = new Board(12);
            var wordlist = new List<string>();
            wordlist.Add("kettledrum");
            wordlist.Add("cabaret");
            wordlist.Add("shopping");
            wordlist.Add("embezzler");
            wordlist.Add("automatic");
            wordlist.Add("gladioli");
            wordlist.Add("competition");
            wordlist.Add("sargent");
            wordlist.Add("sieve");
            wordlist.Add("compass");
            wordlist.Add("playwright");
            wordlist.Add("trombone");
            wordlist.Add("bus");
            wordlist.Add("bre");
            wordlist.Add("encyclopaedia");
            wordlist.Add("ewes");
            wordlist.Add("rand");
            wordlist.Add("plotted");
            wordlist.Add("can");
            wordlist.Add("perennials");
            wordlist.Add("catapult");
            wordlist.Add("retort");
            wordlist.Add("dubbin");
            wordlist.Add("separating");
            wordlist.Add("accelerator");
            wordlist.Add("aisle");
            wordlist.Add("castanets");
            wordlist.Add("rescuing");
            wordlist.Add("retrieve");
            wordlist.Add("aphid"); ;

            List<string> result = board.ProcessWords(wordlist.ToArray());
            PrintBoard(board);
        }

        [TestMethod]
        public void ShouldAddWordVerticallyThatIsTwelveCharsLong()
        {
            var board = new Board(12);
            var wordlist = new List<string>();
            wordlist.Add("embezzler");
            wordlist.Add("zodiaclights");

            // Board after first 3 words are added
            //k e t t l e d r u m - -
            // - m - - - - - - - - - -
            // - b - - - - - - - - - -
            // - e - - - - - - - - - -
            // - z - - - - - - - - - -
            // - z - - - - - - - - - -
            // g l a d i o l i - - - -   <- Attempt to add 'platted' here on 'L', will overun the grid
            // - e - - - - - - - - - -
            // - r - - - - - - - - - -
            // - - - - - - - - - - - -
            // - - - - - - - - - - - -
            // - - - - - - - - - - - -
            
            List<string> result = board.ProcessWords(wordlist.ToArray());
            PrintBoard(board);
        }

        private async Task<StorageFile> GetPackagedFile(string folderName, string fileName)
        {
            var installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            if (folderName != null)
            {
                StorageFolder subFolder = await installFolder.GetFolderAsync(folderName);
                return await subFolder.GetFileAsync(fileName);
            }
            return await installFolder.GetFileAsync(fileName);
        }

        [TestMethod]
        public async void ShouldInsertFromGivenSetOfWords()
        {
            var file = await GetPackagedFile("BuilderTests", "Words.txt");
            var wordsAndHints = await FileIO.ReadLinesAsync(file);
            var wordDic = wordsAndHints.Select(fileDataInLine => fileDataInLine.Split(new[] { '|' }))
                                 .ToDictionary(lineArray =>  (lineArray[0]).TrimEnd().ToLower(), lineArray => lineArray[1]);

            var board = new Board(12);
            var wordlist = wordDic.Select(x => x.Key).Take(30).ToArray();

            var failed = board.ProcessWords(wordlist);
            PrintBoard(board);
            Debug.WriteLine("Failed to insert {0} from the list of {1} words", failed.Count, wordlist.Length);
            Assert.AreNotEqual(failed.Count, wordlist.Length);
        }

        private void PrintBoard(Board board)
        {
            for (int i = 0; i < board.CellBoard.GetLength(0); i++)
            {
                string row = "";
                for (int j = 0; j < board.CellBoard.GetLength(1); j++)
                {
                    if (board.CellBoard[i, j] == null)
                    {
                        row = row + " " + "-";
                    }
                    else
                    {
                        row = row + " " + board.CellBoard[i, j].Character;
                    }
                }
                Debug.WriteLine(row);
            }
        }
    }
}
