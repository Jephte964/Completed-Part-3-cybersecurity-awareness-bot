
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CybersecurityAwarenessBot.Classes;

namespace CybersecurityAwarenessBot.GUI
{
    public partial class MainWindow : Window
    {
        private readonly Random random = new Random();

        // Audio player for the greeting sound.
        private readonly AudioPlayer audioPlayer = new AudioPlayer();

        private readonly Dictionary<string, string[]> responses =
            new Dictionary<string, string[]>
            {
                ["password"] = new string[]
                {
                    "Use a strong, unique password for every account. Avoid personal details.",
                    "A password manager can help you create and store strong passwords.",
                    "Enable multi-factor authentication to add another layer of protection.",
                    "Never share your password with other people. Change it if you think it has been exposed."
                },

                ["scam"] = new string[]
                {
                    "Be careful with unexpected messages asking for money or personal information.",
                    "Verify suspicious requests using the organisation's official contact details.",
                    "Never click links or open attachments in messages you do not trust.",
                    "Be cautious of offers that promise easy money or pressure you to act immediately."
                },

                ["privacy"] = new string[]
                {
                    "Review the privacy settings on your social media accounts.",
                    "Share only the personal information that is necessary.",
                    "Check app permissions and disable access that an app does not need.",
                    "Avoid posting sensitive information such as your address or identity details publicly."
                },

                ["phishing"] = new string[]
                {
                    "Check the sender's address carefully before trusting an email.",
                    "Avoid clicking unexpected links. Visit the official website directly instead.",
                    "Be cautious of messages that create urgency or request passwords.",
                    "Look for unusual links, unexpected attachments, spelling mistakes and requests for confidential information."
                },

                ["malware"] = new string[]
                {
                    "Malware is software designed to harm devices or steal information. Keep your software updated.",
                    "Avoid downloading files from unknown websites and scan suspicious attachments.",
                    "Use reputable security software and back up important files regularly.",
                    "Install apps from trusted sources and avoid pirated software."
                },

                ["wifi"] = new string[]
                {
                    "Avoid accessing sensitive accounts on public Wi-Fi unless you have a secure connection.",
                    "Verify the network name with the venue before connecting to public Wi-Fi.",
                    "Keep your device updated and turn off automatic connection to unknown networks.",
                    "For your home Wi-Fi, use WPA2 or WPA3 security and a strong router password."
                },

                ["authentication"] = new string[]
                {
                    "Two-factor authentication adds another verification step when you sign in.",
                    "Use an authenticator app or security key where possible, and never share verification codes.",
                    "Enable multi-factor authentication on your email and other important accounts.",
                    "Keep backup codes in a safe place in case you lose access to your authentication device."
                }
            };

        private string userName = "";
        private string lastTopic = "";
        private string rememberedFact = "";

        public MainWindow()
        {
            InitializeComponent();

            UpdateCharacterCounter();

            // Play the greeting audio when the main window opens.
            audioPlayer.PlayGreeting();

            AddMessage(
                "BOT",
                "Welcome to Cybersecurity Awareness Bot! What is your name?",
                Brushes.MediumSpringGreen);
        }

        // Adds a message to the chat using a chat-bubble design.
        private void AddMessage(string sender, string message, Brush colour)
        {
            bool isUser = sender == "YOU";

            Border bubble = new Border
            {
                Background = isUser
                    ? new SolidColorBrush(Color.FromRgb(31, 78, 121))
                    : new SolidColorBrush(Color.FromRgb(27, 41, 64)),

                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(14, 10, 14, 10),
                Margin = new Thickness(0, 6, 0, 6),
                MaxWidth = 600,
                HorizontalAlignment = isUser
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left
            };

            StackPanel messageContent = new StackPanel();

            TextBlock senderText = new TextBlock
            {
                Text = isUser ? "YOU" : "CYBERSECURITY BOT",
                Foreground = isUser
                    ? Brushes.LightSkyBlue
                    : Brushes.MediumSpringGreen,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            };

            TextBlock messageText = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 550
            };

            messageContent.Children.Add(senderText);
            messageContent.Children.Add(messageText);

            bubble.Child = messageContent;
            ChatPanel.Children.Add(bubble);

