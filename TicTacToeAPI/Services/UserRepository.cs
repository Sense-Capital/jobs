using System.Data.SQLite;
using TicTacToeAPI.Model;
using TicTacToeAPI.Services.Interfaces;

namespace TicTacToeAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private const string connectionString = "Data Source = tictactoe.db; Version = 3; Pooling = true; Max Pool Size = 100;";
        public int Create(User user)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command= new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO Users(Email,Hash,Token) VALUES(@Email,@Hash,@Token)";
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Hash", user.Hash);
            command.Parameters.AddWithValue("@Token", user.Token);
            command.Prepare();
            return command.ExecuteNonQuery();
        }

        public int Delete(string email)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command= new SQLiteCommand(connection);
            command.CommandText= "DELETE FROM Users WHERE Email=@Email";
            command.Parameters.AddWithValue("@Email", email);
            command.Prepare();
            return command.ExecuteNonQuery();
        }

        public IList<User> GetAll()
        {
            IList<User> list = new List<User>();
            SQLiteConnection connection= new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command= new SQLiteCommand(connection);
            command.CommandText= "SELECT * FROM Users";
            command.Prepare();
            SQLiteDataReader reader= command.ExecuteReader();

            while(reader.Read()) 
            {
                User user = new User();
                user.Email = reader.GetString(0);
                user.Hash = reader.GetString(1);
                user.Token = reader.GetString(2);
                list.Add(user);
            }
            connection.Close();
            return list;
        }

        public User GetById(string email)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM Users WHERE Email=@Email";
            command.Parameters.AddWithValue("@Email", email);
            command.Prepare();
            SQLiteDataReader reader= command.ExecuteReader();
            if (reader.Read())
            {
                User user = new User();
                user.Email = reader.GetString(0);
                user.Hash = reader.GetString(1);
                user.Token = reader.GetString(2);
                connection.Close();
                return user;
            }
            connection.Close();
            return null;
        }

        public int Update(string previousEmail, User user)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = @"UPDATE Users SET Email=@Email,
                                                     Hash=@Hash,
                                                     Token=@Token
                                    WHERE Email=@LastEmail";
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Hash", user.Hash);
            command.Parameters.AddWithValue("@Token", user.Token);
            command.Parameters.AddWithValue("@LastEmail", previousEmail);
            command.Prepare();
            return command.ExecuteNonQuery();
        }
    }
}