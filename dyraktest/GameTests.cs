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

        [TestMethod]
        public void CanBeat_SameSuitHigher_ShouldBeat()
        {
            Game game = new Game();
            game.NewGame();

            Card lowCard = new Card("♠", "6", 6);
            Card highCard = new Card("♠", "10", 10);

            bool result = game.CanBeat(highCard, lowCard);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanBeat_LowerCard_ShouldNotBeat()
        {
            Game game = new Game();
            game.NewGame();

            Card highCard = new Card("♠", "10", 10);
            Card lowCard = new Card("♠", "6", 6);

            bool result = game.CanBeat(lowCard, highCard);
            Assert.IsFalse(result);
        }
    }
}