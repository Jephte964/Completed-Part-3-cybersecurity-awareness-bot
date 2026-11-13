
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Threading;
using CybersecurityAwarenessBot.GUI.Models;
using CybersecurityAwarenessBot.GUI.Services;

namespace CybersecurityAwarenessBot.GUI
{
    public partial class TaskListWindow : Window
    {
        private ObservableCollection<CybersecurityTask> tasks =
            new ObservableCollection<CybersecurityTask>();

        private TaskService taskService = new TaskService();

        // ACTIVITY LOG SERVICE
        private ActivityLogService activityLogService =
            new ActivityLogService();

        // REMINDER TIMER
        private DispatcherTimer reminderTimer =
            new DispatcherTimer();

        private HashSet<int> notifiedTasks =
            new HashSet<int>();

        public TaskListWindow()
        {
            InitializeComponent();

            TaskList.ItemsSource = tasks;

            LoadTasksFromDatabase();

            // START REMINDER TIMER
            reminderTimer.Interval = TimeSpan.FromSeconds(30);
            reminderTimer.Tick += ReminderTimer_Tick;
            reminderTimer.Start();
        }

        // RECORD ACTIVITY WITHOUT INTERRUPTING TASK OPERATIONS
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

        // LOAD TASKS FROM MYSQL
        private void LoadTasksFromDatabase()
        {
            try
            {
                tasks.Clear();

                var savedTasks = taskService.GetAllTasks();

                foreach (var task in savedTasks)
                {
                    tasks.Add(task);
                }

                UpdateProgress();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load tasks from the database.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // ADD A NEW TASK
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string description = TaskInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter a task first.");
                return;
            }

            try
            {
                CybersecurityTask newTask = new CybersecurityTask
                {
                    Title = description,
                    Description = description,
                    ReminderDate = ReminderDatePicker.SelectedDate,
                    IsCompleted = false
                };

                taskService.AddTask(newTask);

                // RECORD ACTIVITY
                LogActivity("Added a new task: " + newTask.Title);

                LoadTasksFromDatabase();

                TaskInput.Clear();
                ReminderDatePicker.SelectedDate = null;

                MessageBox.Show("Task saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not save the task.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // MARK OR UNMARK A TASK
        private void TaskCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox == null)
                return;

            CybersecurityTask selectedTask =
                checkBox.DataContext as CybersecurityTask;

            if (selectedTask == null)
                return;

            try
            {
                bool isCompleted = checkBox.IsChecked == true;

                taskService.SetTaskCompletion(
                    selectedTask.Id,
                    isCompleted);

                selectedTask.IsCompleted = isCompleted;

                // RECORD ACTIVITY
                LogActivity(
                    (isCompleted ? "Completed task: " : "Reopened task: ")
                    + selectedTask.Title);

                UpdateProgress();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not update the task.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // DELETE SELECTED TASK
        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            CybersecurityTask selectedTask =
                TaskList.SelectedItem as CybersecurityTask;

            if (selectedTask == null)
            {
                MessageBox.Show("Please select a task to delete.");
                return;
            }

            try
            {
                taskService.DeleteTask(selectedTask.Id);

                // RECORD ACTIVITY BEFORE REMOVING
                LogActivity("Deleted task: " + selectedTask.Title);

                tasks.Remove(selectedTask);

                notifiedTasks.Remove(selectedTask.Id);

                UpdateProgress();

                MessageBox.Show("Task deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not delete the task.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // DELETE ALL COMPLETED TASKS
        private void ClearCompletedButton_Click(object sender, RoutedEventArgs e)
        {
            var completedTasks = tasks
                .Where(task => task.IsCompleted)
                .ToList();

            if (completedTasks.Count == 0)
            {
                MessageBox.Show("There are no completed tasks to clear.");
                return;
            }

            try
            {
                foreach (var task in completedTasks)
                {
                    taskService.DeleteTask(task.Id);

                    // RECORD EACH DELETED TASK
                    LogActivity("Cleared completed task: " + task.Title);

                    notifiedTasks.Remove(task.Id);
                    tasks.Remove(task);
                }

                UpdateProgress();

                MessageBox.Show("Completed tasks cleared successfully!");
            }
            catch (Exception ex)
            {
                LoadTasksFromDatabase();

                MessageBox.Show(
                    "Could not clear all completed tasks.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // CHECK TASK REMINDERS
        private void ReminderTimer_Tick(object sender, EventArgs e)
        {
            foreach (var task in tasks.ToList())
            {
                if (task.ReminderDate.HasValue &&
                    task.ReminderDate.Value <= DateTime.Now &&
                    !task.IsCompleted &&
                    !notifiedTasks.Contains(task.Id))
                {
                    notifiedTasks.Add(task.Id);

                    // RECORD REMINDER ACTIVITY
                    LogActivity("Reminder displayed for task: " + task.Title);

                    MessageBox.Show(
                        "Reminder: " + task.Title,
                        "Cybersecurity Task Reminder",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        // UPDATE TASK PROGRESS
        private void UpdateProgress()
        {
            int totalTasks = tasks.Count;
            int completedTasks = tasks.Count(task => task.IsCompleted);

            ProgressText.Text =
                $"{completedTasks} of {totalTasks} tasks completed";
        }

        // CLOSE WINDOW
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            reminderTimer.Stop();
            Close();
        }
    }
}
