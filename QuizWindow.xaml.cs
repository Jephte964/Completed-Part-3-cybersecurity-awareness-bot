
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CybersecurityAwarenessBot.GUI.Services;

namespace CybersecurityAwarenessBot.GUI
{
    public partial class QuizWindow : Window
    {
        private readonly ActivityLogService activityLogService =
            new ActivityLogService();

        private class QuizQuestion
        {
            public string Question { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public int CorrectAnswer { get; set; }
            public string Explanation { get; set; }
            public bool IsTrueFalse { get; set; }
        }

        private readonly List<QuizQuestion> questions =
            new List<QuizQuestion>
        {
            new QuizQuestion
            {
                Question = "1. What is phishing?",
                A = "A type of computer screen",
                B = "A fraudulent attempt to steal information",
                C = "A safe way to store passwords",
                CorrectAnswer = 2,
                Explanation = "Phishing tricks people into revealing sensitive information."
            },
            new QuizQuestion
            {
                Question = "2. Which password is strongest?",
                A = "123456",
                B = "password",
                C = "T!ger_River92#Cloud",
                CorrectAnswer = 3,
                Explanation = "A long, unique password is harder to guess."
            },
            new QuizQuestion
            {
                Question = "3. What should you do with a suspicious link?",
                A = "Click it immediately",
                B = "Verify the sender before taking action",
                C = "Send it to all your friends",
                CorrectAnswer = 2,
                Explanation = "Verify unexpected messages through a trusted channel."
            },
            new QuizQuestion
            {
                Question = "4. What does two-factor authentication do?",
                A = "Adds another verification step",
                B = "Deletes your account",
                C = "Makes every website public",
                CorrectAnswer = 1,
                Explanation = "It adds another layer of protection to your account."
            },
            new QuizQuestion
            {
                Question = "5. How should you use public Wi-Fi?",
                A = "Perform sensitive transactions without checking",
                B = "Share your password with other users",
                C = "Use caution and avoid sensitive activity on untrusted networks",
                CorrectAnswer = 3,
                Explanation = "Use trusted connections and secure websites."
            },
            new QuizQuestion
            {
                Question = "6. True or False: You should share your password with friends.",
                A = "True",
                B = "False",
                C = "",
                CorrectAnswer = 2,
                IsTrueFalse = true,
                Explanation = "Passwords should remain private and should never be shared."
            },
            new QuizQuestion
            {
                Question = "7. What is malware?",
                A = "A type of antivirus",
                B = "A secure password",
                C = "Malicious software designed to harm devices or steal data",
                CorrectAnswer = 3,
                Explanation = "Malware includes viruses, ransomware and spyware."
            },
            new QuizQuestion
            {
                Question = "8. True or False: Software updates can fix security vulnerabilities.",
                A = "True",
                B = "False",
                C = "",
                CorrectAnswer = 1,
                IsTrueFalse = true,
                Explanation = "Updates often include important security patches."
            },
            new QuizQuestion
            {
                Question = "9. What should you do if you receive an unexpected attachment?",
                A = "Open it immediately",
                B = "Check the sender and verify it is safe",
                C = "Forward it to everyone",
                CorrectAnswer = 2,
                Explanation = "Unexpected attachments may contain malware."
            },
            new QuizQuestion
            {
                Question = "10. True or False: Backing up important files helps protect against data loss.",
                A = "True",
                B = "False",
                C = "",
                CorrectAnswer = 1,
                IsTrueFalse = true,
                Explanation = "Backups help recover files after device failure or ransomware attacks."
            }
        };

        private int currentQuestion = 0;
        private int score = 0;
        private bool answerSubmitted = false;
        private bool resultLogged = false;

        public QuizWindow()
        {
            InitializeComponent();

            // RECORD QUIZ START
            LogActivity("Started the cybersecurity quiz.");

            DisplayQuestion();
        }

