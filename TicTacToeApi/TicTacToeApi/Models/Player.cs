namespace TicTacToeApi.Models
{
    public class Player : BaseModel
    {
        public string Name { get; private set; }

        public Player(string name)
        {
            Name = name;
        }
    }
}
