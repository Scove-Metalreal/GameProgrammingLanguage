using System.Text;
using Firebase.Database;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace LAB12_FINAL;

class Program
{
    private const string FIREBASE_DB_URL =
        "https://lab12-final-2a21d-default-rtdb.asia-southeast1.firebasedatabase.app/";
    
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(
                    "G:\\GameProgrammingLanguage\\LAB12_FINAL\\lab12-final.json")
            });
        }
        
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        
        List<Player> players = await LoadPLayers();
        
        List<InactivePLayer> inactivePLayers = await AnalyzeInactivePlayers(players);
        await firebase.Child("final_exam_bai1_inactive_players")
            .PutAsync(JsonConvert.SerializeObject(inactivePLayers));
        
        List<LowLevelPLayer> lowLevelPLayers = await AnalyzeLowLevelPlayers(players);
        await firebase.Child("final_exam_bai1_low_level_players")
            .PutAsync(JsonConvert.SerializeObject(lowLevelPLayers));

        List<VipPlayerAward> vipPlayerAwards = await AnalyzeVipPlayers(players);
        await firebase.Child("final_exam_bai2_top3_vip_awards")
            .PutAsync(JsonConvert.SerializeObject(vipPlayerAwards));
    }

    static async Task<List<Player>> LoadPLayers()
    {
        string url = "https://raw.githubusercontent.com/NTH-VTC/OnlineDemoC-/refs/heads/main/lab12_players.json";
        HttpClient httpClient = new HttpClient();

        var json = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<List<Player>>(json);
    }

    static async Task<List<InactivePLayer>> AnalyzeInactivePlayers(List<Player> pLayers)
    {
        Console.WriteLine("\n1.1. DANH SÁCH NGƯỜI CHƠI KHÔNG HOẠT ĐỘNG GẦN ĐÂY");
        DateTime now  = new DateTime(2025, 06, 30, 0, 0, 0, DateTimeKind.Utc);

        var inactivePLayers = from pLayer in pLayers
            where !pLayer.IsActive || (now - pLayer.LastLogin).TotalDays >= 5
            select new InactivePLayer
            {
                Name = pLayer.Name,
                IsActive = pLayer.IsActive,
                LastLogin = pLayer.LastLogin,
            };
        
        Console.WriteLine("Kết quả:");
        Console.WriteLine("| Tên Người Chơi  | Hoạt Động |  Đăng Nhập Cuối          |");
        Console.WriteLine("|-----------------|-----------|--------------------------|");
        foreach (var player in inactivePLayers)
        {
            Console.WriteLine($"| {player.Name,-15} | {player.IsActive,-9} |     {player.LastLogin:dd/MM/yyyy HH:mm:ssZ} |");
        }
        Console.WriteLine("|-----------------|-----------|--------------------------|");
        Console.WriteLine();
        
        return inactivePLayers.ToList();
    }

    static async Task<List<LowLevelPLayer>> AnalyzeLowLevelPlayers(List<Player> pLayers)
    {
        Console.WriteLine("\n1.2. DANH SÁCH NGƯỜI CHƠI CẤP THẤP");

        var lowLevelPLayers = from player in pLayers
            where player.Level < 10
            select new LowLevelPLayer
            {
                Name = player.Name,
                Level = player.Level,
                CurrentGold = player.Gold,
            };
        
        Console.WriteLine("Kết quả:");
        Console.WriteLine("| Tên Người Chơi  | Level | Gold Hiện Tại |");
        Console.WriteLine("|-----------------|-------|---------------|");
        foreach (var player in lowLevelPLayers)
        {
            Console.WriteLine($"| {player.Name,-15} | {player.Level,-5} | {player.CurrentGold,-13} |");
        }
        Console.WriteLine("|-----------------|-------|---------------|");
        Console.WriteLine();

        return lowLevelPLayers.ToList();
    }

    static async Task<List<VipPlayerAward>> AnalyzeVipPlayers(List<Player> pLayers)
    {
        Console.WriteLine("\n2.1. TOP 3 NGƯỜI CHƠI VIP CẤP ĐỘ CAO NHẤT VÀ GOLD THƯỞNG ");

        var topVipPlayers = pLayers
            .Where(p => p.VipLevel > 0)
            .OrderByDescending(p => p.Level)
            .Take(3)
            .Select((pLayer, index) => new VipPlayerAward
            {
                Name = pLayer.Name,
                Level = pLayer.Level,
                VipLevel = pLayer.VipLevel,
                CurrentGold = pLayer.Gold,
                AwardedGoldAmount = (index + 1) switch
                {
                    1 => 2000,
                    2 => 1500,
                    3 => 1000,
                    _ => 0
                },
                Rank = index + 1

            }).ToList();
        
        Console.WriteLine("Kết quả:");
        Console.WriteLine("| Hạng | Tên Người Chơi | VIP Level | Level | Gold Hiện Tại | Gold Sẽ Được Thưởng |");
        Console.WriteLine("|------|----------------|-----------|-------|---------------|---------------------|");
        foreach (var player in topVipPlayers)
        {
            Console.WriteLine($"| {player.Rank,-4} | {player.Name,-14} | {player.VipLevel,-9} | {player.Level,-5} | {player.CurrentGold,-13} | {player.AwardedGoldAmount,-19} |");
        }
        Console.WriteLine("|------|----------------|-----------|-------|---------------|---------------------|");
        Console.WriteLine();



        return topVipPlayers.ToList();
    }
}