using Microsoft.Data.Sqlite;
using Proj1.Models;

namespace Proj1.Repositories
{
    public class UserRepository
    {

        private readonly SqliteConnection _connection;

        public UserRepository()
        {
            _connection = new SqliteConnection("Data Source=AvtoTest.db");

            _connection.Open();

            CreateUserTable();
        }

        private void CreateUserTable()
        {
            var command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS users(" +
                "id TEXT UNIQUE," +
                "username TEXT NOT NULL, " +
                "password TEXT," +
                "name TEXT," +
                "photo_url TEXT," +
                "current_ticket_index INTEGER)";

            command.ExecuteNonQuery();
        }
        public void AddUser(User user)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO users(id,username,password,name,photo_url, current_ticket_index) VALUES(@id,@username,@password,@name,@photo_url,@current_ticket_index)";
            command.Parameters.AddWithValue("id", user.Id);
            command.Parameters.AddWithValue("username", user.Username);
            command.Parameters.AddWithValue("password", user.Password);
            command.Parameters.AddWithValue("name", user.Name);
            command.Parameters.AddWithValue("photo_url", user.PhotoPath);
            command.Parameters.AddWithValue("current_ticket_index", user.CurrentTicketIndex);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public User? GetUSerById(string id)
        {
            return GetUser("id", id);
        }

        public User? GetUserByUsername(string username)
        {
            return GetUser("username", username);
        }

        public User? GetUser(string paramName, string paramValue)
        {
            var command = _connection.CreateCommand();
            command.CommandText = $"SELECT * FROM users WHERE {paramName} = @p";
            command.Parameters.AddWithValue("p", paramValue);
            command.Prepare();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var user = new User()
                {
                    Id = reader.GetString(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Name = reader.GetString(3),
                    PhotoPath = reader.GetString(4),
                    CurrentTicketIndex = reader.GetInt32(5)
                };

                reader.Close();
                return user;
            }

            reader.Close();

            return null;
        }

        public void UpdateUser(User user)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE users SET  username = @username, password = @password, name = @name, photo_url = @photo_url, current_ticket_index = @current_ticket_index WHERE id = @id";
            command.Parameters.AddWithValue("id", user.Id);
            command.Parameters.AddWithValue("username", user.Username);
            command.Parameters.AddWithValue("password", user.Password);
            command.Parameters.AddWithValue("name", user.Name);
            command.Parameters.AddWithValue("photo_url", user.PhotoPath);
            command.Parameters.AddWithValue("current_ticket_index", user.CurrentTicketIndex);
            command.Prepare();
            command.ExecuteNonQuery();
        }

    }


}
