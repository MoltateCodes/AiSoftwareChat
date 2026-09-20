using System;
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

            ["BYTE"] = new Character(
        "BYTE",
        "/CharImg/byte_profile.png"
    ),

            ["NIX"] = new Character(
        "NIX",
        "/CharImg/nix_profile.png"
    ),

            ["R-01"] = new Character(
        "R-01",
        "/CharImg/R-01_profile.png"
    )
        };

        private Character? CurrentCharacter;
        public MainWindow()
        {
            InitializeComponent();

            // Start with MIRA
            CurrentCharacter = Characters["MIRA"];

            ChatCharacterName.Text = CurrentCharacter.Name;
            CenterCharacterName.Text = CurrentCharacter.Name;

            LoadChatHistory();
        }

        private void Character_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Border characterBorder)
                return;

            string? characterName = characterBorder.Tag?.ToString();

            if (characterName == null)
                return;

            if (!Characters.TryGetValue(
                characterName,
                out Character? character))
                return;

            CurrentCharacter = character;

            // Change RIGHT chat header name
            ChatCharacterName.Text = character.Name;

            // Change RIGHT chat header image
            ChatCharacterImage.Fill = new ImageBrush
            {
                ImageSource = new BitmapImage(
                    new Uri(
                        $"pack://application:,,,{character.ProfileImage}",
                        UriKind.Absolute
                    )
                ),
                Stretch = Stretch.UniformToFill
            };

            // Change center name
            CenterCharacterName.Text = character.Name;

            // Load character chat
            LoadChatHistory();
        }
        private void LoadChatHistory()
        {
            // Remove messages currently displayed
            ChatMessages.Children.Clear();

            if (CurrentCharacter == null)
                return;

            // Display this character's saved messages
            foreach (ChatMessage chatMessage in CurrentCharacter.ChatHistory)
            {
                if (chatMessage.Sender == "User")
                {
                    AddUserMessage(chatMessage.Message);
                }
                else if (chatMessage.Sender == "AI")
                {
                    AddAIMessage(chatMessage.Message);
                }
            }

            ChatScrollViewer.ScrollToEnd();
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

            // Make sure a character is selected
            if (CurrentCharacter == null)
                return;

            // Save user message to current character
            CurrentCharacter.ChatHistory.Add(
                new ChatMessage("User", message)
            );

            // Display user message
            AddUserMessage(message);

            // Temporary AI response
            string aiResponse = "Hi! How can I help you?";

            // Save AI response to current character
            CurrentCharacter.ChatHistory.Add(
                new ChatMessage("AI", aiResponse)
            );

            // Display AI response
            AddAIMessage(aiResponse);

            // Clear input box
            MessageInput.Clear();

            // Scroll to latest message
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