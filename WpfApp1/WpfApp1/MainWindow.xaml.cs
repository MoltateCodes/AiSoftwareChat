using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;

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
        private Border? SelectedCharacterBorder;
        public MainWindow()
        {

            InitializeComponent();

            SourceInitialized += MainWindow_SourceInitialized;
            // Start with MIRA
            CurrentCharacter = Characters["MIRA"];

            ChatCharacterName.Text = CurrentCharacter.Name;
            CenterCharacterName.Text = CurrentCharacter.Name;

            LoadChatHistory();
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
    IntPtr hwnd,
    int dwAttribute,
    ref int pvAttribute,
    int cbAttribute);

        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWA_TEXT_COLOR = 36;

        private static int ColorToBgr(byte r, byte g, byte b)
        {
            return (b << 16) | (g << 8) | r;
        }

        private void MainWindow_SourceInitialized(object? sender, EventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;

            // Window title bar background
            int captionColor = ColorToBgr(16, 19, 24);

            // Window title text
            int textColor = ColorToBgr(255, 255, 255);

            DwmSetWindowAttribute(
                hwnd,
                DWMWA_CAPTION_COLOR,
                ref captionColor,
                sizeof(int));

            DwmSetWindowAttribute(
                hwnd,
                DWMWA_TEXT_COLOR,
                ref textColor,
                sizeof(int));
        }

        private void Character_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Border characterBorder)
                return;

            // Remove selection from previous character
            // Remove previous selection
            if (SelectedCharacterBorder != null)
            {
                SelectedCharacterBorder.Background = Brushes.Transparent;
                SelectedCharacterBorder.BorderBrush = Brushes.Transparent;
                SelectedCharacterBorder.BorderThickness = new Thickness(1);
            }

            string? characterName = characterBorder.Tag?.ToString();

            if (characterName == null)
                return;


            // Different colors for each character
            switch (characterName)
            {
                case "MIRA":

                    characterBorder.Background = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1),

                        GradientStops =
            {
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#D9467A"),
                    0.0
                ),
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#9E315A"),
                    0.5
                ),
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#541B32"),
                    1.0
                )
            }
                    };

                    characterBorder.BorderBrush =
                        new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#8FB7FF")
                        );

                    break;


                case "BYTE":

                    characterBorder.Background = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1),

                        GradientStops =
            {
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#684B32"),
                    0.0
                ),
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#493421"),
                    0.5
                ),
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#241A12"),
                    1.0
                )
            }
                    };

                    characterBorder.BorderBrush =
                        new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#E5A86B")
                        );

                    break;


                case "NIX":

                    characterBorder.Background = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1),

                        GradientStops =
                        {
                            new GradientStop(
                                (Color)ColorConverter.ConvertFromString("#3B82F6"),
                                0.0
                                ),
                            new GradientStop(
                                (Color)ColorConverter.ConvertFromString("#06008D"),
                                0.5
                                 ),
                            new GradientStop(
                                (Color)ColorConverter.ConvertFromString("#050147"),
                                1.0
                                 )
                            }
                    };

                    characterBorder.BorderBrush =
                        new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#76D6E8")
                        );

                    break;


                case "R-01":

                    characterBorder.Background = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1),

                        GradientStops =
            {
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#374B55"),
                    0.0
                ),
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#26363E"),
                    0.5
                ),
                new GradientStop(
                    (Color)ColorConverter.ConvertFromString("#11191D"),
                    1.0
                )
            }
                    };

                    characterBorder.BorderBrush =
                        new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString("#76D6E8")
                        );

                    break;
            }

            characterBorder.BorderThickness = new Thickness(1);

            SelectedCharacterBorder = characterBorder;

            // Highlight selected character with gradient background
            


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
    KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, new RoutedEventArgs());

                e.Handled = true;
            }
        }
        private void MessageInput_GotFocus(object sender, RoutedEventArgs e)
        {
            MessagePlaceholder.Visibility = Visibility.Collapsed;
            MessageInput.Foreground = Brushes.White;
        }

        private void MessageInput_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            MessagePlaceholder.Visibility =
                string.IsNullOrEmpty(MessageInput.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }
        private void MessageInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                MessagePlaceholder.Visibility = Visibility.Visible;
                MessageInput.Foreground = new SolidColorBrush(
                    Color.FromRgb(107, 114, 128)
                );
            }
        }



        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Get message
            string message = MessageInput.Text.Trim();

            // Don't send an empty message
            if (string.IsNullOrWhiteSpace(message))
                return;

            // Make sure a character is selected
            if (CurrentCharacter == null)
                return;

            // Save user message
            CurrentCharacter.ChatHistory.Add(
                new ChatMessage("User", message)
            );

            // Display user message immediately
            AddUserMessage(message);

            // Clear input
            MessageInput.Clear();

            // Scroll to latest message
            ChatScrollViewer.ScrollToEnd();


            // --------------------------------
            // THINKING DELAY
            // --------------------------------

            await Task.Delay(500);


            // --------------------------------
            // SHOW "MIRA IS TYPING..."
            // --------------------------------

            ShowTypingIndicator();


            // --------------------------------
            // TYPING DELAY
            // --------------------------------

            int typingDelay = Math.Clamp(
                700 + (message.Length * 30),
                700,
                3500
            );

            await Task.Delay(typingDelay);


            // --------------------------------
            // HIDE TYPING INDICATOR
            // --------------------------------

            HideTypingIndicator();


            // --------------------------------
            // TEMPORARY AI RESPONSE
            // --------------------------------

            string aiResponse = "Hi! How can I help you?";


            // Save AI response
            CurrentCharacter.ChatHistory.Add(
                new ChatMessage("AI", aiResponse)
            );

            // Display AI response
            AddAIMessage(aiResponse);

            // Scroll to latest message
            ChatScrollViewer.ScrollToEnd();
        }
        private void ShowTypingIndicator()
        {
            if (CurrentCharacter == null)
                return;

            TypingCharacterName.Text = CurrentCharacter.Name;
            TypingIndicator.Visibility = Visibility.Visible;

            ChatScrollViewer.ScrollToEnd();
        }

        private void HideTypingIndicator()
        {
            TypingIndicator.Visibility = Visibility.Collapsed;
        }

        private void AnimateMessage(UIElement element)
        {
            // Start slightly below its final position
            element.RenderTransform = new TranslateTransform(0, 25);
            element.Opacity = 0;

            // Slide up
            DoubleAnimation slideAnimation = new DoubleAnimation
            {
                From = 25,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            // Fade in
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            TranslateTransform transform =
                (TranslateTransform)element.RenderTransform;

            transform.BeginAnimation(
                TranslateTransform.YProperty,
                slideAnimation
            );

            element.BeginAnimation(
                UIElement.OpacityProperty,
                fadeAnimation
            );
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
            AnimateMessage(messageBubble);
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
            AnimateMessage(messageBubble);
        }
    }
}