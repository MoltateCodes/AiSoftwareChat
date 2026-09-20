using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Character> Characters = new()
        {
            ["MIRA"] = new Character(
        "MIRA",
        "/CharImg/mira_profile.png"
    ),

            ["NOVA"] = new Character(
        "NOVA",
        "/CharImg/mira_profile.png"
    ),

            ["LUNA"] = new Character(
        "LUNA",
        "/CharImg/mira_profile.png"
    ),

            ["ARIA"] = new Character(
        "ARIA",
        "/CharImg/mira_profile.png"
    )
        };
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Character_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Border characterBorder)
                return;

            string? characterName = characterBorder.Tag?.ToString();

            if (characterName == null)
                return;

            if (!Characters.TryGetValue(characterName, out Character? character))
                return;

            ChatCharacterName.Text = character.Name;
            CenterCharacterName.Text = character.Name;
        }
        private void MessageInput_KeyDown(
            object sender,
            System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SendButton_Click(sender, new RoutedEventArgs());
                e.Handled = true;
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text.Trim();

            // Don't send an empty message
            if (string.IsNullOrWhiteSpace(message))
                return;

            AddUserMessage(message);

            // Temporary AI response
            AddAIMessage("Hi! How can I help you?");

            // Clear input box
            MessageInput.Clear();

            // Scroll to the latest message
            ChatScrollViewer.ScrollToEnd();
        }


        private void AddUserMessage(string message)
        {
            Border messageBubble = new Border
            {
                Background = new SolidColorBrush(
        Color.FromRgb(51, 68, 90)
    ),

                CornerRadius = new CornerRadius(12),

                Padding = new Thickness(14, 10, 14, 10),

                Margin = new Thickness(40, 0, 0, 10),

                HorizontalAlignment = HorizontalAlignment.Right,

                MaxWidth = 500,

                Child = new TextBlock
                {
                    Text = message,
                    Foreground = Brushes.White,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap
                }
            };


            ChatMessages.Children.Add(messageBubble);
        }
        private void AddAIMessage(string message)
        {
            Border messageBubble = new Border
            {
                Background = new SolidColorBrush(
                    Color.FromRgb(37, 43, 54)
                ),

                CornerRadius = new CornerRadius(12),

                Padding = new Thickness(14, 10, 14, 10),

                Margin = new Thickness(0, 0, 40, 10),

                HorizontalAlignment = HorizontalAlignment.Left,

                MaxWidth = 500,

                Child = new TextBlock
                {
                    Text = message,
                    Foreground = Brushes.White,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap
                }
            };

            ChatMessages.Children.Add(messageBubble);
        }
    }
}