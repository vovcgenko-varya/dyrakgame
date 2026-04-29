using System;
using System.Collections.Generic;

namespace DurakLibrary
{
    public class Game
    {
        public List<Card> Deck = new List<Card>();
        public List<Card> HumanHand = new List<Card>();
        public List<Card> ComputerHand = new List<Card>();
        public List<Card> TableCards = new List<Card>();
        public Card TrumpCard;
        public bool HumanTurn = true;
        public bool IsOver = false;
        public string Message = "";
        private Random rnd = new Random();

        public void NewGame()
        {
            Deck.Clear();
            HumanHand.Clear();
            ComputerHand.Clear();
            TableCards.Clear();

            string[] suits = { "♠", "♣", "♦", "♥" };
            string[] ranks = { "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            int[] values = { 6, 7, 8, 9, 10, 11, 12, 13, 14 };

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = 0; j < ranks.Length; j++)
                {
                    Card card = new Card(suits[i], ranks[j], values[j]);
                    Deck.Add(card);
                }
            }

            Shuffle();
            TrumpCard = Deck[Deck.Count - 1];

            for (int i = 0; i < 6; i++)
            {
                HumanHand.Add(Deck[0]);
                Deck.RemoveAt(0);
                ComputerHand.Add(Deck[0]);
                Deck.RemoveAt(0);
            }

            HumanTurn = true;
            IsOver = false;
            Message = "Ваш ход. Выберите карту.";
        }

        private void Shuffle()
        {
            for (int i = 0; i < Deck.Count; i++)
            {
                int j = rnd.Next(Deck.Count);
                Card temp = Deck[i];
                Deck[i] = Deck[j];
                Deck[j] = temp;
            }
        }

        public bool CanBeat(Card defender, Card attacker)
        {
            if (defender.Suit == attacker.Suit && defender.Value > attacker.Value)
                return true;
            if (defender.Suit == TrumpCard.Suit && attacker.Suit != TrumpCard.Suit)
                return true;
            return false;
        }

        public bool HumanAddCard(Card card)
        {
            if (IsOver) return false;
            if (!HumanTurn) return false;

            HumanHand.Remove(card);

            if (TableCards.Count == 0)
            {
                TableCards.Add(card);
                Message = "Компьютер должен отбиться.";
                HumanTurn = false;
                return true;
            }

            bool canAdd = false;
            for (int i = 0; i < TableCards.Count; i++)
            {
                if (TableCards[i].Value == card.Value)
                {
                    canAdd = true;
                    break;
                }
            }

            if (!canAdd)
            {
                HumanHand.Add(card);
                Message = "Нельзя подкинуть эту карту!";
                return false;
            }

            TableCards.Add(card);
            Message = "Карта подкинута.";
            return true;
        }

        public void ComputerDefend()
        {
            for (int i = TableCards.Count - 1; i >= 0; i--)
            {
                Card attackCard = TableCards[i];
                Card bestCard = null;

                for (int j = 0; j < ComputerHand.Count; j++)
                {
                    if (CanBeat(ComputerHand[j], attackCard))
                    {
                        if (bestCard == null || ComputerHand[j].Value < bestCard.Value)
                        {
                            bestCard = ComputerHand[j];
                        }
                    }
                }

                if (bestCard != null)
                {
                    ComputerHand.Remove(bestCard);
                    TableCards.RemoveAt(i);
                }
                else
                {
                    ComputerHand.AddRange(TableCards);
                    TableCards.Clear();
                    Message = "Компьютер забирает карты. Ваш ход.";
                    HumanTurn = true;
                    FillHands();
                    return;
                }
            }

            Message = "Компьютер отбился. Ваш ход.";
            HumanTurn = true;
            FillHands();
        }

        public void ComputerMove()
        {
            Card cardToMove = ComputerHand[0];
            for (int i = 1; i < ComputerHand.Count; i++)
            {
                if (ComputerHand[i].Value < cardToMove.Value)
                {
                    cardToMove = ComputerHand[i];
                }
            }

            ComputerHand.Remove(cardToMove);
            TableCards.Add(cardToMove);
            Message = "Компьютер походил: " + cardToMove + ". Отбивайтесь или нажмите Взять.";
            HumanTurn = false;
        }

        public bool HumanDefend(Card myCard, Card attackCard)
        {
            if (!CanBeat(myCard, attackCard))
            {
                Message = "Эта карта не может побить!";
                return false;
            }

            HumanHand.Remove(myCard);
            TableCards.Remove(attackCard);
            Message = "Отбито!";

            if (TableCards.Count == 0)
            {
                Message = "Вы отбились! Ход компьютера.";
                HumanTurn = true;
                FillHands();
                ComputerMove();
            }

            return true;
        }

        public void HumanTakeCards()
        {
            HumanHand.AddRange(TableCards);
            TableCards.Clear();
            Message = "Вы забрали карты. Компьютер ходит.";
            HumanTurn = true;
            FillHands();
            ComputerMove();
        }

        public void FillHands()
        {
            while (HumanHand.Count < 6 && Deck.Count > 0)
            {
                HumanHand.Add(Deck[0]);
                Deck.RemoveAt(0);
            }
            while (ComputerHand.Count < 6 && Deck.Count > 0)
            {
                ComputerHand.Add(Deck[0]);
                Deck.RemoveAt(0);
            }

            if (Deck.Count == 0)
            {
                if (HumanHand.Count == 0)
                {
                    IsOver = true;
                    Message = "Вы победили!";
                }
                else if (ComputerHand.Count == 0)
                {
                    IsOver = true;
                    Message = "Компьютер победил!";
                }
            }
        }
    }
}