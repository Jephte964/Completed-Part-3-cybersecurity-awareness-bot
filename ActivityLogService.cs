
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CybersecurityAwarenessBot.GUI.Data;

namespace CybersecurityAwarenessBot.GUI.Services
{
    public class ActivityLogService
    {
        private readonly DatabaseConnection database =
            new DatabaseConnection();

        // SAVE AN ACTIVITY
        public void AddActivity(string action)
        {
            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query =
                    "INSERT INTO ActivityLogs (Action, ActivityTime) " +
                    "VALUES (@Action, @ActivityTime)";

                using (MySqlCommand command =
                    new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue(
                        "@ActivityTime", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        // GET THE 10 MOST RECENT ACTIVITIES
        public List<string> GetRecentActivities()
        {
            List<string> activities = new List<string>();

            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query =
                    "SELECT Action, ActivityTime FROM ActivityLogs " +
                    "ORDER BY ActivityTime DESC, Id DESC LIMIT 10";

                using (MySqlCommand command =
                    new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string action = reader.GetString("Action");

                        DateTime time =
                            reader.GetDateTime("ActivityTime");

                        activities.Add(
                            $"{time:dd MMM yyyy, HH:mm} - {action}");
                    }
                }
            }

            return activities;
        }
    }
}
