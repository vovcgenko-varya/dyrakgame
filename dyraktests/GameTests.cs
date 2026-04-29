using Microsoft.VisualStudio.TestTools.UnitTesting;
using DurakLibrary;

namespace DurakTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void NewGame_ShouldGiveSixCards()
        {
            Game game = new Game();
            game.NewGame();
            Assert.AreEqual(6, game.HumanHand.Count);
            Assert.AreEqual(6, game.ComputerHand.Count);
        }

        [TestMethod]
        public void NewGame_ShouldHaveTrumpCard()
        {
            Game game = new Game();
            game.NewGame();
            Assert.IsNotNull(game.TrumpCard);
        }
    }
}