using System.Runtime.InteropServices.JavaScript;

namespace Lab11;

public class Player(string name, int gold, int coins, int vipLevel, string region, DateTime lastLogin)
{
    public string Name { get; set; } = name;
    public int Gold { get; set; } = gold;
    public int Coins { get; set; } = coins;
    public int VipLevel { get; set; } = vipLevel;
    public string Region { get; set; } = region;
    public DateTime LastLogin { get; set; } = lastLogin;

    public Player() : this("", 0, 0, 0, null, DateTime.Now)
    {
    }
}