            ChatScrollViewer.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessMessage();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessMessage();
                e.Handled = true;
            }
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCharacterCounter();
        }

        private void UpdateCharacterCounter()
        {
            if (InputBox == null || CharacterCounter == null)
                return;

            int length = InputBox.Text.Length;

            CharacterCounter.Text = $"{length} / 500 characters";

            CharacterCounter.Foreground = length >= 450
                ? Brushes.Orange
                : new SolidColorBrush(Color.FromRgb(129, 146, 170));
        }

        private void ProcessMessage()
        {
            try
            {
                string input = InputBox.Text?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    AddMessage(
                        "BOT",
                        "Please type a message before pressing Send.",
                        Brushes.Orange);

                    InputBox.Focus();
                    return;
                }

                AddMessage("YOU", input, Brushes.LightSkyBlue);

                InputBox.Clear();
                InputBox.Focus();

                if (string.IsNullOrWhiteSpace(userName))
                {
                    if (input.Length > 50)
                    {
                        AddMessage(
                            "BOT",
                            "Please enter a shorter name (50 characters or fewer).",
                            Brushes.Orange);
                        return;
                    }

                    userName = input;

                    AddMessage(
                        "BOT",
                        $"Nice to meet you, {userName}! Choose a topic above or type 'help' to see what I can do.",
                        Brushes.MediumSpringGreen);

                    return;
                }

                string message = input.ToLowerInvariant();

                if (message == "hi" || message == "hello" ||
                    message == "hey" || message == "good morning" ||
                    message == "good afternoon" || message == "good evening")
                {
                    AddMessage(
                        "BOT",
                        $"Hello, {userName}! How can I help you stay safe online today?",
                        Brushes.MediumSpringGreen);
                    return;
                }

                if (message == "help" || message == "menu" ||
                    message == "topics" || message == "what topics")
                {
                    AddMessage(
                        "BOT",
                        "Here are the cybersecurity topics I can help you with:\n\n" +
                        "- Password safety\n" +
                        "- Phishing emails\n" +
                        "- Online scams and fraud\n" +
                        "- Privacy protection\n" +
                        "- Malware and viruses\n" +
                        "- Wi-Fi security\n" +
                        "- Two-factor authentication\n\n" +
                        "You can also ask me to open your task list, start the quiz or view your activity log.",
                        Brushes.MediumSpringGreen);
                    return;
                }

                if (message == "bye" || message == "goodbye" ||
                    message == "exit" || message == "quit")
                {
                    AddMessage(
                        "BOT",
                        $"Goodbye, {userName}! Remember to protect your passwords and personal information. Stay safe online!",
                        Brushes.MediumSpringGreen);
                    return;
                }

                // REMEMBER USER INFORMATION
                if (message.StartsWith("remember that "))
                {
                    string fact = input.Substring("remember that ".Length).Trim();

                    if (string.IsNullOrWhiteSpace(fact))
                    {
                        AddMessage(
                            "BOT",
                            "Please tell me what you would like me to remember.",
                            Brushes.Orange);
                    }
                    else
                    {
                        rememberedFact = fact;

                        AddMessage(
                            "BOT",
                            "I'll remember this during our current session: " + rememberedFact,
                            Brushes.MediumSpringGreen);
                    }

                    return;
                }

                if (message.Contains("what do you remember") ||
                    message.Contains("what did i ask you to remember"))
                {
                    AddMessage(
                        "BOT",
                        string.IsNullOrWhiteSpace(rememberedFact)
                            ? "You haven't asked me to remember anything in this session yet. Type 'remember that' followed by the information."
                            : "You asked me to remember: " + rememberedFact,
                        Brushes.MediumSpringGreen);

                    return;
                }

                // ASK FOR ANOTHER CYBERSECURITY TIP
                if (message.Contains("another tip") ||
                    message.Contains("tell me more") ||
                    message.Contains("explain more") ||
                    message == "more")
                {
                    if (!string.IsNullOrWhiteSpace(lastTopic) &&
                        responses.ContainsKey(lastTopic))
                    {
                        RespondWithTopic(lastTopic);
                    }
                    else
                    {
                        AddMessage(
                            "BOT",
                            "Choose a cybersecurity topic first, then ask me for another tip.",
                            Brushes.Orange);
                    }

                    return;
                }

                // NLP: OPEN TASK ASSISTANT
                if (message.Contains("open task") ||
                    message.Contains("show my tasks") ||
                    message.Contains("task list") ||
                    message.Contains("add a task") ||
                    message.Contains("remind me to"))
                {
                    AddMessage(
                        "BOT",
                        "Opening your Cybersecurity Task Assistant. You can add your task and set a reminder there.",
                        Brushes.MediumSpringGreen);

                    TaskListWindow taskWindow = new TaskListWindow();
                    taskWindow.Owner = this;
                    taskWindow.ShowDialog();
                    return;
                }

                // NLP: OPEN CYBERSECURITY QUIZ
                if (message.Contains("open quiz") ||
                    message.Contains("start quiz") ||
                    message.Contains("play quiz") ||
                    message.Contains("test my knowledge"))
                {
                    AddMessage(
                        "BOT",
                        "Let's test your cybersecurity knowledge!",
                        Brushes.MediumSpringGreen);

                    QuizWindow quiz = new QuizWindow();
                    quiz.Owner = this;
                    quiz.ShowDialog();
                    return;
                }

                // NLP: OPEN ACTIVITY LOG
                if (message.Contains("activity log") ||
                    message.Contains("what have you done for me") ||
                    message.Contains("show my activities") ||
                    message.Contains("recent activities"))
                {
                    AddMessage(
                        "BOT",
                        "Here is your recent activity history.",
                        Brushes.MediumSpringGreen);

                    ActivityLogWindow activityWindow = new ActivityLogWindow();
                    activityWindow.Owner = this;
                    activityWindow.ShowDialog();
                    return;
                }

                // SENTIMENT DETECTION
                if (RespondToSentiment(message))
                    return;

                // CYBERSECURITY TOPIC RECOGNITION
                if (message.Contains("password") ||
                    message.Contains("passcode") ||
                    message.Contains("passphrase"))
                {
                    RespondWithTopic("password");
                }
                else if (message.Contains("phishing") ||
                         message.Contains("suspicious email") ||
                         message.Contains("fake email") ||
                         message.Contains("fake message"))
                {
                    RespondWithTopic("phishing");
                }
                else if (message.Contains("scam") ||
                         message.Contains("fraud") ||
                         message.Contains("scammer"))
                {
                    RespondWithTopic("scam");
                }
                else if (message.Contains("privacy") ||
                         message.Contains("personal information") ||
                         message.Contains("data protection"))
                {
                    RespondWithTopic("privacy");
                }
                else if (message.Contains("malware") ||
                         message.Contains("virus") ||
                         message.Contains("ransomware") ||
                         message.Contains("spyware"))
                {
                    RespondWithTopic("malware");
                }
                else if (message.Contains("wi-fi") ||
                         message.Contains("wifi") ||
                         message.Contains("wireless network") ||
                         message.Contains("public network"))
                {
                    RespondWithTopic("wifi");
                }
                else if (message.Contains("two-factor") ||
                         message.Contains("two factor") ||
                         message.Contains("multi-factor") ||
                         message.Contains("multi factor") ||
                         message.Contains("multifactor") ||
                         message.Contains("authentication") ||
                         message.Contains("verification code"))
                {
                    RespondWithTopic("authentication");
                }
                else if (message.Contains("how are you"))
                {
                    AddMessage(
                        "BOT",
                        $"I'm ready to help you stay safe online, {userName}!",
                        Brushes.MediumSpringGreen);
                }
                else if (message.Contains("your purpose") ||
                         message.Contains("what is your purpose") ||
                         message.Contains("purpose"))
                {
                    AddMessage(
                        "BOT",
                        "My purpose is to help you understand cybersecurity risks and learn practical ways to protect yourself online.",
                        Brushes.MediumSpringGreen);
                }
                else if (message.Contains("what can i ask") ||
                         message.Contains("what can you do") ||
                         message.Contains("how do you work"))
                {
                    AddMessage(
                        "BOT",
                        "I provide cybersecurity awareness tips, recognise common topics, respond to some feelings, remember one fact during this session, and open your task assistant, quiz and activity log.",
                        Brushes.MediumSpringGreen);
                }
                else
                {
                    AddMessage(
                        "BOT",
                        "I'm not sure I understand that question. Try asking about passwords, phishing, scams, privacy, malware, Wi-Fi security or two-factor authentication. You can also ask me to open your tasks, quiz or activity log. Type 'help' for assistance.",
                        Brushes.Orange);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "An unexpected error occurred while processing your message. Please try again.",
                    "Cybersecurity Awareness Bot",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private bool RespondToSentiment(string message)
        {
            if (message.Contains("worried") ||
                message.Contains("scared") ||
                message.Contains("afraid") ||
                message.Contains("anxious"))
            {
                AddMessage(
                    "BOT",
                    $"It's understandable to feel worried, {userName}. Stay calm, avoid suspicious links, and verify unexpected messages before responding.",
                    Brushes.MediumSpringGreen);
                return true;
            }

            if (message.Contains("confused") ||
                message.Contains("don't understand") ||
                message.Contains("do not understand"))
            {
                AddMessage(
                    "BOT",
                    "No problem! I can explain cybersecurity step by step. Try asking about passwords, phishing, scams or privacy.",
                    Brushes.MediumSpringGreen);
                return true;
            }

            if (message.Contains("frustrated") ||
                message.Contains("upset") ||
                message.Contains("angry"))
            {
                AddMessage(
                    "BOT",
                    "I understand this can be frustrating. Let's work through the cybersecurity problem one step at a time.",
                    Brushes.MediumSpringGreen);
                return true;
            }

            if (message.Contains("happy") ||
                message.Contains("great") ||
                message.Contains("thank you") ||
                message.Contains("thanks"))
            {
                AddMessage(
                    "BOT",
                    $"I'm glad to hear that, {userName}! Keep practising safe online habits.",
                    Brushes.MediumSpringGreen);
                return true;
            }

            return false;
        }

        private void RespondWithTopic(string topic)
        {
            if (responses.TryGetValue(topic, out string[] options) &&
                options.Length > 0)
            {
                lastTopic = topic;
                string answer = options[random.Next(options.Length)];

                AddMessage(
                    "BOT",
                    answer,
                    Brushes.MediumSpringGreen);
            }
            else
            {
                AddMessage(
                    "BOT",
                    "Sorry, I could not find a response for that topic. Type 'help' to see the available topics.",
                    Brushes.Orange);
            }
        }

        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button) ||
                !(button.Tag is string topic))
                return;

            if (string.IsNullOrWhiteSpace(userName))
            {
                AddMessage(
                    "BOT",
                    "Please enter your name in the message box first. Then you can select a topic.",
                    Brushes.Orange);

                InputBox.Focus();
                return;
            }

            AddMessage(
                "YOU",
                button.Content?.ToString() ?? topic,
                Brushes.LightSkyBlue);

            RespondWithTopic(topic);
        }

        private void ClearChatButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to clear the conversation?",
                "Clear Chat",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ChatPanel.Children.Clear();

                userName = "";
                lastTopic = "";
                rememberedFact = "";

                AddMessage(
                    "BOT",
                    "Chat cleared. Welcome back! What is your name?",
                    Brushes.MediumSpringGreen);

                InputBox.Clear();
                InputBox.Focus();
            }
        }

        // Opens the cybersecurity quiz window.
        private void OpenQuizButton_Click(object sender, RoutedEventArgs e)
        {
            QuizWindow quiz = new QuizWindow();
            quiz.Owner = this;
            quiz.ShowDialog();
        }

        // Opens the cybersecurity task list window.
        private void OpenTaskListButton_Click(object sender, RoutedEventArgs e)
        {
            TaskListWindow taskWindow = new TaskListWindow();
            taskWindow.Owner = this;
            taskWindow.ShowDialog();
        }

        // Opens the activity log window.
        private void OpenActivityLogButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityLogWindow activityWindow = new ActivityLogWindow();
            activityWindow.Owner = this;
            activityWindow.ShowDialog();
        }
    }
}
