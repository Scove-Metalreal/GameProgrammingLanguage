namespace SEMI_FINAL;

public class Player
{
    public string PlayerID { get; set; }
    public string Name { get; set; }
    public int Gold { get; set; }
    public int Score { get; set; }

    public Player()
    {
        this.PlayerID = "";
        this.Name = "";
        this.Gold = 0;
        this.Score = 0;
    }

    public Player(string playerID, string name, int gold, int score)
    {
        this.PlayerID = playerID;
        this.Name = name;
        this.Gold = gold;
        this.Score = score;
    }

    public override string ToString()
    {
        return $"PLayer ID: {PlayerID}, Name: {Name}, Gold: {Gold}, Score: {Score}";
    }
}