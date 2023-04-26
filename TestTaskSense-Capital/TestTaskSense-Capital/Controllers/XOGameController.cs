using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestTaskSense_Capital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XOGameController : ControllerBase
    {
        private XOGame game = new XOGame();
        [Route("/api/[controller]/start")]
        [AcceptVerbs("GET")]
        public IActionResult StartGame()
        {
            game = new XOGame();
            game.SaveGame();
            return Ok("Game started");
        }

        [HttpGet]
        public IActionResult Get()
        {
            game.LoadGame();
            XOGame.XOGameStatus status = new XOGame.XOGameStatus();
            status.field = game.field;
            if (game.IsVinner(game.activeTurn))
            {
                status.status = $"{game.activeTurn} Is Won";
            }
            else
            {
                status.status = $"{game.activeTurn} turn";
            }
            return Ok(JsonConvert.SerializeObject(status));
        }
        [HttpPost]
        public IActionResult Post(JObject _payload)
        {
            XOGame.XOGameTurn turn;
            try
            {
                turn = JsonConvert.DeserializeObject<XOGame.XOGameTurn>(_payload.ToString());
            }
            catch(Exception exeption)
            {
                return BadRequest($"status: invalid data; messege: {exeption.Message}");
            }
            XOGame.XOGameStatus status = new XOGame.XOGameStatus();
            game.LoadGame();
            status.field = game.field;
            try
            {
                game.MakeTurn(turn);
            }
            catch(Exception exeption)
            {
                string messege = exeption.Message;
                string code = messege[0].ToString();
                if (code == "1")
                {
                    status.status = $"{messege}; {game.activeTurn} turn";
                    return BadRequest(JsonConvert.SerializeObject(status));
                }
                if (code == "2")
                {
                    status.status = $"{messege}; Coordinates must be in range 1-3 and cell must be empty; {game.activeTurn} turn";
                    return BadRequest(JsonConvert.SerializeObject(status));
                }
            }
            status.field = game.field;
            if (game.IsVinner(game.activeTurn))
            {
                status.status = $"{game.activeTurn} Is Won";
            }
            else
            {
                status.status = $"{game.activeTurn} turn";
            }
            game.ChangeActive();
            return Ok(JsonConvert.SerializeObject(status));
        }
    }
}