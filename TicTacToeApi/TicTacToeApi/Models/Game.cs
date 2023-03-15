using System.Linq;

namespace TicTacToeApi.Models
{
    public class Game : BaseModel
    {
        public string Row1 { get; private set; } = "...";
        public string Row2 { get; private set; } = "...";
        public string Row3 { get; private set; } = "...";
        public string Status { get; private set; } = "Game in process. First player's turn.";
        public char Turn { get; private set; } = 'X';
        public Player? Player1 { get; set; }
        public Player? Player2 { get; set; }

        public string DoMove(Move move)
        {
            if (!Status.Contains("in process.")) return Status;
            updateBoard(move);
            updateTurn();
            updateStatus();
            return Status;
        }

        private void updateBoard(Move move)
        {
            var board = new[] { Row1, Row2, Row3 };
            char[] boardToArray = board[move.Row].ToCharArray();
            boardToArray[move.Column] = move.Val;
            board[move.Row] = string.Concat(boardToArray);
        }

        private void updateStatus()
        {
            Status = checkWinner('X') ? "First player wins."
                : checkWinner('O') ? "Second player wins."
                : checkDraw() ? "Draw." : Status;
        }

        private void updateTurn()
        {
            Turn = Turn == 'X' ? 'O' : 'X';
            Status = Turn == 'X' ? "Game in process. First player's turn." : "Game in process. Second player's turn.";
        }

        private bool checkWinner(char ch)
        {
            var board = new[] { Row1, Row2, Row3 };
            return Enumerable.Range(0, 2).Any(i =>
            Enumerable.Range(0, 2).All(j => board[i][j] == ch) ||
            Enumerable.Range(0, 2).All(j => board[j][i] == ch)) || 
            Enumerable.Range(0, 2).All(i => board[i][i] == ch) ||
            Enumerable.Range(0, 2).All(i => board[i][2 - i] == ch);
        }
        
        private bool checkDraw()
        {
            var board = new[] {Row1, Row2, Row3};
            return string.Concat(board).Contains('.');
        }
    }
}
