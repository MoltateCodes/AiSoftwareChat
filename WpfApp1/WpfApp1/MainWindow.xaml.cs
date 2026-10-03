using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // =========================================================
        // CHARACTERS
        // =========================================================

        private readonly HttpClient httpClient = new HttpClient();

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


        // =========================================================
        // WINDOW
        // =========================================================

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


        // =========================================================
        // WINDOWS TITLE BAR
        // =========================================================

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            int dwAttribute,
            ref int pvAttribute,
            int cbAttribute
        );

        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWA_TEXT_COLOR = 36;


        private static int ColorToBgr(byte r, byte g, byte b)
        {
            return (b << 16) | (g << 8) | r;
        }


        private void MainWindow_SourceInitialized(
            object? sender,
            EventArgs e)
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
                sizeof(int)
            );


            DwmSetWindowAttribute(
                hwnd,
                DWMWA_TEXT_COLOR,
                ref textColor,
                sizeof(int)
            );
        }


        // =========================================================
        // CHARACTER SELECTION
        // =========================================================

        private void Character_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Border characterBorder)
                return;


            // =====================================================
            // IMPORTANT:
            // Hide typing indicator immediately when switching
            // to another character.
            // =====================================================

            TypingIndicator.Visibility = Visibility.Collapsed;
            TypingIndicator.Opacity = 0;

            TypingIndicatorTransform.BeginAnimation(
                TranslateTransform.YProperty,
                null
            );


            // =====================================================
            // REMOVE PREVIOUS SELECTION
            // =====================================================

            if (SelectedCharacterBorder != null)
            {
                SelectedCharacterBorder.Background =
                    Brushes.Transparent;

                SelectedCharacterBorder.BorderBrush =
                    Brushes.Transparent;

                SelectedCharacterBorder.BorderThickness =
                    new Thickness(1);
            }


            string? characterName =
                characterBorder.Tag?.ToString();


            if (characterName == null)
                return;


            // =====================================================
            // CHARACTER COLORS
            // =====================================================

            switch (characterName)
            {
                case "MIRA":

                    characterBorder.Background =
                        new LinearGradientBrush
                        {
                            StartPoint = new Point(0, 0),
                            EndPoint = new Point(1, 1),

                            GradientStops =
                            {
                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#D9467A"),
                                    0.0
                                ),

                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#9E315A"),
                                    0.5
                                ),

                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#541B32"),
                                    1.0
                                )
                            }
                        };


                    characterBorder.BorderBrush =
                        new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString(
                                "#8FB7FF")
                        );

                    break;


                case "BYTE":

                    characterBorder.Background =
                        new LinearGradientBrush
                        {
                            StartPoint = new Point(0, 0),
                            EndPoint = new Point(1, 1),

                            GradientStops =
                            {
                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#684B32"),
                                    0.0
                                ),

                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#493421"),
                                    0.5
                                ),

                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#241A12"),
                                    1.0
                                )
                            }
                        };


                    characterBorder.BorderBrush =
                        new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString(
                                "#E5A86B")
                        );

                    break;


                case "NIX":

                    characterBorder.Background =
                        new LinearGradientBrush
                        {
                            StartPoint = new Point(0, 0),
                            EndPoint = new Point(1, 1),

                            GradientStops =
                            {
                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#3B82F6"),
                                    0.0
                                ),

                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#06008D"),
                                    0.5
                                ),

                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#050147"),
                                    1.0
                                )
                            }
                        };


                    characterBorder.BorderBrush =
                        new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString(
                                "#76D6E8")
                        );

                    break;


                case "R-01":

                    characterBorder.Background =
                        new LinearGradientBrush
                        {
                            StartPoint = new Point(0, 0),
                            EndPoint = new Point(1, 1),

                            GradientStops =
                            {
                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#374B55"),
                                    0.0
                                ),

                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#26363E"),
                                    0.5
                                ),

                                new GradientStop(
                                    (Color)ColorConverter.ConvertFromString(
                                        "#11191D"),
                                    1.0
                                )
                            }
                        };


                    characterBorder.BorderBrush =
                        new SolidColorBrush(
                            (Color)ColorConverter.ConvertFromString(
                                "#76D6E8")
                        );

                    break;
            }


            characterBorder.BorderThickness =
                new Thickness(1);

            SelectedCharacterBorder =
                characterBorder;


            // =====================================================
            // FIND CHARACTER
            // =====================================================

            if (!Characters.TryGetValue(
                characterName,
                out Character? character))
            {
                return;
            }


            // =====================================================
            // CHANGE CURRENT CHARACTER
            // =====================================================

            CurrentCharacter = character;


            // =====================================================
            // UPDATE RIGHT CHAT HEADER
            // =====================================================

            ChatCharacterName.Text =
                character.Name;


            ChatCharacterImage.Fill =
                new ImageBrush
                {
                    ImageSource =
                        new BitmapImage(
                            new Uri(
                                $"pack://application:,,,{character.ProfileImage}",
                                UriKind.Absolute
                            )
                        ),

                    Stretch =
                        Stretch.UniformToFill
                };


            // =====================================================
            // UPDATE CENTER CHARACTER NAME
            // =====================================================

            CenterCharacterName.Text =
                character.Name;


            // =====================================================
            // LOAD THIS CHARACTER'S CHAT
            // =====================================================

            LoadChatHistory();
        }


        // =========================================================
        // LOAD CHAT HISTORY
        // =========================================================

        private void LoadChatHistory()
        {
            // Remove messages currently displayed
            ChatMessages.Children.Clear();


            if (CurrentCharacter == null)
                return;


            // Display this character's saved messages
            foreach (ChatMessage chatMessage
                     in CurrentCharacter.ChatHistory)
            {
                if (chatMessage.Sender == "User")
                {
                    AddUserMessage(
                        chatMessage.Message
                    );
                }

                else if (chatMessage.Sender == "AI")
                {
                    AddAIMessage(
                        chatMessage.Message,
                        CurrentCharacter
                    );
                }

                else if (chatMessage.Sender == "Sticker")
                {
                    AddStickerMessage(
                        chatMessage.Message
                    );
                }
            }


            ChatScrollViewer.ScrollToEnd();
        }


        // =========================================================
        // STICKER BUTTON
        // =========================================================

        private void StickerButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            StickerPopup.IsOpen =
                !StickerPopup.IsOpen;
        }


        // =========================================================
        // STICKER CLICK
        // =========================================================

        private async void Sticker_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Tag is not string stickerPath)
                return;

            if (CurrentCharacter == null)
                return;


            // =====================================================
            // CAPTURE CHARACTER
            // =====================================================

            Character respondingCharacter =
                CurrentCharacter;


            // =====================================================
            // SAVE STICKER
            // =====================================================

            respondingCharacter.ChatHistory.Add(
                new ChatMessage(
                    "Sticker",
                    stickerPath
                )
            );


            // =====================================================
            // SHOW STICKER
            // =====================================================

            AddStickerMessage(
                stickerPath
            );


            // Close popup
            StickerPopup.IsOpen = false;


            // Scroll
            ChatScrollViewer.ScrollToEnd();


            // =====================================================
            // WAIT
            // =====================================================

            await Task.Delay(500);


            // =====================================================
            // SHOW TYPING ONLY IF CHARACTER IS STILL SELECTED
            // =====================================================

            if (CurrentCharacter == respondingCharacter)
            {
                ShowTypingIndicatorFor(
                    respondingCharacter
                );
            }


            // =====================================================
            // TYPING DELAY
            // =====================================================

            await Task.Delay(1200);


            // =====================================================
            // HIDE TYPING ONLY IF CHARACTER IS STILL SELECTED
            // =====================================================

            if (CurrentCharacter == respondingCharacter)
            {
                await HideTypingIndicator();
            }


            // =====================================================
            // AI RESPONSE
            // =====================================================

            string aiResponse =
                "Haha, I see your sticker!";


            // =====================================================
            // SAVE TO CORRECT CHARACTER
            // =====================================================

            respondingCharacter.ChatHistory.Add(
                new ChatMessage(
                    "AI",
                    aiResponse
                )
            );


            // =====================================================
            // DISPLAY ONLY IF STILL SELECTED
            // =====================================================

            if (CurrentCharacter == respondingCharacter)
            {
                AddAIMessage(
                    aiResponse,
                    respondingCharacter
                );

                ChatScrollViewer.ScrollToEnd();
            }
        }


        // =========================================================
        // ENTER KEY
        // =========================================================

        private void MessageInput_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(
                    sender,
                    new RoutedEventArgs()
                );

                e.Handled = true;
            }
        }


        // =========================================================
        // MESSAGE INPUT FOCUS
        // =========================================================

        private void MessageInput_GotFocus(
            object sender,
            RoutedEventArgs e)
        {
            MessagePlaceholder.Visibility =
                Visibility.Collapsed;

            MessageInput.Foreground =
                Brushes.White;
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


        private void MessageInput_LostFocus(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                MessageInput.Text))
            {
                MessagePlaceholder.Visibility =
                    Visibility.Visible;

                MessageInput.Foreground =
                    new SolidColorBrush(
                        Color.FromRgb(
                            107,
                            114,
                            128
                        )
                    );
            }
        }


        // =========================================================
        // SEND MESSAGE
        // =========================================================

        private async void SendButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            // ==========================================
            // GET MESSAGE
            // ==========================================

            string message = MessageInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
                return;

            if (CurrentCharacter == null)
                return;


            // ==========================================
            // CAPTURE CHARACTER
            // ==========================================

            Character respondingCharacter = CurrentCharacter;


            // ==========================================
            // SAVE USER MESSAGE
            // ==========================================

            respondingCharacter.ChatHistory.Add(
                new ChatMessage(
                    "User",
                    message
                )
            );


            // Display user message
            AddUserMessage(message);

            MessageInput.Clear();

            ChatScrollViewer.ScrollToEnd();


            // ==========================================
            // SHOW TYPING
            // ==========================================

            if (CurrentCharacter == respondingCharacter)
            {
                ShowTypingIndicatorFor(
                    respondingCharacter
                );
            }


            try
            {
                // ==========================================
                // SEND MESSAGE TO NODE.JS BACKEND
                // ==========================================

                var requestData = new
                {
                    message = message,
                    character = respondingCharacter.Name,
                    history = respondingCharacter.ChatHistory
                };

                string json =
                    JsonSerializer.Serialize(requestData);

                using StringContent content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );


                HttpResponseMessage response =
                    await httpClient.PostAsync(
                        "http://localhost:5000/api/chat",
                        content
                    );


                // ==========================================
                // READ RESPONSE
                // ==========================================

                string responseText =
                    await response.Content.ReadAsStringAsync();


                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        responseText
                    );
                }


                using JsonDocument jsonDocument =
                    JsonDocument.Parse(responseText);


                string aiResponse =
                    jsonDocument.RootElement
                        .GetProperty("reply")
                        .GetString()
                    ?? "No response received.";


                // ==========================================
                // SAVE AI RESPONSE
                // ==========================================

                respondingCharacter.ChatHistory.Add(
                    new ChatMessage(
                        "AI",
                        aiResponse
                    )
                );


                // ==========================================
                // DISPLAY RESPONSE
                // ==========================================

                if (CurrentCharacter == respondingCharacter)
                {
                    await HideTypingIndicator();

                    AddAIMessage(
                        aiResponse,
                        respondingCharacter
                    );

                    ChatScrollViewer.ScrollToEnd();
                }
            }
            catch (Exception ex)
            {
                // Hide typing indicator
                if (CurrentCharacter == respondingCharacter)
                {
                    await HideTypingIndicator();
                }


                // Show error in chat
                string errorMessage =
                    "Backend connection failed.\n\n" +
                    ex.Message;

                if (CurrentCharacter == respondingCharacter)
                {
                    AddAIMessage(
                        errorMessage,
                        respondingCharacter
                    );

                    ChatScrollViewer.ScrollToEnd();
                }
            }
        }


        // =========================================================
        // SHOW TYPING INDICATOR
        // =========================================================

        private void ShowTypingIndicatorFor(
            Character character)
        {
            // Set correct character name
            TypingCharacterName.Text =
                character.Name;


            // Make visible
            TypingIndicator.Visibility =
                Visibility.Visible;


            // Starting position
            TypingIndicator.Opacity = 0;

            TypingIndicatorTransform.Y = -8;


            // =====================================================
            // SLIDE UP
            // =====================================================

            DoubleAnimation slideAnimation =
                new DoubleAnimation
                {
                    From = -8,
                    To = 0,

                    Duration =
                        TimeSpan.FromMilliseconds(220),

                    EasingFunction =
                        new QuadraticEase
                        {
                            EasingMode =
                                EasingMode.EaseOut
                        }
                };


            // =====================================================
            // FADE IN
            // =====================================================

            DoubleAnimation fadeAnimation =
                new DoubleAnimation
                {
                    From = 0,
                    To = 1,

                    Duration =
                        TimeSpan.FromMilliseconds(220),

                    EasingFunction =
                        new QuadraticEase
                        {
                            EasingMode =
                                EasingMode.EaseOut
                        }
                };


            TypingIndicatorTransform.BeginAnimation(
                TranslateTransform.YProperty,
                slideAnimation
            );


            TypingIndicator.BeginAnimation(
                UIElement.OpacityProperty,
                fadeAnimation
            );


            ChatScrollViewer.ScrollToEnd();
        }


        // =========================================================
        // HIDE TYPING INDICATOR
        // =========================================================

        private async Task HideTypingIndicator()
        {
            // Fade out
            DoubleAnimation fadeAnimation =
                new DoubleAnimation
                {
                    From = 1,
                    To = 0,

                    Duration =
                        TimeSpan.FromMilliseconds(180),

                    EasingFunction =
                        new QuadraticEase
                        {
                            EasingMode =
                                EasingMode.EaseIn
                        }
                };


            // Slide down
            DoubleAnimation slideAnimation =
                new DoubleAnimation
                {
                    From = 0,
                    To = 5,

                    Duration =
                        TimeSpan.FromMilliseconds(180),

                    EasingFunction =
                        new QuadraticEase
                        {
                            EasingMode =
                                EasingMode.EaseIn
                        }
                };


            TypingIndicatorTransform.BeginAnimation(
                TranslateTransform.YProperty,
                slideAnimation
            );


            TypingIndicator.BeginAnimation(
                UIElement.OpacityProperty,
                fadeAnimation
            );


            // Wait for animation
            await Task.Delay(180);


            TypingIndicator.Visibility =
                Visibility.Collapsed;


            // Reset position
            TypingIndicatorTransform.Y = 8;
        }


        // =========================================================
        // MESSAGE ANIMATION
        // =========================================================

        private void AnimateMessage(
            UIElement element)
        {
            // Start slightly below
            element.RenderTransform =
                new TranslateTransform(
                    0,
                    25
                );

            element.Opacity = 0;


            // Slide up
            DoubleAnimation slideAnimation =
                new DoubleAnimation
                {
                    From = 25,
                    To = 0,

                    Duration =
                        TimeSpan.FromMilliseconds(250),

                    EasingFunction =
                        new QuadraticEase
                        {
                            EasingMode =
                                EasingMode.EaseOut
                        }
                };


            // Fade in
            DoubleAnimation fadeAnimation =
                new DoubleAnimation
                {
                    From = 0,
                    To = 1,

                    Duration =
                        TimeSpan.FromMilliseconds(200)
                };


            TranslateTransform transform =
                (TranslateTransform)
                element.RenderTransform;


            transform.BeginAnimation(
                TranslateTransform.YProperty,
                slideAnimation
            );


            element.BeginAnimation(
                UIElement.OpacityProperty,
                fadeAnimation
            );
        }


        // =========================================================
        // USER MESSAGE
        // =========================================================

        private void AddUserMessage(
            string message)
        {
            Border messageBubble =
                new Border
                {
                    Background =
                        new SolidColorBrush(
                            Color.FromRgb(
                                51,
                                68,
                                90
                            )
                        ),

                    CornerRadius =
                        new CornerRadius(12),

                    Padding =
                        new Thickness(
                            14,
                            10,
                            14,
                            10
                        ),

                    Margin =
                        new Thickness(
                            40,
                            0,
                            10,
                            10
                        ),

                    HorizontalAlignment =
                        HorizontalAlignment.Right,

                    MaxWidth = 500,

                    Child =
                        new TextBlock
                        {
                            Text = message,

                            Foreground =
                                Brushes.White,

                            FontSize = 14,

                            TextWrapping =
                                TextWrapping.Wrap
                        }
                };


            ChatMessages.Children.Add(
                messageBubble
            );


            AnimateMessage(
                messageBubble
            );
        }


        // =========================================================
        // STICKER MESSAGE
        // =========================================================

        private void AddStickerMessage(
            string stickerPath)
        {
            Border stickerContainer =
                new Border
                {
                    Background =
                        Brushes.Transparent,

                    Margin =
                        new Thickness(
                            40,
                            5,
                            15,
                            12
                        ),

                    HorizontalAlignment =
                        HorizontalAlignment.Right
                };


            BitmapImage bitmap =
                new BitmapImage();


            bitmap.BeginInit();


            // Convert:
            // /Stickers/byte_fine.png
            //
            // to:
            // /WpfApp1;component/Stickers/byte_fine.png

            string cleanPath =
                stickerPath.TrimStart('/');


            bitmap.UriSource =
                new Uri(
                    $"pack://application:,,,/WpfApp1;component/{cleanPath}",
                    UriKind.Absolute
                );


            bitmap.CacheOption =
                BitmapCacheOption.OnLoad;


            bitmap.EndInit();


            Image stickerImage =
                new Image
                {
                    Source = bitmap,

                    Width = 180,
                    Height = 180,

                    Stretch =
                        Stretch.Uniform,

                    SnapsToDevicePixels =
                        true,

                    UseLayoutRounding =
                        true
                };


            RenderOptions.SetBitmapScalingMode(
                stickerImage,
                BitmapScalingMode.HighQuality
            );


            stickerContainer.Child =
                stickerImage;


            ChatMessages.Children.Add(
                stickerContainer
            );


            AnimateMessage(
                stickerContainer
            );
        }


        // =========================================================
        // AI MESSAGE
        // =========================================================
        //
        // IMPORTANT:
        // We now pass the CHARACTER explicitly.
        //
        // This prevents the bubble color from changing if the
        // user switches characters while an AI response is running.
        // =========================================================

        private void AddAIMessage(
            string message,
            Character character)
        {
            Color bubbleColor;


            switch (character.Name)
            {
                case "MIRA":

                    bubbleColor =
                        Color.FromRgb(
                            255,
                            18,
                            125
                        );

                    break;


                case "BYTE":

                    bubbleColor =
                        Color.FromRgb(
                            73,
                            52,
                            33
                        );

                    break;


                case "NIX":

                    bubbleColor =
                        Color.FromRgb(
                            35,
                            45,
                            85
                        );

                    break;


                case "R-01":

                    bubbleColor =
                        Color.FromRgb(
                            45,
                            58,
                            65
                        );

                    break;


                default:

                    bubbleColor =
                        Color.FromRgb(
                            37,
                            43,
                            54
                        );

                    break;
            }


            Border messageBubble =
                new Border
                {
                    Background =
                        new SolidColorBrush(
                            bubbleColor
                        ),

                    CornerRadius =
                        new CornerRadius(12),

                    Padding =
                        new Thickness(
                            14,
                            10,
                            14,
                            10
                        ),

                    Margin =
                        new Thickness(
                            0,
                            0,
                            40,
                            10
                        ),

                    HorizontalAlignment =
                        HorizontalAlignment.Left,

                    MaxWidth = 500,

                    Child =
                        new TextBlock
                        {
                            Text = message,

                            Foreground =
                                Brushes.White,

                            FontSize = 14,

                            TextWrapping =
                                TextWrapping.Wrap
                        }
                };


            ChatMessages.Children.Add(
                messageBubble
            );


            AnimateMessage(
                messageBubble
            );
        }
    }
}