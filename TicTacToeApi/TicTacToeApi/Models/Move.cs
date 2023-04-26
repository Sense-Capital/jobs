namespace TicTacToeApi.Models
{
    public class Move : BaseModel
    {
        public char Val { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public Player Player { get; private set; }
        public Game Game { get; private set; }

        public Move(char val, int column, int row, Player player, Game game)
        {
            Val = val;
            setPosition(column, row);
            Player = player;
            Game = game;
        }

        private void setPosition(int column, int row)
        {
            if (column > 3 || row > 3 || column < 1 || row < 1)
            {
                Column = -1;
                Row = -1;
            }
            else
            {
                Column = column;
                Row = row;
            }
        }
    }
}
