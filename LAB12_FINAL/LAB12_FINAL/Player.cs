namespace LAB12_FINAL;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int Gold { get; set; }
    public int Coins { get; set; }
    public bool IsActive { get; set; }
    public int VipLevel { get; set; }
    public string Region { get; set; }
    public DateTime LastLogin { get; set; }
}

public class InactivePLayer
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastLogin { get; set; }
}

public class LowLevelPLayer
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int CurrentGold { get; set; }
}

public class VipPlayerAward
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int VipLevel { get; set; }
    public int CurrentGold { get; set; }
    public int AwardedGoldAmount { get; set; }
    public int Rank { get; set; }
}