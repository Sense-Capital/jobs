using System.Data.SQLite;
using TicTacToeAPI.Model;
using Newtonsoft.Json;
using TicTacToeAPI.Services.Interfaces;

namespace TicTacToeAPI.Services
{
    public class GameRepository : IGameRepository
    {
        private const string connectionString = "Data Source = tictactoe.db; Version = 3; Pooling = true; Max Pool Size = 100;";
        public int Create(Game game)
        {
            SQLiteConnection connection=new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command=new SQLiteCommand(connection);
            command.CommandText = @"INSERT INTO Games(Token,Field,Status)
                                    VALUES(@Token,@Field,@Status)";
            command.Parameters.AddWithValue("@Token", game.Token);
            command.Parameters.AddWithValue("@Field", JsonConvert.SerializeObject(game.Field));
            command.Parameters.AddWithValue("@Status",JsonConvert.SerializeObject(game.Status));
            command.Prepare();
            return command.ExecuteNonQuery();
        }

        public int Delete(string token)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command=new SQLiteCommand(connection);
            command.CommandText = "DELETE FROM Games WHERE Token=@Token";
            command.Prepare();
            return command.ExecuteNonQuery();
        }

        public IList<Game> GetAll()
        {
            IList<Game> list = new List<Game>();
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command=new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM Games";
            command.Prepare();
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Game game = new Game
                {
                    Token = reader.GetString(0),
                    Field = JsonConvert.DeserializeObject<int[,]>(reader.GetString(1)),
                    Status = JsonConvert.DeserializeObject<Status>(reader.GetString(2))
                };
                list.Add(game);
            }
            return list;
        }

        public Game GetById(string token)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command=new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM Games WHERE Token=@Token";
            command.Parameters.AddWithValue("@Token", token);
            command.Prepare();
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Game game = new Game {
                    Token = reader.GetString(0),
                    Field = JsonConvert.DeserializeObject<int[,]>(reader.GetString(1)),
                    Status = JsonConvert.DeserializeObject<Status>(reader.GetString(2))
                };
                connection.Close();
                return game;
            }
            return null;
        }

        public int Update(string prevuiousToken, Game game)
        {
            Game oldGame = GetById(prevuiousToken);
            if (oldGame != null)
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = @"UPDATE Games SET 
                                            Token=@Token, 
                                            Field=@Field, 
                                            Status=@Status
                                        WHERE Token=@PreviousToken";
                command.Parameters.AddWithValue("@Token", game.Token);
                command.Parameters.AddWithValue("@PreviousToken", prevuiousToken);
                command.Parameters.AddWithValue("@Field", JsonConvert.SerializeObject(game.Field));
                command.Parameters.AddWithValue("@Status",JsonConvert.SerializeObject(game.Status));
                command.Prepare();
                int res = command.ExecuteNonQuery();
                connection.Close();
                return res;
            } else
            {
                return Create(game);
            }
        }
    }
}