
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CybersecurityAwarenessBot.GUI.Data;
using CybersecurityAwarenessBot.GUI.Models;

namespace CybersecurityAwarenessBot.GUI.Services
{
    public class TaskService
    {
        private readonly DatabaseConnection database =
            new DatabaseConnection();

        // ADD A TASK
        public void AddTask(CybersecurityTask task)
        {
            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query = @"INSERT INTO Tasks
                    (Title, Description, ReminderDate, IsCompleted)
                    VALUES (@Title, @Description, @ReminderDate, @IsCompleted)";

                using (MySqlCommand command =
                    new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description",
                        task.Description ?? "");

                    command.Parameters.AddWithValue("@ReminderDate",
                        task.ReminderDate.HasValue
                            ? (object)task.ReminderDate.Value
                            : DBNull.Value);

                    command.Parameters.AddWithValue("@IsCompleted",
                        task.IsCompleted);

                    command.ExecuteNonQuery();
                }
            }
        }

        // LOAD ALL TASKS
        public List<CybersecurityTask> GetAllTasks()
        {
            List<CybersecurityTask> tasks =
                new List<CybersecurityTask>();

            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Tasks ORDER BY Id DESC";

                using (MySqlCommand command =
                    new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int reminderIndex =
                            reader.GetOrdinal("ReminderDate");

                        tasks.Add(new CybersecurityTask
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title"),

                            Description = reader.IsDBNull(
                                reader.GetOrdinal("Description"))
                                ? ""
                                : reader.GetString("Description"),

                            ReminderDate = reader.IsDBNull(reminderIndex)
                                ? (DateTime?)null
                                : reader.GetDateTime(reminderIndex),

                            IsCompleted = reader.GetBoolean("IsCompleted")
                        });
                    }
                }
            }

            return tasks;
        }

        // MARK A TASK AS COMPLETED
        public void MarkTaskCompleted(int taskId)
        {
            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query =
                    "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id";

                using (MySqlCommand command =
                    new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // UPDATE TASK COMPLETION STATUS
        public void SetTaskCompletion(int taskId, bool isCompleted)
        {
            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query =
                    "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE Id = @Id";

                using (MySqlCommand command =
                    new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);
                    command.Parameters.AddWithValue("@IsCompleted",
                        isCompleted);

                    command.ExecuteNonQuery();
                }
            }
        }

        // DELETE A TASK
        public void DeleteTask(int taskId)
        {
            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query =
                    "DELETE FROM Tasks WHERE Id = @Id";

                using (MySqlCommand command =
                    new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