        // SAVE ACTIVITY WITHOUT INTERRUPTING THE QUIZ
        private void LogActivity(string action)
        {
            try
            {
                activityLogService.AddActivity(action);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Activity log error: " + ex.Message);
            }
        }

        // DISPLAY QUESTION
        private void DisplayQuestion()
        {
            if (currentQuestion >= questions.Count)
            {
                ShowResults();
                return;
            }

            QuizQuestion q = questions[currentQuestion];

            ProgressText.Text =
                $"Question {currentQuestion + 1} of {questions.Count}";

            QuestionText.Text = q.Question;

            OptionA.Content = q.A;
            OptionB.Content = q.B;
            OptionC.Content = q.C;

            OptionC.Visibility = q.IsTrueFalse
                ? Visibility.Collapsed
                : Visibility.Visible;

            OptionA.IsChecked = false;
            OptionB.IsChecked = false;
            OptionC.IsChecked = false;

            OptionA.IsEnabled = true;
            OptionB.IsEnabled = true;
            OptionC.IsEnabled = true;

            FeedbackText.Text = "";
            ScoreText.Text = $"Score: {score}";

            NextButton.Content = "Submit Answer";

            answerSubmitted = false;
        }

        // SUBMIT ANSWER OR MOVE TO NEXT QUESTION
        private void NextButton_Click(
            object sender, RoutedEventArgs e)
        {
            if (currentQuestion >= questions.Count)
            {
                Close();
                return;
            }

            if (!answerSubmitted)
            {
                int selectedAnswer = 0;

                if (OptionA.IsChecked == true)
                    selectedAnswer = 1;
                else if (OptionB.IsChecked == true)
                    selectedAnswer = 2;
                else if (OptionC.IsChecked == true)
                    selectedAnswer = 3;

                if (selectedAnswer == 0)
                {
                    FeedbackText.Foreground = Brushes.Orange;
                    FeedbackText.Text =
                        "Please select an answer first.";
                    return;
                }

                QuizQuestion q = questions[currentQuestion];

                if (selectedAnswer == q.CorrectAnswer)
                {
                    score++;

                    FeedbackText.Foreground = Brushes.LightGreen;
                    FeedbackText.Text =
                        "Correct! " + q.Explanation;
                }
                else
                {
                    FeedbackText.Foreground = Brushes.Salmon;
                    FeedbackText.Text =
                        "Incorrect. " + q.Explanation;
                }

                ScoreText.Text = $"Score: {score}";

                OptionA.IsEnabled = false;
                OptionB.IsEnabled = false;
                OptionC.IsEnabled = false;

                NextButton.Content =
                    currentQuestion == questions.Count - 1
                    ? "View Results"
                    : "Next Question";

                answerSubmitted = true;
            }
            else
            {
                currentQuestion++;
                DisplayQuestion();
            }
        }

        // DISPLAY FINAL RESULTS
        private void ShowResults()
        {
            QuestionText.Text = "Quiz Completed!";

            OptionA.Visibility = Visibility.Collapsed;
            OptionB.Visibility = Visibility.Collapsed;
            OptionC.Visibility = Visibility.Collapsed;

            ProgressText.Text = "Your final result";

            ScoreText.Text =
                $"Final Score: {score} / {questions.Count}";

            double percentage =
                (double)score / questions.Count * 100;

            FeedbackText.Foreground = Brushes.LightGreen;

            FeedbackText.Text =
                percentage >= 80
                ? "Excellent work! You have a strong understanding of cybersecurity."
                : percentage >= 60
                ? "Good effort! Review the explanations to improve your knowledge."
                : "Keep learning! Review the cybersecurity topics and try again.";

            NextButton.Content = "Close Quiz";

            // RECORD FINAL RESULT ONLY ONCE
            if (!resultLogged)
            {
                LogActivity(
                    $"Completed cybersecurity quiz: {score}/{questions.Count} ({percentage:F0}%).");

                resultLogged = true;
            }

            currentQuestion = questions.Count;
        }
    }
}
