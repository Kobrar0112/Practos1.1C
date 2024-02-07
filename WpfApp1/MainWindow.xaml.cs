using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private bool isPlayerTurn = true;
        private bool gameEnded;
        private Button[] buttons;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            buttons = new Button[9] { Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8 };
            foreach (var button in buttons)
            {
                button.Content = "";
                button.IsEnabled = true;
            }
            gameEnded = false;
            isPlayerTurn = true; // Пользователь начинает первым
            ResultText.Text = "Начните игру! Вы играете крестиками.";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameEnded) return;

            var button = (Button)sender;
            if (button.Content.ToString() != "") return;

            if (isPlayerTurn) // Пользователь крест
            {
                button.Content = "X";
                CheckForWinner();
                if (!gameEnded) SwitchPlayer();
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame(); // Перезапуск 
        }

        private async void MakeComputerMove()
        {
            await Task.Delay(500);
            if (!isPlayerTurn) // Ход робота
            {
                var random = new Random();
                var availableButtons = buttons.Where(b => b.Content.ToString() == "").ToArray();
                if (availableButtons.Length > 0)
                {
                    var button = availableButtons[random.Next(availableButtons.Length)];
                    button.Content = "O"; // Робот ноль
                    CheckForWinner();
                    if (!gameEnded) SwitchPlayer();
                }
            }
        }

        private void CheckForWinner()
        {
            // Горизонталь
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i * 3].Content.ToString() != "" &&
                    buttons[i * 3].Content == buttons[i * 3 + 1].Content &&
                    buttons[i * 3 + 1].Content == buttons[i * 3 + 2].Content)
                {
                    EndGame(buttons[i * 3].Content.ToString());
                    return;
                }
            }

            // Вертикаль
            for (int i = 0; i < 3; i++)
            {
                if (buttons[i].Content.ToString() != "" &&
                    buttons[i].Content == buttons[i + 3].Content &&
                    buttons[i + 3].Content == buttons[i + 6].Content)
                {
                    EndGame(buttons[i].Content.ToString());
                    return;
                }
            }

            // Диагональ
            if (buttons[0].Content.ToString() != "" &&
                buttons[0].Content == buttons[4].Content &&
                buttons[4].Content == buttons[8].Content)
            {
                EndGame(buttons[0].Content.ToString());
                return;
            }
            if (buttons[2].Content.ToString() != "" &&
                buttons[2].Content == buttons[4].Content &&
                buttons[4].Content == buttons[6].Content)
            {
                EndGame(buttons[2].Content.ToString());
                return;
            }

            // Ничья
            if (buttons.All(b => b.Content.ToString() != ""))
            {
                ResultText.Text = "Ничья!";
                DisableButtons();
                return;
            }
        }

        private void EndGame(string winner)
        {
            ResultText.Text = winner == "X" ? "Победили крестики!" : "Ну что кожанный,выкусил!";
            gameEnded = true;
            DisableButtons();
        }

        private void DisableButtons()
        {
            foreach (var button in buttons)
            {
                button.IsEnabled = false;
            }
        }

        private void SwitchPlayer()
        {
            isPlayerTurn = !isPlayerTurn;
           
            if (!isPlayerTurn) MakeComputerMove();
        }
    }
}