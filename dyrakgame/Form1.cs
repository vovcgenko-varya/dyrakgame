using System;
using System.Drawing;
using System.Windows.Forms;
using DurakLibrary;

namespace dyrakgame
{
    public partial class Form1 : Form
    {
        private readonly Game game;
        private readonly Label labelTrump;
        private readonly Label labelMessage;
        private readonly FlowLayoutPanel panelHuman;
        private readonly FlowLayoutPanel panelTable;
        private readonly Button buttonTake;
        private readonly Button buttonNewGame;
        private Card selectedAttackCard;

        public Form1()
        {
            this.Text = "Дурак";
            this.Width = 800;
            this.Height = 600;

            labelTrump = new Label();
            labelTrump.Top = 10;
            labelTrump.Left = 10;
            labelTrump.Width = 400;
            labelTrump.Text = "Козырь: ";

            labelMessage = new Label();
            labelMessage.Top = 35;
            labelMessage.Left = 10;
            labelMessage.Width = 600;
            labelMessage.Text = "";

            panelHuman = new FlowLayoutPanel();
            panelHuman.Top = 400;
            panelHuman.Left = 10;
            panelHuman.Width = 760;
            panelHuman.Height = 120;
            panelHuman.BorderStyle = BorderStyle.FixedSingle;
            panelHuman.AutoScroll = true;

            panelTable = new FlowLayoutPanel();
            panelTable.Top = 100;
            panelTable.Left = 10;
            panelTable.Width = 760;
            panelTable.Height = 250;
            panelTable.BorderStyle = BorderStyle.FixedSingle;
            panelTable.AutoScroll = true;

            buttonTake = new Button();
            buttonTake.Text = "Взять карты";
            buttonTake.Top = 360;
            buttonTake.Left = 10;
            buttonTake.Width = 120;
            buttonTake.Click += ButtonTake_Click;

            buttonNewGame = new Button();
            buttonNewGame.Text = "Новая игра";
            buttonNewGame.Top = 360;
            buttonNewGame.Left = 140;
            buttonNewGame.Width = 120;
            buttonNewGame.Click += ButtonNewGame_Click;

            this.Controls.Add(labelTrump);
            this.Controls.Add(labelMessage);
            this.Controls.Add(panelHuman);
            this.Controls.Add(panelTable);
            this.Controls.Add(buttonTake);
            this.Controls.Add(buttonNewGame);

            game = new Game();
            game.NewGame();
            UpdateScreen();
        }

        private void UpdateScreen()
        {
            panelHuman.Controls.Clear();
            panelTable.Controls.Clear();

            labelTrump.Text = "Козырь: " + game.TrumpCard + " | В колоде: " + game.Deck.Count;
            labelMessage.Text = game.Message;

            for (int i = 0; i < game.HumanHand.Count; i++)
            {
                Card card = game.HumanHand[i];
                Button btn = new Button();
                btn.Text = card.ToString();
                btn.Width = 60;
                btn.Height = 85;
                btn.BackColor = Color.White;
                btn.Tag = card;
                btn.Click += HumanCardClick;
                panelHuman.Controls.Add(btn);
            }

            for (int i = 0; i < game.TableCards.Count; i++)
            {
                Card card = game.TableCards[i];
                Button btn = new Button();
                btn.Text = card.ToString();
                btn.Width = 60;
                btn.Height = 85;
                btn.BackColor = Color.LightYellow;
                btn.Tag = card;
                btn.Click += TableCardClick;
                panelTable.Controls.Add(btn);
            }

            if (game.IsOver)
            {
                MessageBox.Show(game.Message, "Конец игры");
                game.NewGame();
                UpdateScreen();
            }
        }

        private void HumanCardClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Card card = (Card)btn.Tag;

            if (game.HumanTurn)
            {
                game.HumanAddCard(card);
                if (!game.HumanTurn)
                {
                    game.ComputerDefend();
                }
                UpdateScreen();
            }
            else
            {
                if (selectedAttackCard == null)
                {
                    labelMessage.Text = "Сначала выберите карту со стола!";
                    return;
                }

                game.HumanDefend(card, selectedAttackCard);
                selectedAttackCard = null;
                UpdateScreen();
            }
        }

        private void TableCardClick(object sender, EventArgs e)
        {
            if (!game.HumanTurn)
            {
                Button btn = (Button)sender;
                selectedAttackCard = (Card)btn.Tag;
                labelMessage.Text = "Выбрана: " + selectedAttackCard + ". Теперь выберите свою карту.";
            }
        }

        private void ButtonTake_Click(object sender, EventArgs e)
        {
            if (!game.HumanTurn && game.TableCards.Count > 0)
            {
                game.HumanTakeCards();
                UpdateScreen();
            }
        }

        private void ButtonNewGame_Click(object sender, EventArgs e)
        {
            game.NewGame();
            UpdateScreen();
        }
    }
}