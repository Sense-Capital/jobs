using Newtonsoft.Json;
using System;
using System.IO;

public class XOGame
{
    public String[,] field = new string[3, 3];
    public String activeTurn = "X";
    public void SaveGame()
    {
        File.WriteAllText("./gameState.txt", JsonConvert.SerializeObject(this));
    }
    public void LoadGame()
    {
        XOGame tmp =  JsonConvert.DeserializeObject<XOGame>(File.ReadAllText("./gameState.txt"));
        this.field = tmp.field;
        this.activeTurn = tmp.activeTurn;
    }
    public XOGame()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                field[i, j] = " ";
            }
        }
    }
    public bool IsVinner(String _turnChar)
    {
        for (int i = 0; i < 3; i++)
        {
            if (field[i, 0] == field[i, 1] && field[i, 1] == field[i, 2] && field[i, 0] == _turnChar)
            {
                return true;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (field[0, i] == field[1, i] && field[1, i] == field[2, i] && field[0, i] == _turnChar)
            {
                return true;
            }
        }
        if (field[0, 0] == field[1, 1] && field[1, 1] == field[2, 2] && field[0, 0] == _turnChar)
        {
            return true;
        }
        if (field[2, 0] == field[1, 1] && field[1, 1] == field[0, 2] && field[1, 1] == _turnChar)
        {
            return true;
        }
        return false;
    }
    public void ChangeActive()
    {
        if (this.activeTurn == "X")
        {
            this.activeTurn = "O";
        }
        else
        {
            this.activeTurn = "X";
        }
        this.SaveGame();
    }
    public void MakeTurn(XOGameTurn _turn)
    {
        this.LoadGame();
        if(_turn.turnChar != this.activeTurn)
        {
            throw new Exception("1: wrong turn");
        }
        if(_turn.x>3 || _turn.x<1 || _turn.y<1 || _turn.y>3)
        {
            throw new Exception("2: wrong position");
        }
        _turn.x -= 1;
        _turn.y = 2 - (_turn.y - 1);
        if(this.field[_turn.y,_turn.x] != " ")
        {
            throw new Exception("2: wrong position");
        }
        this.field[_turn.y, _turn.x] = this.activeTurn;
        this.SaveGame();
    }
    public class XOGameStatus
    {
        public String[,] field;
        public String status;
    }
    public class XOGameTurn
    {
        public int x;
        public int y;
        public string turnChar;
    }

}