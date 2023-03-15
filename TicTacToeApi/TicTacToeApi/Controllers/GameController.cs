using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToeApi.DataBases;
using TicTacToeApi.Models;

namespace TicTacToeApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class GameController : ControllerBase
    {
        private ApplicationContext context;

        public GameController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("AllGames")]
        public JsonResult GetAllGames()
        {
            return new JsonResult(context.Games.Include(g => g.Player1)
            .Include(g => g.Player2).Select(g => g));
        }

        [HttpGet]
        [Route("GetGame")]
        public async Task<JsonResult> GetGameAsync(int id)
        {
            var game = await context.Games.Include(g => g.Player1)
                .Include(g => g.Player2).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null) return new JsonResult("The game doesn't exist.");
            return new JsonResult(game);
        }

        [HttpPost]
        [Route("NewGame")]
        public async Task<JsonResult> NewGameAsync(string name1, string name2)
        {
            var player1 = await context.Players.FirstOrDefaultAsync(p => p.Name == name1);
            var player2 = await context.Players.FirstOrDefaultAsync(p => p.Name == name2);
            if (name1 == name2 || player1 == null || player2 == null) return new JsonResult("Invalid parameters.");
            var game = new Game() { Player1 = player1, Player2 = player2 };
            context.Games.Add(game);
            context.SaveChanges();
            return new JsonResult(game);
        }

        [HttpPost]
        [Route("NewPlayer")]
        public async Task<JsonResult> NewPalyerAsync(string name)
        {
            if (await context.Players.FirstOrDefaultAsync(p => p.Name == name) != null)
                return new JsonResult("This username is already taken.");
            var player = new Player(name);
            context.Players.Add(player);
            context.SaveChanges();
            return new JsonResult(player);
        }

        [HttpGet]
        [Route("Player")]
        public async Task<JsonResult> GetPlayerAsync(int id)
        {
            var player = await context.Players.FirstOrDefaultAsync(p => p.Id == id);
            if (player == null) return new JsonResult("The player doesn't exist.");
            return new JsonResult(player);
        }

        [HttpPost]
        [Route("DoMove")]
        public async Task<JsonResult> DoMoveAsync(string playerName, char moveType, int gameId, int column, int row)
        {
            var game = await context.Games.Include(g => g.Player1)
                .Include(g => g.Player2).FirstOrDefaultAsync(g => g.Id == gameId);
            if (game == null) return new JsonResult("The game doesn't exist.");
            var player = await context.Players.FirstOrDefaultAsync(p => p.Name == playerName);
            if (player == null) return new JsonResult("The player doesn't exist.");
            Move move = new Move(moveType, column, row, player, game);
            if (!checkTurn(move)) return new JsonResult("Wrong turn.");
            game.DoMove(move);
            context.SaveChanges();
            return new JsonResult(game);
        }

        [HttpDelete]
        [Route("DeletePlayer")]
        public async Task<JsonResult> DeletePlayerAsync(int id)
        {
            var player = await context.Players.FirstOrDefaultAsync(p => p.Id == id);
            if (player == null) return new JsonResult("The player doesn't exist.");
            if (await context.Games.FirstOrDefaultAsync(g => g.Player1 == player || g.Player2 == player) != null)
            {
                return new JsonResult("Unable to delete this player.");
            }
            context.SaveChanges();
            return new JsonResult("Player has deleted.");
        }

        [HttpDelete]
        [Route("DeleteGame")]
        public async Task<JsonResult> DeleteGameAsync(int id)
        {
            var game = await context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game == null) return new JsonResult("The game doesn't exist.");
            context.Games.Remove(game);
            context.SaveChanges();
            return new JsonResult("The game has deleted.");
        }

        private bool checkTurn(Move move)
        {
            if (move.Column < 0 || move.Val != move.Game.Turn) return false;
            if (!checkPlayer(move)) return false;
            var board = new[] { move.Game.Row1, move.Game.Row2, move.Game.Row3 };
            if (board[move.Row][move.Column] != '.') return false;
            return true;
        }

        private bool checkPlayer(Move move)
        {
            int playerNumber = 0;
            if (move.Player == move.Game.Player1) playerNumber = 1;
            else if (move.Player == move.Game.Player2) playerNumber = 2;
            if (playerNumber == 0) return false;
            if (playerNumber == 1 && move.Val != 'X' || playerNumber == 2 && move.Val != 'O') return false;
            return true;
        }
    }
}
