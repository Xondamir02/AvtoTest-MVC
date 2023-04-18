using Microsoft.Data.Sqlite;
using Proj1.Models;

namespace Proj1.Repositories
{
    public class UserRepository
    {

        private readonly SqliteConnection _connection;
        private readonly TicketRepository _ticketRepository;
        public UserRepository(TicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
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
                    Id = (string)reader["id"],
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Name = reader.GetString(3),
                    PhotoPath = reader.GetString(4),
                    CurrentTicketIndex = reader.GetInt32(5)
                };
                user.CurrentTicket = user.CurrentTicketIndex == null ? null : _ticketRepository.GetTicket(user.CurrentTicketIndex.Value);
                user.Tickets = _ticketRepository.GetTicketList(user.Id);
                reader.Close();
                return user;
            }

            reader.Close();

            return null;
        }

        public void UpdateUser(User user)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE users SET  username = @username, password = @password, name = @name, " +
                "photo_url = @photo_url, current_ticket_index = @current_ticket_index WHERE id = @id";
            command.Parameters.AddWithValue("id", user.Id);
            command.Parameters.AddWithValue("username", user.Username);
            command.Parameters.AddWithValue("password", user.Password);
            command.Parameters.AddWithValue("name", user.Name);
            command.Parameters.AddWithValue("photo_url", user.PhotoPath);
            command.Parameters.AddWithValue("current_ticket_index", user.CurrentTicketIndex);
            command.Prepare();
            command.ExecuteNonQuery();
        }


        public void UpdateName(ChangeUserModel changeUserModel,User user)
        {
            user.Name = changeUserModel.Name;
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE users SET name = @name WHERE id=@id";
            command.Parameters.AddWithValue("name", user.Name);
            command.Parameters.AddWithValue("id", user.Id);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateUsername(ChangeUserModel changeUserModel, User user)
        {
            user.Username = changeUserModel.Username;
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE users SET username = @username WHERE id=@id";
            command.Parameters.AddWithValue("username", user.Username);
            command.Parameters.AddWithValue("id", user.Id);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateUserPhoto(string photoPath, User user)
        {
            user.PhotoPath = photoPath;
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE users SET photo_url = @photo_url WHERE id=@id";
            command.Parameters.AddWithValue("photo_url", user.PhotoPath);
            command.Parameters.AddWithValue("id", user.Id);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateUserPassword(ChangeUserModel changeUserModel, User user)
        {
            user.Password = changeUserModel.Password;
            var command = _connection.CreateCommand();
            command.CommandText = "UPDATE users SET password =@password WHERE id=@id";
            command.Parameters.AddWithValue("password", user.Password);
            command.Parameters.AddWithValue("id", user.Id);
            command.Prepare();
            command.ExecuteNonQuery();
        }





    }


}
