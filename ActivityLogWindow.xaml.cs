
using System;
using System.Windows;
using CybersecurityAwarenessBot.GUI.Services;

namespace CybersecurityAwarenessBot.GUI
{
    public partial class ActivityLogWindow : Window
    {
        private readonly ActivityLogService activityService =
            new ActivityLogService();

        public ActivityLogWindow()
        {
            InitializeComponent();
            LoadActivities();
        }

        private void LoadActivities()
        {
            try
            {
                ActivityList.ItemsSource =
                    activityService.GetRecentActivities();

                if (ActivityList.Items.Count == 0)
                {
                    ActivityList.Items.Add(
                        "No activities recorded yet.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Could not load activities from the database.",
                    "Activity Log",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void RefreshButton_Click(
            object sender, RoutedEventArgs e)
        {
            LoadActivities();
        }

        private void CloseButton_Click(
            object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
