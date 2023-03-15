using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using TicTacToeAPI.Model;
using TicTacToeAPI.Services.Interfaces;

namespace TicTacToeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private int fieldSize;
        private IUserRepository _userRepository;
        private IGameRepository _gameRepository;

        public GameController(IUserRepository userRepository, IGameRepository gameRepository)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            fieldSize = 3;
        }

        [HttpPut("login")]
        public ActionResult<string> LogIn(string email, string password)
        {
            User user = _userRepository.GetById(email);
            if (user == null)
            {
                string token = GetHash(email+password);
                User newUser = new User();
                newUser.Email = email;
                newUser.Hash = GetHash(password);
                newUser.Token = token;
                _userRepository.Create(newUser);
                _gameRepository.Create(new Game
                {
                    Token = token,
                    Field = new int[fieldSize,fieldSize],
                    Status = Status.Make_a_Move
                });
                return Ok(token);
            }
            if (VerifyHash(password, user.Hash))
            {
                return Ok(user.Token);
            }
            return BadRequest("Incorrect Password");
        }

        [HttpGet("status")]
        public ActionResult<string> GetGameStatus(string token) 
        {
            Game game = _gameRepository.GetById(token);
            if (game == null)
            {
                return BadRequest("Game Not Found");
            }
            return Ok(JsonConvert.SerializeObject(game));
        }

        [HttpGet("new-game")]
        public ActionResult<string> GetNewGame(string token)
        {
            Game game = _gameRepository.GetById(token);
            if (game == null)
            {
                return BadRequest("You must log in");
            }
            else
            {
                game.Field= new int[fieldSize,fieldSize];
                game.Status= Status.Make_a_Move;
                int res = _gameRepository.Update(game.Token,game);
                if (res == 1)
                {
                    return GetGameStatus(token);
                } else
                {
                    return BadRequest("Inner Server Error. Please Try Later");
                }
            }
        }

        [HttpGet("get-move")]
        public ActionResult<string> GetMove(string token, int raw, int column)
        {

            //TODO check for empty cell

            Game game = _gameRepository.GetById(token);
            if (game == null)
            {
                return BadRequest("You must log in");
            }
            if (raw > fieldSize-1 || column>fieldSize-1 || raw<0 || column <0)
            {
                return BadRequest("Wrong field size");
            }
            if (game.Status!= Status.Make_a_Move) 
            {
                return BadRequest("Game is over. You need to start new game");
            }
            if (game.Field[raw, column] != 0)
            {
                return BadRequest("Field is occupied");
            }
            game.Field[raw,column] = 1;
            game.Status = CheckStatus(game);

            ComputerMove(game);

            int res = _gameRepository.Update(game.Token,game);
            if (res == 1)
            {
                return GetGameStatus(token);
            }
            else
            {
                return BadRequest("Inner Server Error. Please Try Later");
            }
        }

        private void ComputerMove(Game game)
        {
            while (true)
            {
                Random random = new Random();
                int raw = random.Next(0, fieldSize);
                int column = random.Next(0, fieldSize);
                if (game.Field[raw, column] == 0)
                {
                    game.Field[raw, column] = 2;
                    game.Status = CheckStatus(game);
                    break;
                }
            }
        }

        private Status CheckStatus(Game game)
        {
            bool noMoreMove = true;
            for (int i=0;i<fieldSize-2;i++)
            {
                for (int j = 0; j < fieldSize - 2; j++)
                {
                    if (game.Field[i, j] == 0)
                    {
                        noMoreMove = false;
                    }
                    if (game.Field[i, j] != 0)
                    {
                        if (game.Field[i, j] == game.Field[i + 1, j + 1] && game.Field[i, j] == game.Field[i + 2, j + 2] && game.Field[i, j] == 1)
                        {
                            return Status.Player_Win;
                        }
                        if (game.Field[i + 2, j] == game.Field[i + 1, j + 1] && game.Field[i, j] == game.Field[i, j + 2] && game.Field[i, j] == 1)
                        {
                            return Status.Player_Win;
                        }
                        if (game.Field[i, j] == game.Field[i, j + 1] && game.Field[i, j] == game.Field[i, j + 2] && game.Field[i, j] == 1)
                        {
                            return Status.Player_Win;
                        }
                        if (game.Field[i, j] == game.Field[i + 1, j] && game.Field[i, j] == game.Field[i + 2, j] && game.Field[i, j] == 1)
                        {
                            return Status.Player_Win;
                        }

                        if (game.Field[i, j] == game.Field[i + 1, j + 1] && game.Field[i, j] == game.Field[i + 2, j + 2] && game.Field[i, j] == 2)
                        {
                            return Status.Computer_Win;
                        }
                        if (game.Field[i + 2, j] == game.Field[i + 1, j + 1] && game.Field[i, j] == game.Field[i, j + 2] && game.Field[i, j] == 2)
                        {
                            return Status.Computer_Win;
                        }
                        if (game.Field[i, j] == game.Field[i, j + 1] && game.Field[i, j] == game.Field[i, j + 2] && game.Field[i, j] == 2)
                        {
                            return Status.Computer_Win;
                        }
                        if (game.Field[i, j] == game.Field[i + 1, j] && game.Field[i, j] == game.Field[i + 2, j] && game.Field[i, j] == 2)
                        {
                            return Status.Computer_Win;
                        }
                    }
                }

            }
            
            for (int i = fieldSize - 2; i < fieldSize; i++)
            {
                for(int j=0;j< fieldSize-2;j++)
                {
                    if (game.Field[i, j] == 0)
                    {
                        noMoreMove = false;
                    }

                    if (game.Field[i, j] == game.Field[i, j + 1] && game.Field[i, j] == game.Field[i, j + 2] && game.Field[i, j] == 2)
                    {
                        return Status.Computer_Win;
                    }

                    if (game.Field[i, j] == game.Field[i, j + 1] && game.Field[i, j] == game.Field[i, j + 2] && game.Field[i, j] == 1)
                    {
                        return Status.Player_Win;
                    }
                }
            }

            for (int i=0;i< fieldSize-2;i++)
            {
                for(int j = fieldSize - 2; j < fieldSize; j++)
                {

                    if (game.Field[i, j] == 0)
                    {
                        noMoreMove = false;
                    }

                    if (game.Field[i, j] == game.Field[i+1, j] && game.Field[i, j] == game.Field[i+2, j] && game.Field[i, j] == 2)
                    {
                        return Status.Computer_Win;
                    }

                    if (game.Field[i, j] == game.Field[i+1, j] && game.Field[i, j] == game.Field[i+2, j] && game.Field[i, j] == 1)
                    {
                        return Status.Player_Win;
                    }
                }
            }

            if (noMoreMove)
            {
                return Status.Draw;
            }

            return Status.Make_a_Move;
        }

        private static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = SHA256.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyHash(string input, string hash)
        {
            HashAlgorithm hashAlgorithm = SHA256.Create();

            // Hash the input.
            var hashOfInput = GetHash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }

}