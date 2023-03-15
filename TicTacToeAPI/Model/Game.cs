namespace TicTacToeAPI.Model
{
    public class Game
    {
        public string Token { get; set; }
        public int[,] Field { get; set; }
        public Status Status { get; set; }
    }
}