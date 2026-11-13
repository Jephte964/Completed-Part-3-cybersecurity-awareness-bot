

using MySql.Data.MySqlClient;

namespace CybersecurityAwarenessBot.GUI.Data
{
    public class DatabaseConnection
    {
        private readonly string connectionString =
            "Server=localhost;Port=3306;Database=CybersecurityDB;Uid=root;Pwd=Jft@0000;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}

