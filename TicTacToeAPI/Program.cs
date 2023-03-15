using System.Data.SQLite;
using TicTacToeAPI.Services;
using TicTacToeAPI.Services.Interfaces;

namespace TicTacToeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            
            builder.Services.AddControllers();

            ConfigureSqliteConnection();

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureSqliteConnection()
        {
            string connectionString = "Data Source = tictactoe.db; Version = 3; Pooling = true; Max Pool Size = 100;";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
            connection.Close();
        }

        private static void PrepareSchema(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand(connection);

            //command.CommandText = "DROP TABLE IF EXISTS Users";
            //command.ExecuteNonQuery();
            //command.CommandText = "DROP TABLE IF EXISTS Games";
            //command.ExecuteNonQuery();

            command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Users(
                    Email TEXT PRIMARY KEY,
                    Hash TEXT,
                    Token TEXT)";
            command.ExecuteNonQuery();

            command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Games(
                    Token TEXT PRIMARY KEY,
                    Field TEXT,
                    Status TEXT)";
            command.ExecuteNonQuery();
        }
    }
}